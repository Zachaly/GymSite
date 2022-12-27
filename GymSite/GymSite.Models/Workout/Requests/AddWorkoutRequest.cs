namespace GymSite.Models.Workout.Requests
{
    public class AddWorkoutRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
