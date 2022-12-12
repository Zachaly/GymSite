namespace GymSite.Models.Exercise.Request
{
    public class AddExerciseRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UserId { get; set; }
    }
}
