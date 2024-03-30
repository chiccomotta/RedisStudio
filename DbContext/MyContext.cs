using Microsoft.EntityFrameworkCore;

namespace RedisStudio.DbContext;

public partial class MyContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Travel> Travel { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Travel>(entity =>
        {
            entity.HasKey(e => e.TravelId).HasName("PK__Travel__E9315235C52C1543");

            entity.HasIndex(e => e.DossierCode, "IX_DossierCode").IsUnique();

            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.DossierCode)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.InsertBy).HasMaxLength(50);
            entity.Property(e => e.InsertDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Nation).HasMaxLength(60);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });
        modelBuilder.HasSequence<int>("OneSequence");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
