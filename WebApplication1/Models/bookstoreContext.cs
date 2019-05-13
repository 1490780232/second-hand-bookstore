using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Models
{
    public partial class bookstoreContext : DbContext
    {
        public bookstoreContext()
        {
        }

        public bookstoreContext(DbContextOptions<bookstoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Bookcase> Bookcase { get; set; }
        public virtual DbSet<BookStatu> BookStatu { get; set; }
        public virtual DbSet<CheckBook> CheckBook { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.AdminId)
                    .HasColumnName("adminID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminName)
                    .HasColumnName("adminName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNum)
                    .HasColumnName("phoneNum")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.BookId)
                    .HasColumnName("bookID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BookIbsn)
                    .HasColumnName("bookIBSN")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BookName)
                    .HasColumnName("bookName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CurrPrice).HasColumnName("currPrice");

                entity.Property(e => e.OriPrice).HasColumnName("oriPrice");

                entity.Property(e => e.Press)
                    .HasColumnName("press")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bookcase>(entity =>
            {
                entity.ToTable("bookcase");

                entity.Property(e => e.BookcaseId)
                    .HasColumnName("bookcaseID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Rfid)
                    .HasColumnName("RFID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BookStatu>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("book_statu");

                entity.Property(e => e.BookId)
                    .HasColumnName("bookID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BookStatus).HasColumnName("bookStatus");

                entity.Property(e => e.BookcaseId)
                    .HasColumnName("BookcaseID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CheckStatus).HasColumnName("checkStatus");

                entity.Property(e => e.Rfid)
                    .HasColumnName("RFID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.STime)
                    .HasColumnName("sTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.SellerId)
                    .HasColumnName("sellerID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CheckBook>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("check_book");

                entity.Property(e => e.BookId)
                    .HasColumnName("bookID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminId)
                    .HasColumnName("adminID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CheckStatus).HasColumnName("checkStatus");

                entity.Property(e => e.CheckTime)
                    .HasColumnName("checkTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FaileReason)
                    .HasColumnName("faileReason")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BookId)
                    .HasColumnName("bookID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderPrice).HasColumnName("orderPrice");

                entity.Property(e => e.OrderStatus).HasColumnName("orderStatus");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("orderTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AlipayAccount)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ContactInfo)
                    .HasColumnName("contactInfo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .HasColumnName("mail")
                    .HasMaxLength(20);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
        }
    }
}
