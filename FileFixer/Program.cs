using System;
using System.IO;
using System.Linq;

namespace FileFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirPath = Console.ReadLine();
            var listOfFiles = Directory.GetFiles(dirPath).Where(p => Path.GetExtension(p) == ".sgm").ToList();
            foreach (var file in listOfFiles)
            {
                var text = File.ReadAllLines(file);
                using (var writer = new StreamWriter(new FileStream(file, FileMode.OpenOrCreate)))
                {
                    writer.WriteLine(text[0]);
                    writer.WriteLine("<root>");
                    for (int i = 1; i < text.Length; i++)
                    {
                        writer.WriteLine(text[i]);
                    }
                    writer.WriteLine("</root>");
                }
            }
        }
    }
}
