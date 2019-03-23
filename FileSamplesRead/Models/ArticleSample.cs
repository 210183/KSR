namespace FileSamplesRead.Models
{
    public class ArticleSample
    {
        public ArticleSample(string title, string dateline, string body)
        {
            Title = title;
            Dateline = dateline;
            Body = body;
        }

        public string Title { get; }
        public string Dateline { get; }
        public string Body { get; }
    }
}
