namespace HoneyBank.Models
{
    public class SignUpViewModel
    {
        // User table fields
        public string Username { get; set; }
        public string Password { get; set; }

        // UserDetails table fields
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
