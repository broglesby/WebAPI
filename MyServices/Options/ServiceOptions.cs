namespace MyServices.Options
{
    public class ServiceOptions
    {
        public string BaseUrl { get; set; } = "https://hacker-news.firebaseio.com/v0/";
        public int MaxParallelThreads { get; set; } = 20;
    }
}
