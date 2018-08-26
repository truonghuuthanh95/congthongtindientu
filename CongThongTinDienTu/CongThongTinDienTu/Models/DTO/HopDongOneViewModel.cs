using CongThongTinDienTu.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class HopDongOneViewModel
    {
        public List<District> Districts { get; set; }
        public List<CapTruong> CapTruongs { get; set; }
        public List<School> Schools { get; set; }

        public HopDongOneViewModel(List<District> districts, List<CapTruong> capTruongs, List<School> schools)
        {
            Districts = districts;
            CapTruongs = capTruongs;
            Schools = schools;
        }
    }
}