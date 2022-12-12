using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Models.Record.Request
{
    public class AddExerciseRecordRequest
    {
        public int Reps { get; set; }
        public int ExerciseId { get; set; }
        public decimal Weight { get; set; }
        public string UserId { get; set; }
    }
}
