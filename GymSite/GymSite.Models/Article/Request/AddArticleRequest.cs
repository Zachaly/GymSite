namespace GymSite.Models.Article.Request
{
    public class AddArticleRequest
    {
        public string CreatorId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
