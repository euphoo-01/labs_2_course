using Microsoft.EntityFrameworkCore;
using DAL_Celebrity;

namespace DAL_Celebrity_MSSQL;

public class Context : DbContext {
    public string? ConnectionString { get; private set; } = null;
    public Context(string connstring) : base() { this.ConnectionString = connstring; }
    public Context() : base() { }

    public DbSet<Celebrity> Celebrities { get; set; }
    public DbSet<Lifeevent> Lifeevents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (this.ConnectionString == null) 
            this.ConnectionString = @"Data source=..."; // Твой сервер из Init.cs
        optionsBuilder.UseSqlServer(this.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Celebrity>().ToTable("Celebrities").HasKey(p => p.Id);
        modelBuilder.Entity<Celebrity>().Property(p => p.Id).IsRequired();
        modelBuilder.Entity<Celebrity>().Property(p => p.FullName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Celebrity>().Property(p => p.Nationality).IsRequired().HasMaxLength(20);

        modelBuilder.Entity<Lifeevent>().ToTable("Lifeevents").HasKey(p => p.Id);
        modelBuilder.Entity<Lifeevent>().HasOne<Celebrity>().WithMany().HasForeignKey(p => p.CelebrityId);
        modelBuilder.Entity<Lifeevent>().Property(p => p.Description).HasMaxLength(256);
        base.OnModelCreating(modelBuilder);
    }
}