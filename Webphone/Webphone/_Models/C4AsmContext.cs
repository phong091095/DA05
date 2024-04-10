using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Webphone._Models;

public partial class C4AsmContext : DbContext
{
    public C4AsmContext()
    {
    }

    public C4AsmContext(DbContextOptions<C4AsmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<LoginInfo> LoginInfos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PTPM\\PHONG;Initial Catalog=C#4_ASM;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustId).HasName("PK__Customer__049E3A8901A05EE1");

            entity.ToTable("Customer");

            entity.Property(e => e.CustId).HasColumnName("CustID");
            entity.Property(e => e.CustAd).HasMaxLength(255);
            entity.Property(e => e.CustMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustName).HasMaxLength(255);
            entity.Property(e => e.CustPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.LoginId).HasColumnName("Login_ID");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Customers)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Customer__Invoic__01142BA1");

            entity.HasOne(d => d.Login).WithMany(p => p.Customers)
                .HasForeignKey(d => d.LoginId)
                .HasConstraintName("FK_Login");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__D796AAD52CB36CB9");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.CustAdd).HasMaxLength(255);
            entity.Property(e => e.CustName).HasMaxLength(255);
            entity.Property(e => e.CustPhone).HasMaxLength(20);
            entity.Property(e => e.InvoiceStatus).HasMaxLength(255);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvoiceDetailId).HasName("PK__InvoiceD__1F1578F13C5D5477");

            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.InvoiceDetailId).HasColumnName("InvoiceDetailID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_InID");

            entity.HasOne(d => d.Product).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_InProID");
        });

        modelBuilder.Entity<LoginInfo>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__Login_In__D78868670621E28E");

            entity.ToTable("Login_Info");

            entity.HasIndex(e => e.Email, "UQ__Login_In__A9D105342E2489C5").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Login_In__C9F28456F6C75D6F").IsUnique();

            entity.Property(e => e.LoginId).HasColumnName("Login_ID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LoginRole).HasColumnName("Login_Role");
            entity.Property(e => e.LoginStatus).HasColumnName("Login_Status");
            entity.Property(e => e.Passw).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Products__04BA0DF59553C706");

            entity.Property(e => e.ProId).HasColumnName("Pro_ID");
            entity.Property(e => e.Camera).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(255);
            entity.Property(e => e.Cpu)
                .HasMaxLength(255)
                .HasColumnName("CPU");
            entity.Property(e => e.Hdh)
                .HasMaxLength(255)
                .HasColumnName("HDH");
            entity.Property(e => e.ProBrand)
                .HasMaxLength(255)
                .HasColumnName("Pro_Brand");
            entity.Property(e => e.ProImg).HasColumnName("Pro_Img");
            entity.Property(e => e.ProName)
                .HasMaxLength(255)
                .HasColumnName("Pro_Name");
            entity.Property(e => e.ProPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Pro_Price");
            entity.Property(e => e.ProType)
                .HasMaxLength(255)
                .HasColumnName("Pro_Type");
            entity.Property(e => e.Ram).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
