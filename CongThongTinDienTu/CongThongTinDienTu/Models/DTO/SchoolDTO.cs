using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class SchoolDTO
    {
        
        public int WardId { get; set; }
       
        public string SoNhaTenDuong { get; set; }              
               
        public string HieuTruong { get; set; }
      
        public string Email { get; set; }

        public string SDT { get; set; }

        public string MaSoThue { get; set; }
    }
}