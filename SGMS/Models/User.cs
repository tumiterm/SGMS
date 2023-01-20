using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public partial class User : Base
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Full Name")]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(25)]
        [MinLength(3)]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]

        [Display(Name = "Email Address")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid? ActivationCode { get; set; }
        public string? ResetPasswordCode { get; set; }

        [Display(Name = "Last Login Date")]
        public string? LastLoginDate { get; set; }

        [Display(Name = "Register As")]
        public eSysRole? Role { get; set; }
    }
}
