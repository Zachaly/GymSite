namespace GymSite.Models.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ResponseCode Code { get; set; }
        public Dictionary<string, IEnumerable<string>>? Errors { get; set; }
    }
}
