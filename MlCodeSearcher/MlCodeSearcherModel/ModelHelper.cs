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

using CodeCommentExtractor;

namespace MlCodeSearcherModel;

/// <summary>
/// Helper class for the management of the ML.NET model.
/// </summary>
public static class MlCodeSearcherModelHelper
{
    /// <summary>
    /// Field that contains the model in use.
    /// </summary>
    private static MlCodeSearcherModel _model = null;

    /// <summary>
    /// Initializes the model with the method documentation data of the
    /// input file.
    /// </summary>
    /// <param name="inputCodeCommentsFile">Path to the JSON file that contains
    /// all the input data for the model.</param>
    public static void InitializeModel(string inputCodeCommentsFile)
    {
        var methodDocData = MethodDocumentation.Deserialize(
            inputCodeCommentsFile);
        
        _model = new MlCodeSearcherModel(methodDocData);
    }

    /// <summary>
    /// Method that gets the similarities between the user input and the code
    /// comments that describe the functionalities already implemented.
    /// </summary>
    /// <param name="userInput">The user input description.</param>
    /// <returns>The list of methods and its similarity with the user input.</returns>
    public static IEnumerable<(MethodDocumentation MethodDoc, double Similarity)> GetSimilarities(
        string userInput)
    {
        if (_model == null)
        {
            throw new Exception("Application model not initialized.");
        }

        return _model.CalculateSimilarities(userInput);
    }
}
