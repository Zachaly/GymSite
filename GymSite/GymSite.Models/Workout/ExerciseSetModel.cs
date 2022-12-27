using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Models.Workout
{
    public class ExerciseSetModel
    {
        public int Id { get; set; }
        public string Weight { get; set; }
        public int Reps { get; set; }
    }
}
