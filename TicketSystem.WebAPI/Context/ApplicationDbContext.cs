using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Abstractions;
using TicketSystem.WebAPI.Models;

namespace TicketSystem.WebAPI.Context;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>(); 

    public DbSet<Admin> Admins => Set<Admin>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketReply> TicketReplies => Set<TicketReply>();

    public DbSet<TicketRating> TicketRatings => Set<TicketRating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(x => x.Role)
            .HasConversion(
                x => x.Value,
                x => UserRole.FromValue(x));

        modelBuilder.Entity<Ticket>()
            .Property(x => x.Status)
            .HasConversion(
                x => x.Value,
                x => TicketStatus.FromValue(x)); // TicketStatus enum'unu kullanarak dönüşüm yapıyoruz. DB tarafında string olarak saklanacak.    

        modelBuilder.Entity<Ticket>()
            .HasOne(x => x.CreatedByUser) // Ticket ile User arasındaki birebir ilişkiyi tanımlıyoruz.
            .WithMany(x => x.CreatedTickets) // User'ın oluşturduğu ticketleri temsil eden koleksiyon. Coklu ilişkiyi
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde, ilişkili biletler silinmez.

        modelBuilder.Entity<TicketReply>()
            .HasOne(x => x.Ticket)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade); // Ticket silindiğinde, ilişkili yanıtlar da silinir.

        modelBuilder.Entity<TicketReply>()
            .HasOne(x => x.User)
            .WithMany(x => x.TicketReplies)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketRating>()
            .HasOne(x => x.Ticket)
            .WithOne(x => x.Rating)
            .HasForeignKey<TicketRating>(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketRating>()
            .HasOne(x => x.User)
            .WithMany(x => x.TicketRatings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}