using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MSLR.web.Models;

public partial class MslrDbContext : DbContext
{
    public MslrDbContext()
    {
    }

    public MslrDbContext(DbContextOptions<MslrDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Referendum> Referendums { get; set; }

    public virtual DbSet<ReferendumOption> ReferendumOptions { get; set; }

    public virtual DbSet<ValidScc> ValidSccs { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<Voter> Voters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SHUBHAM\\SQLEXPRESS;Database=MSLR_DB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Referendum>(entity =>
        {
            entity.HasKey(e => e.ReferendumId).HasName("PK__Referend__AE2B192D322AEC45");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ReferendumOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Referend__92C7A1FF3F202323");

            entity.Property(e => e.OptionText)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Referendum).WithMany(p => p.ReferendumOptions)
                .HasForeignKey(d => d.ReferendumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Options_Referendum");
        });

        modelBuilder.Entity<ValidScc>(entity =>
        {
            entity.HasKey(e => e.Scc).HasName("PK__ValidSCC__CA19083403B43D39");

            entity.ToTable("ValidSCC");

            entity.Property(e => e.Scc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SCC");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.VoteId).HasName("PK__Votes__52F015C2C7CFA3D4");

            entity.HasIndex(e => new { e.VoterId, e.ReferendumId }, "UQ_Voter_Referendum").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Option).WithMany(p => p.Votes)
                .HasForeignKey(d => d.OptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Option");

            entity.HasOne(d => d.Referendum).WithMany(p => p.Votes)
                .HasForeignKey(d => d.ReferendumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Referendum");

            entity.HasOne(d => d.Voter).WithMany(p => p.Votes)
                .HasForeignKey(d => d.VoterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Votes_Voter");
        });

        modelBuilder.Entity<Voter>(entity =>
        {
            entity.HasKey(e => e.VoterId).HasName("PK__Voters__12D25BF8DDC4B5EE");

            entity.HasIndex(e => e.Email, "UQ__Voters__A9D1053440E2C249").IsUnique();

            entity.HasIndex(e => e.Scc, "UQ__Voters__CA190835B47507D8").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Scc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SCC");

            entity.HasOne(d => d.SccNavigation).WithOne(p => p.Voter)
                .HasForeignKey<Voter>(d => d.Scc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voters_SCC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
