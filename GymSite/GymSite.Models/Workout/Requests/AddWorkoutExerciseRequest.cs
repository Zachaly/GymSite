namespace GymSite.Models.Workout.Requests
{
    public class AddWorkoutExerciseRequest
    {
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
    }
}
