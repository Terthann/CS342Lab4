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
            // Command Line Arguments
            if (args.Length < 2)
            {
                Console.WriteLine("Please enter two file paths.");
                return;
            }

            //Console.WriteLine("Please enter a file path to evaluate: ");
            string filePath1 = args[0];//Console.ReadLine();
            //Console.WriteLine("Please enter a file for the report: ");
            string filePath2 = args[1];//Console.ReadLine();

            // Writes the report to the console.
            Console.WriteLine(CreateReport(EnumerateFilesRecursively(filePath1)));//"D:\\CS342Lab4"
            // Saves the report to the specified path.
            CreateReport(EnumerateFilesRecursively(filePath1)).Save(filePath2);//"D:\\CS342Lab4" "D:\\CS342Lab4\\index.html"
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

        // Format a byte size in human readable form.
        // B, kB, MB, GB, TB, PB, EB, and ZB where 1kB = 1000B, etc.
        // The numerical value should be greater or equal to 1, less than 1000,
        // and rounded to 2 digits after the decimal point. (e.g. "1.30kB")
        static string FormatByteSize(long byteSize)
        {
            long size = byteSize;
            long decimalPoint = 0;
            string leadingZero = "";
            string sizeIndicator = "";

            // Change to kB if able.
            if (size > 1000)
            {
                decimalPoint = byteSize % 1000;
                size = byteSize / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "kB";
            }
            else
            {
                sizeIndicator = "B";
            }
            // Format to MB if able.
            if (size > 1000)
            {
                decimalPoint = size % 1000;
                size = size / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "MB";
            }
            // Format to GB if able.
            if (size > 1000)
            {
                decimalPoint = size % 1000;
                size = size / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "TB";
            }
            // Format to PB if able.
            if (size > 1000)
            {
                decimalPoint = size % 1000;
                size = size / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "PB";
            }
            // Format to EB if able.
            if (size > 1000)
            {
                decimalPoint = size % 1000;
                size = size / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "EB";
            }
            // Format to ZB if able.
            if (size > 1000)
            {
                decimalPoint = size % 1000;
                size = size / 1000;

                if (decimalPoint % 10 < 5)
                    decimalPoint = decimalPoint / 10;
                else
                    decimalPoint = (decimalPoint / 10) + 1;

                if (decimalPoint < 10)
                    leadingZero = "0";
                else
                    leadingZero = "";

                sizeIndicator = "ZB";
            }

            return size.ToString() + "." + leadingZero + decimalPoint.ToString() + sizeIndicator;
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
                        new XElement("td", f.Key.ToLower()), 
                        new XElement("td", f.Count()),
                        new XElement("td", FormatByteSize(f.Sum(file => new FileInfo(file).Length))))));
        }
    }
}