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
using Microsoft.ML;

namespace MlCodeSearcherModelBuilder;

/// <summary>
/// Class that represents the ML.NET pipeline used by the application.
/// </summary>
public class MlCodeSearcherPipeline
{
    public MlCodeSearcherPipeline(
        List<MethodDocumentation> methodDocData)
    {
        var context = new MLContext();

        IDataView data = context.Data.LoadFromEnumerable(methodDocData);

        // Create the pipeline
        var pipeline = context.Transforms.Text.FeaturizeText(
            "Features", nameof(MethodDocumentation.Comment));
    }
}
