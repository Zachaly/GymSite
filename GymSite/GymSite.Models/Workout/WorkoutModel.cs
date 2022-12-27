namespace GymSite.Models.Workout
{
    public class WorkoutModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<WorkoutExerciseModel> Exercices { get; set; }
    }
}
