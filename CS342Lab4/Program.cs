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
            Console.ReadLine();
        }

        // Enumerate all files in a given folder recursively. (Including entire sub-folder hierarchy)
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            foreach (string s in Directory.EnumerateFiles(path))
                yield return s;
        }

        static string FormatByteSize(long byteSize)
        {
            return "";
        }

        static XDocument CreateReport(IEnumerable<string> files)
        {
            return new XDocument(new XElement("table", new XElement("tr",
                from f in files.AsEnumerable() select
                    new XElement("th", new FileInfo(f).Name))));
        }
    }
}