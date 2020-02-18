using CongThongTinDienTu.Models.DAO;
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
            List<HopDong> hopDongs = _db.HopDongs.AsNoTracking().Where(s => s.SchoolId == schoolId).OrderByDescending(s => s.NgayKiHD).ToList();
            return hopDongs;
        }

        public List<HopDong> GetHopDongsByYear(int year)
        {
            List<HopDong> hopDongs = _db.HopDongs.AsNoTracking().Include("School.Ward.District").Include("School.DVQL").Include("School.CapTruong").Where(s => s.NgayKiHD.Value.Year == year).OrderBy(s => s.NgayKiHD).ToList();
            return hopDongs;
        }

        public int GetMaxSoHopDong(int year)
        {
            var max = _db.HopDongs.AsNoTracking().Where(s => s.NgayHieuLucHD.Value.Year == year).Max(s => s.MaHopDong);
            if (max == null)
            {
                return 1;
            }
            return Convert.ToInt32(max);
        }

        public List<School> GetSchoolsChuaCoCongThongTin(int? dvqlId)
        {
            List<School> schools = _db.Schools.AsNoTracking()
                .Include("Ward.District")
                .Include("CapTruong")
                .Where(s => s.IsDaTaoMoi == false || s.IsDaTaoMoi == null).Where(s => s.DVQLId == dvqlId).ToList();
            return schools;
        }        

        public List<HopDong> GetSchoolsChuaDongTien(int? dvqlId)
        {
            List<HopDong> hopDongs = _db.HopDongs.AsNoTracking().Include("School.CapTruong").Where(s => s.NgayThanhToan == null).Where(s => s.School.DVQLId == dvqlId).ToList();
            return hopDongs;
        }

        public List<School> GetSchoolsChuaGiaHan(int? dvqlId)
        {
            DateTime dateTimeCompare = DateTime.Now.AddYears(-1);
            
            var hopDongDaGiaHan = _db.HopDongs.Where(s => s.School.DVQLId == dvqlId && s.NgayHieuLucHD > dateTimeCompare).Select(s => s.School).ToList();
            var listSchoolIdChuaCanGiaHan = new List<int>();
            foreach (var item in hopDongDaGiaHan)
            {
                listSchoolIdChuaCanGiaHan.Add(item.Id);
            }
            var hopDongs = _db.HopDongs.Where(s => s.School.DVQLId == dvqlId && s.NgayHieuLucHD < dateTimeCompare).Select(s => s.School).ToList();
            var listSchooId = new List<int>();
            foreach (var item in hopDongs)
            {
                listSchooId.Add(item.Id);
            }
            var schoolHetHan = new List<int>();
            foreach (var item in listSchooId)
            {
                if (!listSchoolIdChuaCanGiaHan.Contains(item))
                {
                    schoolHetHan.Add(item);
                }
            }
            List<School> SchoolChuaGiaHan = _db.Schools.Include("CapTruong").Where(s => s.DVQLId == dvqlId && s.IsDaTaoMoi == true).Where(s => schoolHetHan.Contains(s.Id)).ToList();
            return SchoolChuaGiaHan;
        }

        public int GetSoLuongChuaCoCongThongTin(int? dvqlId)
        {
            int schoolsSoLuong = _db.Schools.AsNoTracking().Where(s => s.IsDaTaoMoi == false || s.IsDaTaoMoi == null).Where(s => s.DVQLId == dvqlId).Count();
            return schoolsSoLuong;
        }

        public int GetSoLuongChuaGiaHan(int? dvqlId)
        {

            DateTime dateTimeCompare = DateTime.Now.AddYears(-1);

            var hopDongDaGiaHan = _db.HopDongs.Where(s => s.School.DVQLId == dvqlId && s.NgayHieuLucHD > dateTimeCompare).Select(s => s.School).ToList();
            var listSchoolIdChuaCanGiaHan = new List<int>();
            foreach (var item in hopDongDaGiaHan)
            {
                listSchoolIdChuaCanGiaHan.Add(item.Id);
            }
            var hopDongs = _db.HopDongs.Where(s => s.School.DVQLId == dvqlId && s.NgayHieuLucHD < dateTimeCompare).Select(s => s.School).ToList();
            var listSchooId = new List<int>();
            foreach (var item in hopDongs)
            {
                listSchooId.Add(item.Id);
            }
            var schoolHetHan = new List<int>();
            foreach (var item in listSchooId)
            {
                if (!listSchoolIdChuaCanGiaHan.Contains(item))
                {
                    schoolHetHan.Add(item);
                }
            }
            int count = _db.Schools.Where(s => s.DVQLId == dvqlId && s.IsDaTaoMoi == true).Where(s => schoolHetHan.Contains(s.Id)).Count();
            return count;
        }

        public int GetSoLuongChuaThanhToan(int? dvqlId)
        {
            int count = _db.HopDongs.AsNoTracking().Where(s => s.NgayThanhToan == null && s.School.DVQLId == dvqlId).Count();
            return count;
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
            //HopDong hopDong = new HopDong();
            //hopDong.CreatedAt = DateTime.Now;
            //hopDong.NguoiTiepNhan = nguoiTiepNhan;
            //hopDong.GhiChu = hopDongDTO.GhiChu;
            //hopDong.NgayHieuLucHD = hopDongDTO.NgayHieuLucHD;
            //hopDong.NgayKiHD = hopDongDTO.NgayKiHD;
            //hopDong.SchoolId = hopDongDTO.SchoolId;
            //hopDong.SoNam = hopDongDTO.SoNam;
            //hopDong.IsThanhToanBangTienMat = hopDongDTO.IsThanhToanBangTienMat;
            //hopDong.NgayThanhToan = hopDongDTO.NgayThanhToan;
            //hopDong.IsTaoMoi = false;
            //hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienDuyTry"]) * hopDongDTO.SoNam;
            //int checkNumThisYear = (hopDongDTO.NgayKiHD.Year % 100) * 10000;
            //int maxSoHopdong = GetMaxSoHopDong();
            //if (checkNumThisYear > maxSoHopdong)
            //{
            //    hopDong.MaHopDong = checkNumThisYear + 1;
            //}
            //else
            //{
            //    hopDong.MaHopDong = maxSoHopdong + 1;
            //}
            //_db.HopDongs.Add(hopDong);
            //_db.SaveChanges();
            return null;
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
            int checkNumThisYear = (hopDongDTO.NgayHieuLucHD.Year % 100) * 10000;
            int maxSoHopdong = GetMaxSoHopDong(hopDongDTO.NgayHieuLucHD.Year);
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