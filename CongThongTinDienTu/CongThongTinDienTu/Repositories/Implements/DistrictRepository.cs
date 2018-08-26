using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class DistrictRepository : IDistrictRepository
    {
        CongThongTinDienTuDB _db;

        public DistrictRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public List<District> GetDistrictsByProvinceId(int provinceId)
        {
            List<District> districts = _db.Districts.Where(s => s.ProvinceId == provinceId).OrderByDescending(s => s.Type).ThenBy(s => s.Name).ToList();
            return districts;
        }
    }
}