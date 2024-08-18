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

using System.CommandLine;
using CodeCommentExtractor;

namespace CodeCommentAnalyzer;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Command line parser - Extract options
        var inputPattern = new Option<string>(
            "--inputPattern",
            "The input path pattern to scan for C# source files.")
            {
                IsRequired = true
            };
        var outputFile = new Option<string>(
            "--outputFile",
            "The output file path for the extracted documentation.")
            {
                IsRequired = true
            };
        var extractCommand = new Command(
            "extract",
            "Extracts documentation from C# source files.")
            {
                inputPattern,
                outputFile
            };
        
        extractCommand.SetHandler((input, output) =>
            {
                ExtractCodeComments(input, output);
            },
            inputPattern,
            outputFile);

        var root = new RootCommand("ML code searcher.")
            {
                extractCommand
            };
        
        return await root.InvokeAsync(args);
    }

    /// <summary>
    /// Method that implements the "Extract" command behaviour.
    /// </summary>
    /// <param name="inputPattern">The filepath pattern of the source files.</param>
    /// <param name="outputFile">Path to the output file.</param>
    private static void ExtractCodeComments(string inputPattern, string outputFile)
    {
        // TODO: show command line menu to select extractor
        var extractor = CodeCommentExtractorHelper.GetAvailableExtractors().First();
        extractor.ExtractMethodComments(inputPattern, outputFile);
    }
}
