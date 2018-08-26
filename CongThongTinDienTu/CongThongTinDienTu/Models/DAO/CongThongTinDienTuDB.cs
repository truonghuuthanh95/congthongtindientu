namespace CongThongTinDienTu.Models.DAO
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CongThongTinDienTuDB : DbContext
    {
        public CongThongTinDienTuDB()
            : base("name=CongThongTinDienTuDB3")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<CapTruong> CapTruongs { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DVQL> DVQLs { get; set; }
        public virtual DbSet<HopDong> HopDongs { get; set; }
        public virtual DbSet<LoaiHinh> LoaiHinhs { get; set; }
        public virtual DbSet<LogEmail> LogEmails { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(e => e.HopDongs)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.NguoiTiepNhan);

            modelBuilder.Entity<District>()
                .HasMany(e => e.Wards)
                .WithRequired(e => e.District)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HopDong>()
                .Property(e => e.SoTien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LoaiHinh>()
                .HasMany(e => e.Schools)
                .WithOptional(e => e.LoaiHinh1)
                .HasForeignKey(e => e.LoaiHinh);
        }
    }
}
