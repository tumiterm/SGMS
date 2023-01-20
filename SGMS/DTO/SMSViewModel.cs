using SGMS.Models;

namespace SGMS.DTO
{
    public class SMSViewModel
    {
        public string message { get; set; }
        public List<Recipient> recipients { get; set; }
        public string? scheduledTime { get; set; }
        public int? maxSegments { get; set; }
    }


  

  


}

