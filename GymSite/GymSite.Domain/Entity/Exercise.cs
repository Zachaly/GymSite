namespace GymSite.Domain.Entity
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<ExerciseRecord> Records { get; set; }
        public bool Default { get; set; }
        public ICollection<ExerciseExerciseFilter> ExerciseFilters { get; set; }
    }
}
