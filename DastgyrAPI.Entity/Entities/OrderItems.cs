using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("order_items")]
    public class OrderItems
    {
        [ForeignKey("Orders")]
        [Column("order_id")]
        public int OrderId { get; set; }

        [ForeignKey("ProductSku")]
        [Column("sku_id")]
        public int? SkuId { get; set; }

        [Column("price")]
        public double? Price { get; set; }

        [Column("discount")]
        public double? Discount { get; set; }
        [Column("paid_seller_amount")]
        public double? PaidSellerAmount { get; set; }
        [Column("qty")]
        public int? Quantity { get; set; }

        [Column("status")]
        public int? Status { get; set; }

        [Column("tag")]
        public int? Tag { get; set; }

        [Column("return_qty")]
        public int? ReturnQuantity { get; set; }

        [Column("reason")]
        public string Reason { get; set; }

        [Column("deal_id")]
        public int? DealId { get; set; }

        [Column("delta_return")]
        public int? DeltaReturn { get; set; }

        [Column("seller_status")]
        public int? SellerStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("pickup_at")]
        public DateTime? PickupDate { get; set; }
        public virtual Orders Orders { get; set; }
        public virtual ProductSku ProductSku { get; set; }
    }
}



