using AssestManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AssestManagement.Data
{
    public partial class AssetDbContext: DbContext
    {
        public AssetDbContext()
        {

        }
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options)
        {
        }
        
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetImage> AssetImages { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<UserInfo>? UserInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Asset>()
                .HasOne(a => a.AssetImages)
                .WithOne(ai => ai.Asset)
                .HasForeignKey<AssetImage>(ai => ai.AssetId);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Documents)
                .WithOne(d => d.Asset)
                .HasForeignKey<Document>(d => d.AssetId);

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserInfo");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.DisplayName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

 }

