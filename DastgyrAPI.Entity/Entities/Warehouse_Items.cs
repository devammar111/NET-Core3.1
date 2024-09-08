using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("warehouse_items", Schema = "public")]
    public partial class WarehouseItems
    {
        [Column("warehouse_id")]
        public int? WarehouseId { get; set; }
        [ForeignKey("ProductSku")]
        [Column("sku_id")]
        public int? SkuId { get; set; }
        [Column("stock")]
        public int? Stock { get; set; }
        [Column("flag")]
        public int? Flag { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [Column("price")]
        public double? Price { get; set; }
        [Column("rate")]
        public double? Rate { get; set; }
        [Column("qty")]
        public int? Quantity { get; set; }
        [Column("supplier_id")]
        public int? SupplierId { get; set; }
        public virtual ProductSku ProductSku { get; set; }
    }
}
