using System;
using BlzorPWA.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BlzorPWA.Server.Models
{
    public partial class UserInformationContext : DbContext
    {
        public UserInformationContext()
        {
        }

        public UserInformationContext(DbContextOptions<UserInformationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\Blazor_PWA_LocalDB;Initial Catalog=UserInformation;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<NotificationSubscription>(entity =>
            {
                entity.ToTable("NotificationSubscription");

                entity.Property(e => e.NotificationSubscriptionId).ValueGeneratedNever();

                entity.Property(e => e.Url).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
