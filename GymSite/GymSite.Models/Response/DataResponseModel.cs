namespace GymSite.Models.Response
{
    public class DataResponseModel<TData> : ResponseModel
    {
        public TData? Data { get; set; }
    }
}
