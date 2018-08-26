using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class CapTruongRepository : ICapTruongRepository
    {
        CongThongTinDienTuDB _db;

        public CapTruongRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public List<CapTruong> GetCapTruongs()
        {
            List<CapTruong> capTruongs = _db.CapTruongs.Where(s => s.IsActive == true).ToList();
            return capTruongs;
        }
    }
}