using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ProductsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double? Price { get; set; }
        public double Discount { get; set; }
        public int Stock { get; set; }
        [Column(TypeName = "jsonb")]
        public string Image { get; set; }
        public int Rate { get; set; }
        public bool? IsActive{ get; set; }
        public DateTime CreatedDate{ get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }

    public class ProductsListResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ProductSkuReponse> Products { get; set; }
    }
}
