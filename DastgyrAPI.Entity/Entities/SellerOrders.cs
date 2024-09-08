using DastgyrAPI.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    //[Table("seller_orders", Schema = "public")]
    //public class SellerOrders : EntityBase
    //{
    //    public SellerOrders()
    //    {
    //        CreatedDate = DateTime.Now;
    //        ModifiedDate = DateTime.Now;
    //    }
    //    [Column("seller_id")]
    //    public int SellerId { get; set; }
    //    [ForeignKey("Orders")]
    //    [Column("order_id")]
    //    public int OrderId { get; set; }
    //    [Column("status")]
    //    public int? Status { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
    //    [Column("created_at")]
    //    public DateTime CreatedDate { get; set; }
    //    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
    //    [Column("updated_at")]
    //    public DateTime ModifiedDate { get; set; }
    //    public virtual Orders Orders { get; set; }
    //}
}
