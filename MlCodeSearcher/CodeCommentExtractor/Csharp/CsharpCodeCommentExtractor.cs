// This file is part of the ML Code Searcher project.
//
// ML Code Searcher is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// ML Code Searcher is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with ML Code Searcher. If not, see <https://www.gnu.org/licenses/>.

using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeCommentExtractor.Csharp;

/// <summary>
/// Implementation of a code comment extractor for C#.
/// </summary>
public class CsharpCodeCommentExtractor : ICodeCommentExtractor
{
    /// <summary>
    /// Gets the name that identifies the extractor.
    /// </summary>
    public string ExtractorName
    {
        get { return "C# code comment extractor"; }
    }

    /// <summary>
    /// Method that extracts the code comments and saves them to an output
    /// </summary>
    /// <param name="inputCodePathPattern">The filepath pattern of the source files
    /// that will be analyzed.</param>
    /// <param name="outputFile">Path to the output file.</param>
    public void ExtractMethodComments(
        string inputCodePathPattern, string outputFile)
    {
        var comments = new List<MethodDocumentation>();

        var dir = Path.GetDirectoryName(inputCodePathPattern);
        var searchPattern = Path.GetFileName(inputCodePathPattern);

        if (dir == null || searchPattern == null)
        {
            throw new Exception("Invalid input path pattern.");
        }

        foreach (var filePath in
            Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories))
        {
            comments.AddRange(
                ExtractMethodCommentsFromFile(filePath));
        }

        MethodDocumentation.Serialize(comments, outputFile);
    }

    /// <summary>
    /// Method that extracts the comments of the classes and methos defined
    /// in the input file.
    /// </summary>
    /// <param name="path">Path to the input file.</param>
    /// <returns>All the method documentation items read from file.</returns>
    private List<MethodDocumentation> ExtractMethodCommentsFromFile(string path)
    {
        var result = new List<MethodDocumentation>();

        // Get comments using roslyn
        var code = File.ReadAllText(path);
        var root = CSharpSyntaxTree.ParseText(code).GetCompilationUnitRoot();

        foreach (var node in root.DescendantNodes())
        {
            // Only get documentation comments for methods
            if (node is MethodDeclarationSyntax method)
            {
                // Get method leading documentation comment and its class
                var docComments = node.GetLeadingTrivia().Where(
                    x => x.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
                var classNode = method.Ancestors().OfType<ClassDeclarationSyntax>();

                if (docComments.Count() == 1 && classNode.Count() == 1)
                {
                    var comment = docComments.First();
                    var className = classNode.First().Identifier.Text;

                    // Get method info
                    var methodName = method.Identifier.Text;
                    var inputParameters = method.ParameterList.Parameters.Select(
                        x => (Type: x.Type.ToString(), Name: x.Identifier.Text)).ToList();
                    var returnType = method.ReturnType.ToString();

                    // Create the method documentation object
                    var methodDoc = CreateMethodDocumentation(
                        className, methodName, inputParameters, returnType, comment.ToString());
                    
                    result.Add(methodDoc);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Creates the method documentation object from the data extracted from code.
    /// </summary>
    /// <param name="className">The name of the class the method belongs to.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameters">The input parameters of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="comment">The documentation comment associated to the method.</param>
    /// <returns>The method documentation object that contains all the method info.</returns>
    private MethodDocumentation CreateMethodDocumentation(
        string className,
        string name,
        List<(string Type, string Name)> parameters,
        string returnType,
        string comment)
    {
        var doc = new MethodDocumentation
            {
                ClassName = className,
                Name = name
            };

        // Parse XML doc comment to get its elements
        var xml = comment.Replace("///", string.Empty).Trim();
        xml = xml.Replace("\n", string.Empty);
        xml = Regex.Replace(xml, @"\s+", " ");

        var xmlDoc = XDocument.Parse($"<root>{xml}</root>");

        doc.Comment = xmlDoc.Root.Element("summary").Value.Trim();

        // Method return info
        doc.Returns = [];

        if (xmlDoc.Root.Element("returns") != null)
        {
            var returnComment = xmlDoc.Root.Element("returns")?.Value.Trim();
            var methodReturn = new MethodReturn
                {
                    Type = returnType,
                    Comment = returnComment
                };
            doc.Returns.Add(methodReturn);
        }

        // Input parameters info
        doc.Parameters = [];

        if (parameters.Count > 0)
        {
            foreach (var inputParam in parameters)
            {
                var inputParamDoc = xmlDoc.Root.Elements("param").Where(
                    x => x.Attribute("name").Value == inputParam.Name).FirstOrDefault();
                
                if (inputParamDoc != null)
                {
                    var methodParameter = new MethodParameter
                        {
                            Name = inputParam.Name,
                            Type = inputParam.Type,
                            Comment = inputParamDoc.Value.Trim()
                        };

                    doc.Parameters.Add(methodParameter);
                }
            }
        }

        return doc;
    }
}
