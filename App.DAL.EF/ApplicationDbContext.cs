using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class ApplicationDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<Person> Persons { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<EventCompanies> EventsCompanies { get; set; } = default!;
    public DbSet<EventPersons> EventsPersons { get; set; } = default!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}