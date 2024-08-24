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
/// Abstract representation of a documentation element.
/// </summary>
public abstract class DocumentationElement
{
    /// <summary>
    /// Method that serializes a list of doc elements.
    /// </summary>
    /// <typeparam name="T">The type of doc elements to serialize.</typeparam>
    /// <param name="elements">The doc elements to serialize.</param>
    /// <returns>A string with the serialization of the elements.</returns>
    protected static string Serialize<T>(List<T> elements)
    {
        return JsonSerializer.Serialize(elements);
    }

    /// <summary>
    /// Method that serializes a list of doc elements to a file.
    /// </summary>
    /// <typeparam name="T">The type of doc elements to serialize.</typeparam>
    /// <param name="elements">The doc elements to serialize.</param>
    /// <param name="path">Path to the output file.</param>
    protected static void SerializeToFile<T>(List<T> elements, string path)
    {
        string jsonStr = JsonSerializer.Serialize(
            elements,
            new JsonSerializerOptions { WriteIndented = true });
        
        File.WriteAllText(path, jsonStr);
    }

    /// <summary>
    /// Method that deserializes a list of doc elements.
    /// </summary>
    /// <typeparam name="T">The type of doc elements to deserialize.</typeparam>
    /// <param name="text">The string with the serialized elements.</param>
    /// <returns>A list with all the deserialized elements.</returns>
    protected static List<T> Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<List<T>>(text);
    }

    /// <summary>
    /// Method that deserializes a list of doc elements from file.
    /// </summary>
    /// <typeparam name="T">The type of doc elements to deserialize.</typeparam>
    /// <param name="path">Path to the input file.</param>
    /// <returns>A list with all the deserialized elements.</returns>
    protected static List<T> DeserializeFromFile<T>(string path)
    {
        return Deserialize<T>(
            File.ReadAllText(path));
    }
}

