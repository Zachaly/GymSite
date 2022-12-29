using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.ExerciseFilter.Validator;
using GymSite.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseFilterService))]
    public class ExerciseFilterService : IExerciseFilterService
    {
        private readonly IExerciseFilterRepository _exerciseFilterRepository;
        private readonly IExerciseFilterFactory _exerciseFilterFactory;
        private readonly IResponseFactory _responseFactory;

        public ExerciseFilterService(IExerciseFilterRepository exerciseFilterRepository, IResponseFactory responseFactory,
            IExerciseFilterFactory exerciseFilterFactory)
        {
            _exerciseFilterRepository = exerciseFilterRepository;
            _exerciseFilterFactory = exerciseFilterFactory;
            _responseFactory = responseFactory;
        }

        public async Task<DataResponseModel<ExerciseFilterModel>> AddFilter(AddFilterRequest request)
        {
            var validation = new AddFilterValidator().Validate(request);

            if(!validation.IsValid)
            {
                return _responseFactory.CreateFail<ExerciseFilterModel>("", validation.GetValidationErrors());
            }

            try
            {
                var filter = _exerciseFilterFactory.Create(request);

                await _exerciseFilterRepository.AddFilter(filter);

                return _responseFactory.CreateSuccess(_exerciseFilterFactory.CreateModel(filter));
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFail<ExerciseFilterModel>(ex.Message, null);
            }
        }

        public DataResponseModel<IEnumerable<ExerciseFilterModel>> GetFilters()
        {
            var filters = _exerciseFilterRepository.GetFilters(filter => _exerciseFilterFactory.CreateModel(filter));

            return _responseFactory.CreateSuccess(filters);
        }

        public async Task<ResponseModel> RemoveFilter(int id)
        {
            try
            {
                await _exerciseFilterRepository.RemoveFilter(id);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }
    }
}
