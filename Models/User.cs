using System.ComponentModel.DataAnnotations;

namespace HoneyBank.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
