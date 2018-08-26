namespace CongThongTinDienTu.Models.DAO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HopDong")]
    public partial class HopDong
    {
        public int Id { get; set; }

        public int? SchoolId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayKiHD { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayHieuLucHD { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        public bool? IsThanhToanBangTienMat { get; set; }

        public int? NguoiTiepNhan { get; set; }

        public bool? IsTaoMoi { get; set; }

        public decimal? SoTien { get; set; }

        public int? MaHopDong { get; set; }

        public virtual Account Account { get; set; }

        public virtual School School { get; set; }
    }
}
