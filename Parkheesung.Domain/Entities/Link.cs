using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Parkheesung.Domain.Entities
{
    public class Link
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long LinkID { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        [Column(TypeName = SqlType.NVarChar)]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(250)]
        public string URL { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(250)]
        public string LinkImage { get; set; }
    }
}
