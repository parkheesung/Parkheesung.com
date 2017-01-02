using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    public class Github
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long GithubID { get; set; }

        [Required]
        public long MemberID { get; set; }

        [Required]
        [Column(TypeName = SqlType.NVarChar)]
        [StringLength(150)]
        public string Title { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(250)]
        public string AccessURL { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        public string Memo { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime LastUpdate { get; set; }
    }
}
