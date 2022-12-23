using GymSite.Models.Record;

namespace GymSite.Models.Exercise
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ExerciseRecordModel> Records { get; set; }
        public bool Removable { get; set; }
        public IEnumerable<string> Filters { get; set; }
    }
}
