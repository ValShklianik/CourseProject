using System.Data.Entity;
using DTO.Models;

namespace DAL
{
    internal class Context : DbContext
    {
        public Context() : base("Accounts")
        {
        }

        public DbSet<Text> Texts { get; set; }
        public DbSet<EncodedText> EncodedTexts { get; set; }
        public DbSet<KasiskiResult> KasiskiResults { get; set; }
        public DbSet<KasiskiResultItem> KasiskiResultItems { get; set; }
    }
}
