using System;
using System.Collections.Generic;
using System.Text;

namespace DastgyrAPI.Models.ViewModels.Responses
{
    public class ErrorResponse
    {
        public string Code { get; set; }
        public List<Error> Errors { get; set; }        
    }

    public class Error
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
