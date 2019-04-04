using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FileSamplesRead.Models;
using LemmaSharp;

namespace DataPreprocessing
{
    public class Lemmatizer
    {
        private ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);

        public PreProcessedSample LemmatizeSample(PreProcessedSample sample)
             => new PreProcessedSample(sample.Labels,
                new PreProcessedArticleSample(
                    sample.PreProcessedArticleSample.Title,
                    sample.PreProcessedArticleSample.Dateline,
                    sample.PreProcessedArticleSample.Body.Select(LemmatizeOne).ToList()
                    ));

        public string LemmatizeOne(string word) => lmtz.Lemmatize(word.ToLower());
    }
}
