using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    public class TokenAuth
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long TokenAuthID { get; set; }

        public long MemberID { get; set; }

        [Required]
        [Column(TypeName = SqlType.Char)]
        [StringLength(36)]
        [Index("IX_TokenAuth_Token", 1, IsUnique = true)]
        public string UserToken { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(300)]
        [DefaultValue("")]
        public string FacebookToken { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime ExpiredDate { get; set; }

        [DefaultValue(true)]
        public bool IsEnabled { get; set; }
    }
}
