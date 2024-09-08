using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("products")]
    public class Products : EntityBase
    {
        public Products()
        {
            ProductSku = new HashSet<ProductSku>();
            ProductSkuUsers = new HashSet<ProductSkuUsers>();
        }
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
        [Column("created_at")]
        public DateTime CreatedDate{ get; set; }
        [Column("updated_at")]
        public DateTime UpdatedDate { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedDate { get; set; }

        public virtual ICollection<ProductSku> ProductSku { get; set; }
        public virtual ICollection<ProductSkuUsers> ProductSkuUsers { get; set; }
        
    }
}
