using System.ComponentModel.DataAnnotations;

namespace HoneyBank.Models
{
    public class Payments
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public string SenderID { get; set; }
        [Required]
        public string ReceiverID { get; set; }

        public string PaymentDate { get; set; }

        public string PaymentDetails { get; set; }
    }
}
