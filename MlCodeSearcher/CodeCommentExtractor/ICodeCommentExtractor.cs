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
/// Interface that represents a code comment extractor. 
/// </summary>
public interface ICodeCommentExtractor
{
    /// <summary>
    /// Gets the name that identifies the extractor.
    /// </summary>
    public string ExtractorName
    {
        get;
    }

    /// <summary>
    /// Method that extracts the code comments and saves them to an output
    /// </summary>
    /// <param name="inputCodePathPattern">The filepath pattern of the source files
    /// that will be analyzed.</param>
    /// <param name="outputFile">Path to the output file.</param>
    public void ExtractMethodComments(
        string inputCodePathPattern, string outputFile);
}
