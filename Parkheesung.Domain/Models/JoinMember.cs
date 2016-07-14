using System;
using System.ComponentModel.DataAnnotations;

namespace Parkheesung.Domain.Models
{
    public class JoinMember
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(254)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public JoinMember()
        {
            this.Email = String.Empty;
            this.Password = String.Empty;
            this.Name = String.Empty;
        }
    }


}
