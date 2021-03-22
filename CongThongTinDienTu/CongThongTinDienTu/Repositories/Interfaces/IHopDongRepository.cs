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
        int GetMaxSoHopDong(int year);

        List<HopDong> GetHopDongsByYear(int year);
        List<HopDong> GetHopDongsBySchoolId(int schoolId);
        HopDong GetHopDongById(int id);
        HopDong CapNhatHopDong(HopDong hopDong);
        List<School> GetSchoolsChuaCoCongThongTin(int? dvqlId);
        int GetSoLuongChuaCoCongThongTin(int? dvqlId);        
        List<School> GetSchoolsChuaGiaHan(int? dvqlId);
        List<HopDong> GetSchoolsChuaDongTien(int? dvqlId);
        int GetSoLuongChuaThanhToan(int? dvqlId);
        int GetSoLuongChuaGiaHan(int? dvqlId);
        HopDong GetLastestHopDongBySchoolId(int schoolId);
    }
}