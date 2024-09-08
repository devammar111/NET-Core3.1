using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("external_suppliers", Schema = "public")]
    public partial class ExternalSuppliers : EntityBase
    {
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("city_id")]
        public int? CityId { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("address")]
        //[Column(TypeName = "ntext")]
        public string Address { get; set; }
        [Column("latlng", TypeName = "jsonb")]
        public string Latlng { get; set; }
    }
}
