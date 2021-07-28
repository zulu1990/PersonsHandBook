using Microsoft.EntityFrameworkCore;
using PersonsHandBook.Domain.Models.Entity;

namespace PersonsHandBook.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(x => x.Photo)
                .WithOne(b=>b.Person)
                .HasForeignKey<Photo>(a=>a.PersonId);


            modelBuilder.Entity<Contact>()
                 .HasOne(c => c.Person)
                 .WithMany(p => p.Contacts);

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Person)
                .WithOne(x => x.Photo);


        }
    }
}
