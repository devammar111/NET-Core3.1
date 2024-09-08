using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class PaymentResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<PaymentItemsResponse> PaymentsData { get; set; }
        public double? TotalMoneyEarned
        {
            get;
            set;
        }
    }
}
