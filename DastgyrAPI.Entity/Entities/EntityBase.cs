using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DastgyrAPI.Models.DomainModels
{
    public class EntityBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public virtual int Id { get; set; }
        
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime CreatedDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime ModifiedDate { get; set; }
        //public int CreatedBy { get; set; }
        //public int ModifiedBy { get; set; }
        //public bool? IsActive { get; set; } = true;
    }
}
