using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.RequestModels
{
    public class ProductsRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double? Price { get; set; }
        public double Discount { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }
        public int Rate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
