namespace GymSite.Database.Repository.Abstractions
{
    public interface IExerciseRecordRepository
    {
        Task AddRecordAsync(ExerciseRecord record);
        Task RemoveRecordByIdAsync(int id);
    }
}
