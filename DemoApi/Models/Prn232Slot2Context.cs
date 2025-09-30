using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Models;

public partial class Prn232Slot2Context : DbContext
{
    public Prn232Slot2Context()
    {
    }

    public Prn232Slot2Context(DbContextOptions<Prn232Slot2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=DESKTOP-P4RNSL0;database=PRN232_slot2;uid=sa;pwd=123;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DepartId)
                .HasMaxLength(10)
                .HasColumnName("departId");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Gpa).HasColumnName("gpa");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.HasOne(d => d.Depart).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Department");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
