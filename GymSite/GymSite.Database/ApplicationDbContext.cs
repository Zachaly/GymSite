using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<ExerciseRecord> ExerciseRecord { get; set; }
        public DbSet<ExerciseFilter> ExerciseFilter { get; set; }
        public DbSet<ExerciseExerciseFilter> ExerciseExerciseFilter { get; set; }
        public DbSet<Workout> Workout { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercise { get; set; }
        public DbSet<ExerciseSet> ExerciseSet { get; set; }
        public DbSet<Article> Article { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().
                HasOne(u => u.UserInfo).
                WithOne(info => info.User).
                HasForeignKey<ApplicationUser>(user => user.UserInfoId);

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.Exercises)
                .WithOne(exercise => exercise.User)
                .HasForeignKey(exercise => exercise.UserId);

            builder.Entity<Exercise>()
                .HasMany(exercise => exercise.Records)
                .WithOne(record => record.Exercise)
                .HasForeignKey(record => record.ExerciseId);

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.ExerciseRecords)
                .WithOne(record => record.User)
                .HasForeignKey(record => record.UserId);

            builder.Entity<ExerciseExerciseFilter>()
                .HasKey(filter => new { filter.ExerciseId, filter.FilterId });
        }
    }
}
