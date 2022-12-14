namespace GymSite.Models.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Dictionary<string, IEnumerable<string>>? ValidationErrors { get; set; }
    }
}
