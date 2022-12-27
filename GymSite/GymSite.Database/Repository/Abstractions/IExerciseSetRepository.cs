namespace GymSite.Database.Repository.Abstractions
{
    public interface IExerciseSetRepository 
    {
        Task AddExerciseSetAsync(ExerciseSet exerciseSet);
        Task RemoveExerciseSetByIdAsync(int id);
    }
}
