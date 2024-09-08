using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class MobileVerificationsRequest
    {
        public string Mobile { get; set; }
        public string Code { get; set; }
        public int? Count { get; set; }
        public bool? IsAvail { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ModifiedDate { get; set; }

    }
}
