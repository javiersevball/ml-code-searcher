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

using System.Text.Json;

namespace CodeCommentExtractor;

/// <summary>
/// Class that represents an input parameter of a method.
/// </summary>
public class MethodParameter
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Comment { get; set; }
}

/// <summary>
/// Class that contains the information of the return type of a method.
/// </summary>
public class MethodReturn
{
    public string Type { get; set; }
    public string Comment { get; set; }
}

/// <summary>
/// Class that contains the doc. information of a method.
/// </summary>
public class MethodDocumentation
{
    public string ClassName { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public List<MethodParameter> Parameters { get; set; }
    public List<MethodReturn> Returns { get; set; }

    /// <summary>
    /// Method that serializes a list of method documentation items
    /// to a JSON file.
    /// </summary>
    /// <param name="docs">The documentation items to serialize.</param>
    /// <param name="path">Path to the output file.</param>
    public static void Serialize(
        List<MethodDocumentation> docs, string path)
    {
        string jsonStr = JsonSerializer.Serialize(
            docs,
            new JsonSerializerOptions { WriteIndented = true });
        
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// Method that deserializes a list of method documentation items
    /// from a JSON file.
    /// </summary>
    /// <param name="path">Path to the input file.</param>
    /// <returns>A list with all the deserialized items.</returns>
    public static List<MethodDocumentation> Deserialize(
        string path)
    {
        string text = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<MethodDocumentation>>(text);
    }
}
