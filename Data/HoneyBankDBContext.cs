using Microsoft.EntityFrameworkCore;
using HoneyBank.Models;

namespace HoneyBank.Data
{
    public class HoneyBankDBContext : DbContext
    {
        public HoneyBankDBContext(DbContextOptions<HoneyBankDBContext> options) : base(options) { }

        // Define DbSets (tables) here
        public DbSet<User> User { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; } 
        public DbSet<BankDetailsModel> BankDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
    }
}
