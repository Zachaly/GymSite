namespace GymSite.Domain.Entity
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
    }
}
