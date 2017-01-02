using OctopusLibrary;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parkheesung.Domain.Entities
{
    [Table("AccountGroupView")]
    public class AccountGroupView
    {
        [Key]
        public long GroupID { get; set; }

        [Required]
        public long MemberID { get; set; }

        public int GroupCount { get; set; }

        [Column(TypeName = SqlType.NVarChar)]
        [StringLength(150)]
        public string GroupName { get; set; }
    }
}
