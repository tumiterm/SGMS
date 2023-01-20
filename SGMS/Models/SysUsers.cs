using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class SysUsers : Base
    {
        [Key]
        public Guid Id { get; set; }
        public eTask Role { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        [MinLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
