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
    public class SchoolRepository : ISchoolRepository
    {
        CongThongTinDienTuDB _db;

        public SchoolRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public School GetSchoolById(int? id)
        {
            School school = _db.Schools.Include("Ward.District").Where(s => s.Id == id).SingleOrDefault();
            return school;
        }

        public School GetSchoolByMaTruong(string matruong)
        {
            School school = _db.Schools.Include("Ward.District").Where(s => s.MaTruong.ToUpper() == matruong.Trim().ToUpper()).FirstOrDefault();
            return school;
        }

        public List<School> GetSchoolsByDistrictAndCapHoc(int districtId, int caphoc)
        {
            List<School> schools = _db.Schools.Where(s => s.Ward.DistrictID == districtId).Where(s => s.CapTruongId == caphoc).ToList();

            return schools;
        }       
        public List<School> GetSchoolsByDistrictAndCapHocWithIsDaTaoLapFalse(int districtId, int caphoc)
        {
            List<School> schools = _db.Schools.Where(s => s.Ward.DistrictID == districtId).Where(s => s.CapTruongId == caphoc).Where(s => s.IsDaTaoMoi == false || s.IsDaTaoMoi == null).ToList();
            return schools;
        }

        public School UpdateSchoolInfomation(SchoolDTO schoolDTO, int id)
        {
            School school = GetSchoolById(id);
            school.WardId = schoolDTO.WardId;
            school.SoNhaTenDuong = schoolDTO.SoNhaTenDuong;
            school.Email = schoolDTO.Email;
            school.SDT = schoolDTO.SDT;
            school.MaSoThue = schoolDTO.MaSoThue;
            school.HieuTruong = schoolDTO.HieuTruong;
            _db.Entry(school).State = EntityState.Modified;
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                return null;
            }
            return school;
        }
    }
}