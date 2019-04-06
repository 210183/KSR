using Core.Models;
using Core.Models.Concrete;
using FileSamplesRead.Exceptions;
using FileSamplesRead.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSamplesRead
{
    public class OneLinerReader
    {
        public static List<RawSample> ReadAllSamples(string filePath, string labelName)
        {
            if (!File.Exists(filePath))
            {
                throw new FileReaderException($"File not found under: {filePath}");
            }
            else
            {
                List<RawSample> samples = new List<RawSample>();
                List<Label> labels = new List<Label>();
                var textLines = File.ReadAllLines(filePath);

                for (int i = 0; i < textLines.Length; i++)
                {
                    var sampleWithLabel = textLines[i].Split();
                    samples.Add(new RawSample(
                        new ArticleSample(
                            null,
                            null,
                            string.Join(" ", sampleWithLabel.Skip(1)))
                        ,new LabelsCollection(
                            new List<Label>{new Label(sampleWithLabel[0])}) ));
                }

                return samples;
            }
        }
    }
}
