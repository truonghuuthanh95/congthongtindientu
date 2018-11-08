using CongThongTinDienTu.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class KiemTraTruongDTO
    {
        public string MaTruong { get; set; }
        public List<HopDong> HopDongs { get; set; }

        public KiemTraTruongDTO(string maTruong, List<HopDong> hopDongs)
        {
            MaTruong = maTruong;
            HopDongs = hopDongs;
        }
    }
}