namespace GymSite.Models.Workout.Requests
{
    public class UpdateWorkoutRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
