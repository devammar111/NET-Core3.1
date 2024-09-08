using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("product_sku_users_returns")]
    public class ProductSkuUsersReturns
    {
        [ForeignKey("Orders")]
        [Column("order_id")]
        public int OrderId { get; set; }
        [ForeignKey("ProductSku")]
        [Column("sku_id")]
        public int? SkuId { get; set; }
        [Column("qty")]
        public int? Quantity { get; set; }
        [Column("return_qty")]
        public int? ReturnQuantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("received_date")]
        public DateTime? ReceivedDate { get; set; }
        [Column("delta_return")]
        public int? DeltaReturn { get; set; }
        [Column("stop_id")]
        public int? StopId { get; set; }
        public virtual Orders Orders { get; set; }
        public virtual ProductSku ProductSku { get; set; }
    }
}
