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
        HopDong TaoMoiHopDong(HopDongDTO hopDongDTO, int nguoiTiepNhan, bool isTaoMoi);
        int GetMaxSoHopDong();

        List<HopDong> GetHopDongsByYear(int year);
    }
}