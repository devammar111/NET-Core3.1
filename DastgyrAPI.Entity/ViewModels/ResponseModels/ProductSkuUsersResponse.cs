using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.ViewModels.ResponseModels
{
    public class ProductSkuUsersResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ProductSkuUserItemsResponse> SKU { get; set; }
    }

    public class UpdateListedQuantityResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
    
}
