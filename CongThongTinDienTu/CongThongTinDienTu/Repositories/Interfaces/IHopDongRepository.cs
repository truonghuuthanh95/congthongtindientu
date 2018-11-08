using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Interfaces
{
    public interface IHopDongRepository
    {
        HopDong TaoMoiHopDongTaoLap(HopDongDTO hopDongDTO, int nguoiTiepNhan);
        HopDong TaoMoiHopDongDuyTri(HopDongDTO hopDongDTO, int nguoiTiepNhan);
        int GetMaxSoHopDong();

        List<HopDong> GetHopDongsByYear(int year);
        List<HopDong> GetHopDongsBySchoolId(int schoolId);
        HopDong GetHopDongById(int id);
        HopDong CapNhatHopDong(HopDong hopDong);
    }
}