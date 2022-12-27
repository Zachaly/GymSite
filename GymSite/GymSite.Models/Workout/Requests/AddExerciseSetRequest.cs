namespace GymSite.Models.Workout.Requests
{
    public class AddExerciseSetRequest
    {
        public int WorkoutExerciseId { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
    }
}
