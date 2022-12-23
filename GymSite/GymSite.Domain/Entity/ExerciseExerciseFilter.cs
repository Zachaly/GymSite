namespace GymSite.Domain.Entity
{
    public class ExerciseExerciseFilter
    {
        public int ExerciseId { get; set; }
        public int FilterId { get; set; }
        public ExerciseFilter Filter { get; set; }
    }
}
