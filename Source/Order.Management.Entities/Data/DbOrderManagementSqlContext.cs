using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Orer.Management.Api.Models;

namespace Orer.Management.Api.Data;

public partial class DbOrderManagementSqlContext : DbContext
{
    public DbOrderManagementSqlContext()
    {
    }

    public DbOrderManagementSqlContext(DbContextOptions<DbOrderManagementSqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLog> OrderLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:OrderManagementConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account", "DB_ORDER_MANAGEMENT_SQL");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee", "DB_ORDER_MANAGEMENT_SQL");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "DB_ORDER_MANAGEMENT_SQL");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(250);

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_Order_Employee");
        });

        modelBuilder.Entity<OrderLog>(entity =>
        {
            entity.ToTable("OrderLog", "DB_ORDER_MANAGEMENT_SQL");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateBy).HasMaxLength(250);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLogs)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_OrderLog_Order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
