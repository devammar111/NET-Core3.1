using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("product_sku")]
    public class ProductSku : EntityBase
    {
        public ProductSku()
        {
            ProductSkuUsers = new HashSet<ProductSkuUsers>();
            OrderItems = new HashSet<OrderItems>();
            WarehouseItems = new HashSet<WarehouseItems>();
            ProductSkuReturns = new HashSet<ProductSkuUsersReturns>();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        [ForeignKey("Products")]
        [Column("product_id")]
        public int? ProductId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("price")]
        public double? Price { get; set; }
        [Column("discount")]
        public double? Discount { get; set; }
        [Column("stock")]
        public int? Stock { get; set; }
        [Column("image", TypeName = "jsonb")]
        public string Image { get; set; }
        [Column("rate")]
        public int? Rate { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("is_stock")]
        public bool? IsStock { get; set; }
        [Column("weight")]
        public double? Weight { get; set; }
        [Column("unit")]
        public string Unit { get; set; }
        [Column("all_types")]
        public double? AllTypes { get; set; }
        [Column("is_deal")]
        public bool? IsDeal { get; set; }
        [Column("max_limit")]
        public int? MaxLimit { get; set; }
        [Column("is_new_label")]
        public bool? IsNewLabel { get; set; }
        [Column("aoos_limit")]
        public int? AoosLimit { get; set; }
        [Column("current_aoos_inv")]
        public int? CurrentAoosInv { get; set; }
        [Column("zero_inventory")]
        public bool? ZeroInventory { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedDate { get; set; }
        public virtual Products Products { get; set; }
        public virtual ICollection<ProductSkuUsers> ProductSkuUsers { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ProductSkuUsersReturns> ProductSkuReturns { get; set; }
        public virtual ICollection<WarehouseItems> WarehouseItems { get; set; }
    }
}

