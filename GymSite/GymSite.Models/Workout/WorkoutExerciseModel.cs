using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Models.Workout
{
    public class WorkoutExerciseModel
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseDescription { get; set; }
        public string ExerciseName { get; set; }
        public IEnumerable<ExerciseSetModel> Sets { get; set; }
    }
}
