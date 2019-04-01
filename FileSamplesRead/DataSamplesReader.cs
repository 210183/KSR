using Core.Models;
using Core.Models.Concrete;
using FileSamplesRead.Exceptions;
using FileSamplesRead.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace FileSamplesRead
{
    public class DataSamplesReader
    {
        public List<RawSample> ReadAllSamples(string filePath, string labelName)
        {
            if (!File.Exists(filePath))
            {
                throw new FileReaderException($"File not found under: {filePath}");
            }
            else
            {
                List<RawSample> samples = new List<RawSample>();
                List<Label> labels = new List<Label>();
                using (var reader = new XmlTextReader(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (string.Equals(reader.Name, labelName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                labels = new List<Label>();
                                while (reader.Read())
                                {
                                    while (reader.Name.ToUpperInvariant() == "D")
                                    {
                                        reader.Read();
                                    }

                                    if(!string.IsNullOrWhiteSpace(reader.Value))
                                        labels.Add(new Label(reader.Value));

                                    if (reader.NodeType == XmlNodeType.EndElement 
                                        && string.Equals(reader.Name, $"{labelName}",
                                            StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (reader.Name.ToUpperInvariant() == "TITLE")
                            {
                                reader.Read();
                                string title = reader.Value;
                                string dateline = ReadTextElement(reader, "dateline");
                                string body = ReadTextElement(reader, "body");
                                if (title != null && dateline != null && body != null)
                                {
                                    if (!labels.Any())
                                    {
                                        labels.Add(new Label(Core.WellKnownNames.UnknownLabelName));
                                    }
                                    samples.Add(new RawSample(
                                        new ArticleSample(title, dateline, body), 
                                        new LabelsCollection(labels)));
                                }
                            }
                        }
                    }
                }
                return samples;
            }
        }

        private string ReadTextElement(XmlTextReader reader, string elementName)
        {
            bool readerState = true;
            while (!string.Equals(reader.Name, elementName, StringComparison.InvariantCultureIgnoreCase)
                && readerState)
            {
                readerState = reader.Read();
            }

            if (readerState)
            {
                reader.Read();
                return reader.Value;
            }

            return null;
        }
    }
}
