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
/// Class that represents an input parameter of a method.
/// </summary>
public class MethodParameter : DocumentationElement
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Comment { get; set; }

    /// <summary>
    /// Method that serializes a list of method parameter elements.
    /// </summary>
    /// <param name="elements">The method parameter elements to serialize.</param>
    /// <returns>A string with the serialization of the elements.</returns>
    public static string Serialize(List<MethodParameter> elements)
    {
        return Serialize<MethodParameter>(elements);
    }

    /// <summary>
    /// Method that deserializes a list of method parameter elements.
    /// </summary>
    /// <param name="text">The string with the serialized elements.</param>
    /// <returns>A list with all the deserialized elements.</returns>
    public static List<MethodParameter> Deserialize(string text)
    {
        return Deserialize<MethodParameter>(text);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that contains the name and age of the person.</returns>
    public override string ToString()
    {
        return $"Name: {Name}, Type: {Type}, Comment: {Comment}";
    }
}