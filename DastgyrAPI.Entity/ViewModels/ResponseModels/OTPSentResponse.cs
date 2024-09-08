using System;
using System.Collections.Generic;
using System.Text;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{

    public class Response
    {
        public string ErrorNo { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class OTPSentResponse
    {
        public Response Response { get; set; }
    }
}
