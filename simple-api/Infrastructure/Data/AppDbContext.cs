namespace SimpleApi.Infrastructure.Data;

using SimpleApi.Entities;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserNote> UserNotes => Set<UserNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Name).HasMaxLength(150).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(200).IsRequired();
            entity.Property(user => user.CreatedAtUtc).IsRequired();

            entity.HasMany(user => user.Notes)
                .WithOne(note => note.User)
                .HasForeignKey(note => note.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserNote>(entity =>
        {
            entity.ToTable("UserNotes");
            entity.HasKey(note => note.Id);
            entity.Property(note => note.Content).HasMaxLength(1000).IsRequired();
            entity.Property(note => note.CreatedAtUtc).IsRequired();
        });
    }
}
