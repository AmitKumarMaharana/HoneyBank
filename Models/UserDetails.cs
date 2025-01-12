using System.ComponentModel.DataAnnotations;

namespace HoneyBank.Models
{
    public class UserDetails
    {
        [Key]
        public int UserDetailsID { get; set; }

        [Required]
        public int UserID { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
    }
}
