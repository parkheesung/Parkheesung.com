using OctopusLibrary;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    public class MemberLoginLog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long LogID { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(50)]
        public string UserIP { get; set; }
    }
}
