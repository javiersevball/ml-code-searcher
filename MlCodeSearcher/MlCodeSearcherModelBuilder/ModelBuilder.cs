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

namespace MlCodeSearcherModelBuilder;

/// <summary>
/// TODO - refactor class?
/// </summary>
public static class ModelBuilder
{
    public static void GenerateModel(string inputCodeCommentsFile)
    {
        // Read input file
        var methodDocData = MethodDocumentation.Deserialize(
            inputCodeCommentsFile);

        var pipeline = new MlCodeSearcherPipeline(methodDocData);
    }
}
