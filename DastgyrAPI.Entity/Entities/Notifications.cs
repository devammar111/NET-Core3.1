using DastgyrAPI.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Entity
{
    [Table("notifications", Schema = "public")]
    public partial class Notifications:EntityBase
    {
        [Column("title")]
        public string Title { get; set; }
        [Column("text")]
        //[Column(TypeName = "ntext")]
        public string Text { get; set; }
        [Column("is_send")]
        public bool? Is_Send { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("created_at")]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("updated_at")]
        public DateTime ModifiedDate { get; set; }
        [Column("destination")]
        public string Destination { get; set; }
    }
}
