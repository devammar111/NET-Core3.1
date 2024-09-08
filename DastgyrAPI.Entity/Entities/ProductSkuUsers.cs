using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("product_sku_users", Schema = "public")]
    public partial class ProductSkuUsers : EntityBase
    {
        public ProductSkuUsers()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }
        [ForeignKey("Products")]
        [Column("product_id")]
        public override int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }
        [ForeignKey("ProductSku")]
        [Column("sku_id")]
        public int? SkuId { get; set; }
        [Column("price")]
        public Double? Price { get; set; }
        [Column("quantity")]
        public int? Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        public virtual Products Products { get; set; }
        public virtual ProductSku ProductSku { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("stock_availability_date")]
        public DateTime? StockAvailabilityDate { get; set; }

        [Column("minimum_quantity")]
        public int? MinimumQuantity { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("send_low_inventory_notification")]
        public bool? SendLowInventoryNotification { get; set; }

        [Column("reminder_quantity")]
        public int? ReminderQuantity { get; set; }
    }
}
