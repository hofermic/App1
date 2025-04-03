using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class FileScanner
{
    static void Main(string[] args)
    {
        string rootDirectory = @"C:\Users\mikeh\OneDrive\Pictures";
        if (args.Length > 0)
        {
            rootDirectory = args[0];
            //validate the directory exists
            if (!Directory.Exists(rootDirectory))
            {
                Console.WriteLine("Directory does not exist");
                return;
            }
        }

        var fileCounts = new Dictionary<string, (int Count, long Size)>();

        // Recursively scan files
        foreach (var file in Directory.EnumerateFiles(rootDirectory, "*", SearchOption.AllDirectories))
        {
            string extension = Path.GetExtension(file).ToLower();

            if (!fileCounts.ContainsKey(extension))
            {
                fileCounts[extension] = (0, 0);
            }

            var (count, size) = fileCounts[extension];
            fileCounts[extension] = (count + 1, size + new FileInfo(file).Length);
        }

        // Format and display results
        Console.WriteLine("File Type\tCount\tSize (MB)");
        Console.WriteLine("--------------------------------");

        foreach (var kvp in fileCounts.OrderByDescending(x => x.Value.Size))
        {
            Console.WriteLine($"{kvp.Key}\t{kvp.Value.Count}\t{kvp.Value.Size / (1024 * 1024):F2}");
        }

        // Total size calculation
        long totalSizeBytes = fileCounts.Sum(x => x.Value.Size);
        double totalSizeMB = totalSizeBytes / (1024 * 1024);
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Total:\t{fileCounts.Sum(x => x.Value.Count)}\t{totalSizeMB:F2}");
    }
}