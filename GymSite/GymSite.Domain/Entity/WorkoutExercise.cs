using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Domain.Entity
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public ICollection<ExerciseSet> ExerciseSets { get; set; }
    }
}
