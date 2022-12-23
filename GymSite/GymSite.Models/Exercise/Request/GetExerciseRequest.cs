namespace GymSite.Models.Exercise.Request
{
    public class GetExerciseRequest
    {
        public string UserId { get; set; }
        public IEnumerable<int>? FilterIds { get; set; }
    }
}
