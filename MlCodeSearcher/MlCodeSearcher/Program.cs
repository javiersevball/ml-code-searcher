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

using System.CommandLine;
using CodeCommentExtractor;
using MlCodeSearcherModel;

namespace CodeCommentAnalyzer;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Options
        var inputPattern = new Option<string>(
            "--inputPattern",
            "The input path pattern to scan for C# source files.")
            {
                IsRequired = true
            };
        var methodDocFilePath = new Option<string>(
            "--methodDocFilePath",
            "Path to the method documentation file.")
            {
                IsRequired = true
            };
        var request = new Option<string>(
            "--request",
            "The input description that will be used to determine if the" +
            "functionality is already implemented.")
            {
                IsRequired = true
            };

        // Extract command
        var extractCommand = new Command(
            "extract",
            "Extracts documentation from C# source files.")
            {
                inputPattern,
                methodDocFilePath
            };
        
        extractCommand.SetHandler((input, output) =>
            {
                ExtractCodeComments(input, output);
            },
            inputPattern,
            methodDocFilePath);
        
        // Search command
        var searchCommand = new Command(
            "search",
            "Searchs if a functionality is already implemented.")
            {
                methodDocFilePath,
                request
            };
        
        searchCommand.SetHandler((path, userInput) =>
            {
                ExecuteSearch(path, userInput);
            },
            methodDocFilePath,
            request);

        // Root options
        var root = new RootCommand("ML code searcher.")
            {
                extractCommand,
                searchCommand
            };
        
        return await root.InvokeAsync(args);
    }

    /// <summary>
    /// Method that implements the "Extract" command behaviour.
    /// </summary>
    /// <param name="inputPattern">The filepath pattern of the source files.</param>
    /// <param name="outputFile">Path to the output file.</param>
    private static void ExtractCodeComments(string inputPattern, string outputFile)
    {
        // TODO: show command line menu to select extractor
        var extractor = CodeCommentExtractorHelper.GetAvailableExtractors().First();
        extractor.ExtractMethodComments(inputPattern, outputFile);
    }

    /// <summary>
    /// Method that implements the "Search" command behaviour.
    /// </summary>
    /// <param name="path">Path to the JSON file that will be the input
    /// for the ML.NET pipeline.</param>
    /// <param name="userInput">The user input description.</param>
    private static void ExecuteSearch(
        string path, string userInput)
    {
        var result = MlCodeSearcherModelHelper.SearchForFunctionality(path, userInput);

        // TODO
        foreach (var x in result)
        {
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Method name: {x.MethodDoc.Name}");
            Console.WriteLine($"Similarity: {x.Similarity}");
        }
    }
}
