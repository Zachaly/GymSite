namespace GymSite.Domain.Entity
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<WorkoutExercise> Exercises { get; set; }
    }
}
