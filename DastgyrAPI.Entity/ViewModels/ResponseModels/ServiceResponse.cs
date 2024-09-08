
namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ServiceResponse
    {
        public int StatusCode { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }

    }
}
