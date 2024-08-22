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
}

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
}

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
}
