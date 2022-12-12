﻿using GymSite.Application.Abstractions;
using GymSite.Application.Response.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Utils;
using GymSite.Models.Record.Request;
using GymSite.Models.Response;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseRecordService))]
    public class ExerciseRecordService : IExerciseRecordService
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IExerciseRecordFactory _exerciseRecordFactory;
        private readonly IExerciseRecordRepository _exerciseRecordRepository;

        public ExerciseRecordService(IResponseFactory responseFactory, IExerciseRecordRepository exerciseRecordRepository,
            IExerciseRecordFactory exerciseRecordFactory)
        {
            _responseFactory = responseFactory;
            _exerciseRecordFactory = exerciseRecordFactory;
            _exerciseRecordRepository = exerciseRecordRepository;
        }

        public async Task<ResponseModel> AddRecord(AddExerciseRecordRequest request)
        {
            var record = _exerciseRecordFactory.Create(request);

            await _exerciseRecordRepository.AddRecordAsync(record);

            return _responseFactory.CreateSuccess(ResponseCode.NoContent, "");
        }

        public async Task<ResponseModel> RemoveRecord(int id)
        {
            try
            {
                await _exerciseRecordRepository.RemoveRecordByIdAsync(id);

                return _responseFactory.CreateSuccess(ResponseCode.NoContent, "");
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFail(ResponseCode.BadRequest, ex.Message, null);
            }
        }
    }
}
