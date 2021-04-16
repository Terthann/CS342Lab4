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
            Console.WriteLine(CreateReport(EnumerateFilesRecursively("D:\\CS342Lab4")));
            CreateReport(EnumerateFilesRecursively("D:\\CS342Lab4")).Save("D:\\CS342Lab4\\index.html");
            Console.ReadLine();
        }

        // Enumerate all files in a given folder recursively. (Including entire sub-folder hierarchy)
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            if (Directory.EnumerateDirectories(path).Count() > 0)
            {
                foreach (string d in Directory.EnumerateDirectories(path))
                    foreach (string s in EnumerateFilesRecursively(d))
                        yield return s;
            }

            //foreach (string d in Directory.EnumerateDirectories("D:\\CS342Lab4"))
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
            return new XDocument(new XElement("table",
                new XElement("tr",
                    new XElement("th", "Type"),
                    new XElement("th", "Count"),
                    new XElement("th", "Size")),
                from f in files.AsEnumerable().GroupBy(file => Path.GetExtension(file)).OrderBy(a => a.Sum(file => new FileInfo(file).Length)).Reverse() select
                    new XElement("tr",
                        new XElement("td", f.Count()),
                        new XElement("td", f.Key),
                        new XElement("td", f.Sum(file => new FileInfo(file).Length)))));
        }
    }
}