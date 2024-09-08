using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("orders")]
    public class Orders : EntityBase
    {
        public Orders()
        {
            OrderItems = new HashSet<OrderItems>();
            ProductSkuUsersReturns = new HashSet<ProductSkuUsersReturns>();
            //SellerOrders = new HashSet<SellerOrders>();
        }
        //[ForeignKey("Users")]
        //[Column("user_id")]
        //public int? UserId { get; set; }
        [Column("order_number")]
        public string OrderNumber { get; set; }
        [Column("invoice_number")]
        public string InvoiceNumber { get; set; }
        [Column("note")]
        public string Note { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        //[Column("seller_status")]
        //public int? SellerStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("pickup_at")]
        public DateTime? PickupDate { get; set; }
        
        [Column("wallet_id")]
        public int? WalletId { get; set; }
        [Column("coupon_id")]
        public int? CouponId { get; set; }
        [Column("previous_status")]
        public int? PreviousStatus { get; set; }
        [Column("special_discount")]
        public Double? SpecialDiscount { get; set; }
        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ProductSkuUsersReturns> ProductSkuUsersReturns { get; set; }
        //public virtual ICollection<SellerOrders> SellerOrders { get; set; }
    }
}
