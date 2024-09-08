using System;
using System.Collections.Generic;
using System.Text;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class AddTokenRequest
    {
        public string Token { get; set; }
        public string SixDigitCode { get; set; }
    }
}
