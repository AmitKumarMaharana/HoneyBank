using System.ComponentModel.DataAnnotations;

namespace HoneyBank.Models
{
    public class BankDetailsModel
    {
        [Key]
        public int BankDetailsID { get; set; }

        [Required]
        public int UserID { get; set; }

        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }

        public string AccountCategory { get; set; }

        public string OtherInfo { get; set; }
    }
}
