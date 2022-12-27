namespace GymSite.Domain.Entity
{
    public class ExerciseSet
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public WorkoutExercise Exercise { get; set; }
        public decimal Weigth { get; set; }
        public int Reps { get; set; }
    }
}
