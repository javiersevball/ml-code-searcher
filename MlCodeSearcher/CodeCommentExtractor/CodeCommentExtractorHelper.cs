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

using System.Reflection;

namespace CodeCommentExtractor;

/// <summary>
/// Helper class that implement methods to manage the implemented extractors.
/// </summary>
public static class CodeCommentExtractorHelper
{
    /// <summary>
    /// Method that gets all the available code comment extractors.
    /// </summary>
    /// <returns>The available extractors.</returns>
    public static List<ICodeCommentExtractor> GetAvailableExtractors()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var extractorInterface = typeof(ICodeCommentExtractor);

        var extractors = types.Where(
            x => extractorInterface.IsAssignableFrom(x) &&
            x.IsClass &&
            x.Namespace.StartsWith(extractorInterface.Namespace));

        return extractors.Select(
            x => (ICodeCommentExtractor)Activator.CreateInstance(x)).ToList();
    }
}
