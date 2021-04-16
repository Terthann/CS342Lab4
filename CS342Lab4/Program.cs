using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace CS342Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Writes the report to the console.
            Console.WriteLine(CreateReport(EnumerateFilesRecursively("D:\\CS342Lab4")));
            // Saves the report to the specified path.
            CreateReport(EnumerateFilesRecursively("D:\\CS342Lab4")).Save("D:\\CS342Lab4\\index.html");
            // Pauses the console.
            Console.ReadLine();
        }

        // Enumerate all files in a given folder recursively. (Including entire sub-folder hierarchy)
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            // Check if there are any more subdirectories.
            if (Directory.EnumerateDirectories(path).Count() > 0)
            {
                // If there are, recursively call.
                foreach (string d in Directory.EnumerateDirectories(path))
                    foreach (string s in EnumerateFilesRecursively(d))
                        yield return s;
            }

            // For each file in this directory.
            foreach (string s in Directory.EnumerateFiles(path))
                yield return s;
        }

        static string FormatByteSize(long byteSize)
        {
            return "";
        }

        // Create a HTML document containing a table with 3 columns:
        // "Type"   - File name extension (lower case).
        // "Count"  - Number of files with this type.
        // "Size"   - Total size of all files with this type.
        static XDocument CreateReport(IEnumerable<string> files)
        {
            // Creates table.
            return new XDocument(new XElement("table",
                // Create row of headers.
                new XElement("tr",
                    new XElement("th", "Type"),
                    new XElement("th", "Count"),
                    new XElement("th", "Size")),
                // Create a row per file extension group. (Descending order based on "Size")
                from f in files.AsEnumerable().GroupBy(file => Path.GetExtension(file)).OrderBy(a => a.Sum(file => new FileInfo(file).Length)).Reverse() select
                    new XElement("tr",
                        new XElement("td", f.Count()),
                        new XElement("td", f.Key),
                        new XElement("td", f.Sum(file => new FileInfo(file).Length)))));
        }
    }
}