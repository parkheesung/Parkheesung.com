using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    public class Member
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long MemberID { get; set; }

        [Required]
        [Column(TypeName = SqlType.NVarChar)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(254)]
        [Index("IX_Member_ID", 1, IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(300)]
        public string Password { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime LastUpdate { get; set; }

        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        [Required]
        [Column(TypeName = SqlType.Char)]
        [StringLength(36)]
        [Index("IX_Member_Token", 1, IsUnique = true)]
        public string UserToken { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(254)]
        public string UserPhotoURL { get; set; }

        [DefaultValue(false)]
        public bool IsFacebook { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(100)]
        public string FacebookID { get; set; }
    }
}
