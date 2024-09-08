using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DastgyrAPI.Entity
{
    
    [Table("seller_change_requests", Schema = "public")]
    public class SellerChangeRequests: EntityBase
    {
        public SellerChangeRequests()
        {
            ApprovalStatus = 0;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        [Column("approval_status")]
        public int? ApprovalStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [Column("seller_id")]
        public int SellerId { get; set; }
        [Column("sku_id")]
        public int SkuId { get; set; }
        [Column("allowed_qty")]
        public int AllowedQuantity { get; set; }
        [Column("requested_qty")]
        public int RequestedQuantity { get; set; }
    }
}
