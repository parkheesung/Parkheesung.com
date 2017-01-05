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
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public long GroupID { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(500)]
        public string UserID { get; set; }

        [Required]
        [Column(TypeName = SqlType.VarChar)]
        [StringLength(500)]
        public string UserPWD { get; set; }

        [Column(TypeName = SqlType.VarChar)]
        [StringLength(500)]
        public string AccessURL { get; set; }

        [Column(TypeName = SqlType.NVarChar)]
        public string Memo { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime RegDate { get; set; }

        [Column(TypeName = SqlType.DateTime2)]
        public DateTime LastUpdate { get; set; }

        public void Decoding(string SecretKey)
        {
            this.UserID = OctopusLibrary.Crypto.AES256.Decrypt(this.UserID, SecretKey, false);
            this.UserPWD = OctopusLibrary.Crypto.AES256.Decrypt(this.UserPWD, SecretKey, false);
        }

        public void Encoding(string SecretKey)
        {
            this.UserID = OctopusLibrary.Crypto.AES256.Encrypt(this.UserID, SecretKey, false);
            this.UserPWD = OctopusLibrary.Crypto.AES256.Encrypt(this.UserPWD, SecretKey, false);
        }

    }
}
