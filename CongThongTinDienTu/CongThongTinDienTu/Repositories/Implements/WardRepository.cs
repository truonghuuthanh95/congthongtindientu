using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class WardRepository : IWardRepository
    {
        CongThongTinDienTuDB _db;

        public WardRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public List<Ward> GetWardsByDistrictId(int districtId)
        {
            List<Ward> wards = _db.Wards.Where(s => s.DistrictID == districtId).OrderBy(s => s.Name).ToList();
            return wards;
        }
    }
}