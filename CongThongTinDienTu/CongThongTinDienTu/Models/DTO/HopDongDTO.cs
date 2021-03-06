﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class HopDongDTO
    {
        public int SchoolId { get; set; }

        public DateTime NgayKiHD { get; set; }

        public DateTime NgayHieuLucHD { get; set; }

        public string GhiChu { get; set; } = "Không có";       

        public decimal? SoTien { get; set; }
        public int? SoNam { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public bool? IsThanhToanBangTienMat { get; set; }
    }
}