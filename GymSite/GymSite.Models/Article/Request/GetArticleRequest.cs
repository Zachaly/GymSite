namespace GymSite.Models.Article.Request
{
    public class GetArticleRequest
    {
        public string? CreatorId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set;}
    }
}
