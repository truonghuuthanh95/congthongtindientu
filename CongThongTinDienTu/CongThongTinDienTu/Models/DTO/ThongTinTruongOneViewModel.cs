using CongThongTinDienTu.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Models.DTO
{
    public class ThongTinTruongOneViewModel
    {
        public School School { get; set; }
        public List<District> Districts { get; set; }
        public List<Ward> Wards { get; set; }

        public ThongTinTruongOneViewModel(School school, List<District> districts, List<Ward> wards)
        {
            School = school;
            Districts = districts;
            Wards = wards;
        }
    }
}