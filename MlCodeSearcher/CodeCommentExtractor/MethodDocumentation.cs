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

using System.Text;

namespace CodeCommentExtractor;

/// <summary>
/// Class that contains the doc. information of a method.
/// </summary>
public class MethodDocumentation : DocumentationElement
{
    public string ClassName { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public string Parameters { get; set; }
    public string Returns { get; set; }

    /// <summary>
    /// Method that serializes a list of method documentation elements to a file.
    /// </summary>
    /// <param name="elements">The method doc elements to serialize.</param>
    /// <param name="path">Path to the output file.</param>
    public static void Serialize(List<MethodDocumentation> elements, string path)
    {
        SerializeToFile(elements, path);
    }

    /// <summary>
    /// Method that deserializes a list of method documentation elements from file.
    /// </summary>
    /// <param name="path">Path to the input file.</param>
    /// <returns>A list with all the deserialized elements.</returns>
    public static List<MethodDocumentation> Deserialize(string path)
    {
        return DeserializeFromFile<MethodDocumentation>(path);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that contains the name and age of the person.</returns>
    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.AppendLine($"Name: {Name}, Class: {ClassName}");
        strBuilder.AppendLine($"Comment: {Comment}");
        strBuilder.AppendLine("Parameters:");

        foreach (var parameter in MethodParameter.Deserialize(Parameters))
        {
            strBuilder.AppendLine($"\t{parameter.ToString()}");
        }

        strBuilder.AppendLine("Returns:");

        foreach (var returnInfo in MethodReturn.Deserialize(Returns))
        {
            strBuilder.AppendLine($"\t{returnInfo.ToString()}");
        }

        return strBuilder.ToString();
    }
}