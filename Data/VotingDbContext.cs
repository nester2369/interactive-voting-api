// Файл описывает контекст Entity Framework Core, таблицы базы данных и связи между сущностями.

using Inter.MixAN.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inter.MixAN.Data;

public class VotingDbContext : DbContext
{
    public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options)
    {
    }

    public DbSet<Voter> Voters => Set<Voter>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Election> Elections => Set<Election>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<CandidateApplication> CandidateApplications => Set<CandidateApplication>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Voter>()
            .HasIndex(x => x.PassportNumber)
            .IsUnique();

        modelBuilder.Entity<Voter>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Vote>()
            .HasIndex(x => new { x.ElectionId, x.VoterId })
            .IsUnique();

        modelBuilder.Entity<Vote>()
            .HasOne(x => x.Election)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.ElectionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vote>()
            .HasOne(x => x.Voter)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.VoterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vote>()
            .HasOne(x => x.Candidate)
            .WithMany(x => x.Votes)
            .HasForeignKey(x => x.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CandidateApplication>()
            .HasOne(x => x.Candidate)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
