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
using MlCodeSearcherModel;

namespace CodeCommentAnalyzer;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        // Options
        var inputPattern = new Option<string>(
            "--inputPattern",
            "The input path pattern to scan for C# source files.")
            {
                IsRequired = true
            };
        var methodDocFilePath = new Option<string>(
            "--methodDocFilePath",
            "Path to the method documentation file.")
            {
                IsRequired = true
            };
        var request = new Option<string>(
            "--request",
            "The input description that will be used to determine if the" +
            "functionality is already implemented.");
        var numberOfResults = new Option<int>(
            "--numberOfResults",
            "Number of results to show.")
            {
                IsRequired = true
            };

        // Extract command
        var extractCommand = new Command(
            "extract",
            "Extracts documentation from C# source files.")
            {
                inputPattern,
                methodDocFilePath
            };
        
        extractCommand.SetHandler((input, output) =>
            {
                ExtractCodeComments(input, output);
            },
            inputPattern,
            methodDocFilePath);
        
        // Search command
        var searchCommand = new Command(
            "search",
            "Searchs if a functionality is already implemented.")
            {
                methodDocFilePath,
                request,
                numberOfResults
            };
        
        searchCommand.SetHandler((path, userInput, number) =>
            {
                ExecuteSearch(path, userInput, number);
            },
            methodDocFilePath,
            request,
            numberOfResults);

        // Root options
        var root = new RootCommand("ML code searcher.")
            {
                extractCommand,
                searchCommand
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
        int selected = -1;
        var extractors = CodeCommentExtractorHelper.GetAvailableExtractors();

        // Select code comment extractor
        Console.WriteLine("--- Available extractors:");

        for (int i = 0; i < extractors.Count; i++)
        {
            Console.WriteLine($"[{i}] - {extractors[i].ExtractorName}");
        }

        Console.Write("\nSelect extractor: ");
        
        if (!int.TryParse(Console.ReadLine(), out selected) ||
            selected < 0 ||
            selected > extractors.Count - 1)
        {
            throw new Exception("Extractor selection - Invalid option.");
        }

        Console.WriteLine("Extracting method comments to file...");
        extractors[selected].ExtractMethodComments(inputPattern, outputFile);
        Console.WriteLine("Done.");
    }

    /// <summary>
    /// Method that implements the "Search" command behaviour.
    /// </summary>
    /// <param name="path">Path to the JSON file that will be the input
    /// for the ML.NET pipeline.</param>
    /// <param name="userInput">The user input description.</param>
    /// <param name="numberOfResults">Number of results to show.</param>
    private static void ExecuteSearch(
        string path, string userInput, int numberOfResults)
    {
        Console.WriteLine("Initializing model...");
        MlCodeSearcherModelHelper.InitializeModel(path);
        Console.WriteLine("Done.\n");

        if (!string.IsNullOrEmpty(userInput))
        {
            PrintUserInputSimilarities(
                MlCodeSearcherModelHelper.GetSimilarities(userInput),
                numberOfResults);
        }
        else
        {
            do
            {
                Console.Write("\n--- User input: ");
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    PrintUserInputSimilarities(
                        MlCodeSearcherModelHelper.GetSimilarities(input),
                        numberOfResults);
                }

                Console.Write("'c' to continue, any other key to exit: ");
            }
            while (Console.ReadKey().KeyChar == 'c');

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Method that prints in console the similarities results when searching
    /// for functionality.
    /// </summary>
    /// <param name="similarities">The similarities results returned from model.</param>
    /// <param name="numberOfResults">Number of results to show.</param>
    private static void PrintUserInputSimilarities(
        IEnumerable<(MethodDocumentation MethodDoc, double Similarity)> similarities,
        int numberOfResults)
    {
        var validSimilarities = similarities.Where(x => x.Similarity > 0);
        var results = validSimilarities.OrderByDescending(
            x => x.Similarity).Take(numberOfResults).ToList();

        for (int i = 0; i < results.Count; i++)
        {
            Console.Write($"--- Result #{i} - ");
            PrintSimilarityResultEvaluation(results[i].Similarity);
            Console.WriteLine();
            Console.WriteLine($"Similarity: {results[i].Similarity}");
            Console.WriteLine(results[i].MethodDoc);
        }
    }

    /// <summary>
    /// Method that prints the similarity result evaluation from very high to low.
    /// </summary>
    /// <param name="similarity">The similarity result.</param>
    private static void PrintSimilarityResultEvaluation(double similarity)
    {
        if (similarity > 0.9)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("VERY HIGH similarity");
        }
        else if (similarity > 0.7)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("HIGH similarity");
        }
        else if (similarity > 0.5)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("MODERATE similarity");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("LOW similarity");
        }

        Console.ResetColor();
    }
}
