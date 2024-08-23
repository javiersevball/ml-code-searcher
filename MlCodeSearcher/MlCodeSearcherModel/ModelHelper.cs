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

using System.Collections.Immutable;
using CodeCommentExtractor;

namespace MlCodeSearcherModel;

/// <summary>
/// Helper class for the management of the ML.NET model.
/// </summary>
public static class MlCodeSearcherModelHelper
{
    public static IEnumerable<(MethodDocumentation MethodDoc, double Similarity)> SearchForFunctionality(
        string inputCodeCommentsFile, string userInput)
    {
        // Read input file
        var methodDocData = MethodDocumentation.Deserialize(
            inputCodeCommentsFile);
            
        var model = new MlCodeSearcherModel(methodDocData);

        return model.CalculateSimilarities(userInput).OrderByDescending(
            x => x.Similarity);
    }
}
