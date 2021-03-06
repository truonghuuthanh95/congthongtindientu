﻿using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class HopDongRepository : IHopDongRepository
    {
        CongThongTinDienTuDB _db;
        ISchoolRepository schoolRepository;

        public HopDongRepository(CongThongTinDienTuDB db, ISchoolRepository schoolRepository)
        {
            _db = db;
            this.schoolRepository = schoolRepository;
        }

        public HopDong CapNhatHopDong(HopDong hopDong)
        {

            _db.Entry(hopDong).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                return null;
            }
            return hopDong;
        }

        public HopDong GetHopDongById(int id)
        {
            HopDong hopDong = _db.HopDongs.Where(s => s.Id == id).SingleOrDefault();
            return hopDong;
        }

        public List<HopDong> GetHopDongsBySchoolId(int schoolId)
        {
            List<HopDong> hopDongs = _db.HopDongs.Where(s => s.SchoolId == schoolId).OrderByDescending(s => s.NgayKiHD).ToList();
            return hopDongs;
        }

        public List<HopDong> GetHopDongsByYear(int year)
        {
            List<HopDong> hopDongs = _db.HopDongs.Include("School.Ward.District").Include("School.DVQL").Include("School.CapTruong").Where(s => s.NgayKiHD.Value.Year == year).OrderBy(s => s.NgayKiHD).ToList();
            return hopDongs;
        }

        public int GetMaxSoHopDong()
        {
            var max = _db.HopDongs.Max(s => s.MaHopDong);
            if (max == null)
            {
                return 1;
            }
            return Convert.ToInt32(max);
        }

        //public HopDong TaoMoiHopDong(HopDongDTO hopDongDTO, int nguoiTiepNhan, bool isTaoMoi)
        //{
        //    HopDong hopDong = new HopDong();
        //    hopDong.CreatedAt = DateTime.Now;
        //    hopDong.NguoiTiepNhan = nguoiTiepNhan;
        //    hopDong.GhiChu = hopDongDTO.GhiChu;            
        //    hopDong.NgayHieuLucHD = hopDongDTO.NgayHieuLucHD;
        //    hopDong.NgayKiHD = hopDongDTO.NgayKiHD;
        //    hopDong.SchoolId = hopDongDTO.SchoolId;
        //    hopDong.SoNam = hopDongDTO.SoNam;
        //    hopDong.IsThanhToanBangTienMat = hopDongDTO.IsThanhToanBangTienMat;
        //    hopDong.NgayThanhToan = hopDongDTO.NgayThanhToan;
        //    if (isTaoMoi == false)
        //    {
        //        hopDong.IsTaoMoi = false;

        //        hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienDuyTry"]) * hopDongDTO.SoNam;
        //    }
        //    else
        //    {
        //        hopDong.IsTaoMoi = true;
        //        School school = schoolRepository.GetSchoolById(hopDong.SchoolId);
        //        if (school.Ward.District.Type.Trim() == "Huyện")
        //        {
        //            hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienHuyenTL"]);
        //        }
        //        else
        //        {
        //            hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienQuanTL"]);
        //        }
        //        school.IsDaTaoMoi = true;
        //        _db.Entry(school).State = EntityState.Modified;
        //        //_db.SaveChanges();
        //    }
            
        //    int checkNumThisYear = (DateTime.Now.Year % 100)*10000;
        //    int maxSoHopdong = GetMaxSoHopDong();
        //    if (checkNumThisYear > maxSoHopdong)
        //    {
        //        hopDong.MaHopDong = checkNumThisYear + 1;
        //    }
        //    else
        //    {
        //        hopDong.MaHopDong = maxSoHopdong + 1;
        //    }
        //    _db.HopDongs.Add(hopDong);
        //    _db.SaveChanges();
        //    return hopDong;               
        //}

        public HopDong TaoMoiHopDongDuyTri(HopDongDTO hopDongDTO, int nguoiTiepNhan)
        {
            HopDong hopDong = new HopDong();
            hopDong.CreatedAt = DateTime.Now;
            hopDong.NguoiTiepNhan = nguoiTiepNhan;
            hopDong.GhiChu = hopDongDTO.GhiChu;
            hopDong.NgayHieuLucHD = hopDongDTO.NgayHieuLucHD;
            hopDong.NgayKiHD = hopDongDTO.NgayKiHD;
            hopDong.SchoolId = hopDongDTO.SchoolId;
            hopDong.SoNam = hopDongDTO.SoNam;
            hopDong.IsThanhToanBangTienMat = hopDongDTO.IsThanhToanBangTienMat;
            hopDong.NgayThanhToan = hopDongDTO.NgayThanhToan;
            hopDong.IsTaoMoi = false;
            hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienDuyTry"]) * hopDongDTO.SoNam;
            int checkNumThisYear = (DateTime.Now.Year % 100) * 10000;
            int maxSoHopdong = GetMaxSoHopDong();
            if (checkNumThisYear > maxSoHopdong)
            {
                hopDong.MaHopDong = checkNumThisYear + 1;
            }
            else
            {
                hopDong.MaHopDong = maxSoHopdong + 1;
            }
            _db.HopDongs.Add(hopDong);
            _db.SaveChanges();
            return hopDong;
        }

        public HopDong TaoMoiHopDongTaoLap(HopDongDTO hopDongDTO, int nguoiTiepNhan)
        {
            HopDong hopDong = new HopDong();
            hopDong.CreatedAt = DateTime.Now;
            hopDong.NguoiTiepNhan = nguoiTiepNhan;
            hopDong.GhiChu = hopDongDTO.GhiChu;
            hopDong.NgayHieuLucHD = hopDongDTO.NgayHieuLucHD;
            hopDong.NgayKiHD = hopDongDTO.NgayKiHD;
            hopDong.SchoolId = hopDongDTO.SchoolId;
            hopDong.SoNam = hopDongDTO.SoNam;
            hopDong.IsThanhToanBangTienMat = hopDongDTO.IsThanhToanBangTienMat;
            hopDong.NgayThanhToan = hopDongDTO.NgayThanhToan;

            hopDong.IsTaoMoi = true;
            School school = schoolRepository.GetSchoolById(hopDongDTO.SchoolId);
            if (school.Ward.District.Type.Trim() == "Huyện")
            {
                hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienHuyenTL"]);
            }
            else
            {
                hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienQuanTL"]);
            }
            int updateSchoolIsTaoMoi = _db.Database.ExecuteSqlCommand(@"Update School 
            set IsDaTaoMoi = 1 where Id =" + school.Id);
            int checkNumThisYear = (DateTime.Now.Year % 100) * 10000;
            int maxSoHopdong = GetMaxSoHopDong();
            if (checkNumThisYear > maxSoHopdong)
            {
                hopDong.MaHopDong = checkNumThisYear + 1;
            }
            else
            {
                hopDong.MaHopDong = maxSoHopdong + 1;
            }
            _db.HopDongs.Add(hopDong);
            _db.SaveChanges();
            return hopDong;           
        }
    }
}