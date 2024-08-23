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
using Microsoft.ML.Data;

namespace MlCodeSearcherModel;

/// <summary>
/// Class that represents the ML.NET model used by the application.
/// </summary>
public class MlCodeSearcherModel
{
    /// <summary>
    /// Field that contains the ML.NET context.
    /// </summary>
    private MLContext _mlContext;

    /// <summary>
    /// Field that contains the ML.NET model.
    /// </summary>
    private ITransformer _model;

    /// <summary>
    /// TODO
    /// </summary>
    private List<(MethodDocumentation MethodDoc, float[] Features)> _transformedDescriptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="MlCodeSearcherModel"/> class. 
    /// </summary>
    /// <param name="methodDocData">List of method documentation elements that
    /// will be the input of the model.</param>
    public MlCodeSearcherModel(
        List<MethodDocumentation> methodDocData)
    {
        _mlContext = new MLContext();

        IDataView data = _mlContext.Data.LoadFromEnumerable(methodDocData);

        // Create the pipeline
        var pipeline = _mlContext.Transforms.Text.FeaturizeText(
            "Features", nameof(MethodDocumentation.Comment));
        
        _model = pipeline.Fit(data);

        var transformedData = _model.Transform(data);
        var features = transformedData.GetColumn<float[]>("Features").ToList();

        _transformedDescriptions = methodDocData.Zip(
            features,
            (doc, feature) => (doc, feature)).ToList();
    }

    /// <summary>
    /// Method that calculates the similarity between the input description
    /// of the user and all the features of the model.
    /// </summary>
    /// <param name="inputDescription">The user input description.</param>
    /// <returns>A list indicating the degree of similarity each model input has with
    /// the user's input.</returns>
    public IEnumerable<(MethodDocumentation MethodDoc, double Similarity)> CalculateSimilarities(
        string inputDescription)
    {
        var inputMethodDoc = new List<MethodDocumentation>
            {
                new MethodDocumentation { Comment = inputDescription }
            };
        
        var inputData = _mlContext.Data.LoadFromEnumerable(inputMethodDoc);
        var inputTransformed = _model.Transform(inputData);
        var inputFeatures = inputTransformed.GetColumn<float[]>("Features").First();

        // Calculate cosine similarity for all elements
        return _transformedDescriptions.Select(
            x => (x.MethodDoc, ComputeCosineSimilarity(inputFeatures, x.Features)));
    }

    /// <summary>
    /// Calculates the cosine similarity of two features.
    /// </summary>
    /// <param name="first">The first feature.</param>
    /// <param name="second">The second feature.</param>
    /// <returns>The cosine similarity between the two features.</returns>
    public double ComputeCosineSimilarity(float[] first, float[] second)
    {
        var product = first.Zip(second, (a, b) => a * b).Sum();
        var firstMagnitude = Math.Sqrt(first.Sum(x => x * x));
        var secondMagnitude = Math.Sqrt(second.Sum(x => x * x));

        return product / (firstMagnitude * secondMagnitude);
    }
}
