
using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.Models

{
    public class Player : Base
    { 
        [Key]
        public Guid PlayerId { get; set; }

        [Display(Name ="Player Team")]
        [ForeignKey("Team")]
        public Guid TeamId { get; set; }

        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }

        [Display(Name ="Jersey Number")]
        public int JerseyNumber { get; set; }

        [Display(Name = "Player Surname")]
        public string PlayerLastName { get; set; }

        [MaxLength(13)]
        [MinLength(13)]
        [StringLength(13)]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }

        [Display(Name = "RSA ID Copy")]
        public string? RSAIDCopy { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date-of-Birth")]
        public string DOB { get; set; }
        public string? Photo { get; set; }
        public ePosition Position { get; set; }

        [Display(Name ="Card Number")]
        public string? CardNumber { get; set; }

        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]

        public string Phone { get; set; }
        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        [Display(Name = "Alternative Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? AlternativePhone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name ="Street Name")]
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public eProvince Province { get; set; }
        public eGender Gender { get; set; }

        [MaxLength(4)]
        [MinLength(4)]
        [StringLength(4)]
        [DataType(DataType.PostalCode)]
        [Display(Name ="Postal Code")]
        public string? PostalCode { get; set; }

        [Display(Name ="RSA ID Copy File")]
        [NotMapped]
        public IFormFile? RSAIDCopyFile { get; set; }

        [Display(Name ="Photo File")]
         [NotMapped]
         public IFormFile PhotoFile { get; set; }


        }
}
