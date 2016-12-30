using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long AccountID { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        [Column(TypeName = SqlType.NVarChar)]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public long GroupID { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(250)]
        public string UserID { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(300)]
        public string UserPWD { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(250)]
        public string AccessURL { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        public string Memo { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime LastUpdate { get; set; }

    }
}
