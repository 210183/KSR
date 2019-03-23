using FileSamplesRead.Exceptions;
using FileSamplesRead.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FileSamplesRead
{
    //TODO: implement reading just next
    public class DataSamplesReader
    {
        public List<RawSample> ReadAllSamples(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileReaderException($"File not found under: {filePath}");
            }
            else
            {
                List<RawSample> samples = new List<RawSample>();
                using (var reader = new XmlTextReader(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name.ToUpperInvariant() == "TITLE")
                            {
                                reader.Read();
                                string title = reader.Value;
                                string dateline = ReadTextElement(reader, "dateline");
                                string body = ReadTextElement(reader, "body");
                                samples.Add(new RawSample(
                                    new ArticleSample(title, dateline, body), 
                                    null));
                            }
                        }
                    }
                }
                return samples;
            }
        }

        private string ReadTextElement(XmlTextReader reader, string elementName)
        {
            while (!string.Equals(reader.Name, elementName, StringComparison.InvariantCultureIgnoreCase))
            {
                reader.Read();
            }
            reader.Read();
            return reader.Value;
        }
    }
}
