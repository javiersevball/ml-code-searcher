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

namespace CodeCommentExtractor;

/// <summary>
/// Class that contains the information of the return type of a method.
/// </summary>
public class MethodReturn : DocumentationElement
{
    public string Type { get; set; }
    public string Comment { get; set; }

    /// <summary>
    /// Method that serializes a list of method return elements.
    /// </summary>
    /// <param name="elements">The method return elements to serialize.</param>
    /// <returns>A string with the serialization of the elements.</returns>
    public static string Serialize(List<MethodReturn> elements)
    {
        return Serialize<MethodReturn>(elements);
    }

    /// <summary>
    /// Method that deserializes a list of method return elements.
    /// </summary>
    /// <param name="text">The string with the serialized elements.</param>
    /// <returns>A list with all the deserialized elements.</returns>
    public static List<MethodReturn> Deserialize(string text)
    {
        return Deserialize<MethodReturn>(text);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that contains the name and age of the person.</returns>
    public override string ToString()
    {
        return $"Type: {Type}, Comment: {Comment}";
    }
}