using GymSite.Domain.Entity;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseSetFactory
    {
        ExerciseSet Create(AddExerciseSetRequest request);
        ExerciseSetModel CreateModel(ExerciseSet exerciseSet);
    }
}
