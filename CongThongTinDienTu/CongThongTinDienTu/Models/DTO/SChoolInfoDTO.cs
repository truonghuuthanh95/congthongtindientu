using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class SChoolInfoDTO
    {
        public int Id { get; set; }

        public string MaTruong { get; set; }   
        public string TenTruong { get; set; }

        public SChoolInfoDTO(int id, string maTruong, string tenTruong)
        {
            Id = id;
            MaTruong = maTruong;
            TenTruong = tenTruong;
        }
    }
}