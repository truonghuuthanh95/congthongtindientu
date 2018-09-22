using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongThongTinDienTu.Controllers
{
    public class SchoolController : Controller
    {
        ISchoolRepository schoolRepository;
        IWardRepository wardRepository;
        IDistrictRepository districtRepository;
        IHopDongRepository hopDongRepository;

        public SchoolController(ISchoolRepository schoolRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IHopDongRepository hopDongRepository)
        {
            this.schoolRepository = schoolRepository;
            this.wardRepository = wardRepository;
            this.districtRepository = districtRepository;
            this.hopDongRepository = hopDongRepository;
        }


        // GET: School
        [Route("capnhatthongtintruong")]
        [HttpPost]
        public ActionResult CapNhatThongTinTruong(SchoolDTO schoolDTO, int id)
        {
            School school = schoolRepository.UpdateSchoolInfomation(schoolDTO, id);
            if (school.IsDaTaoMoi == false || school.IsDaTaoMoi == null)
            {
                return Json(new ReturnFormat(400, "success", school.MaTruong));
            }
            if (school.IsDaTaoMoi == true)
            {
                return Json(new ReturnFormat(200, "true", school.MaTruong));
            }
            return Json(new ReturnFormat(200, "false", school.MaTruong));           
        }
        [Route("getWardByDistrictId/{id}")]
        [HttpGet]       
        public ActionResult GetWardByDistrictId(int id)
        {
            
            List<Ward> wards = wardRepository.GetWardsByDistrictId(id);
            var wardsJson = JsonConvert.SerializeObject(wards,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Json(new ReturnFormat(200, "success", wardsJson), JsonRequestBehavior.AllowGet);
        }
        [Route("getSchoolByDistrictAndCapTruong/{districtId}/{captruongId}")]
        [HttpGet]
        public ActionResult GetSchoolByDistrictAnCapTruong(int districtId, int captruongId)
        {

            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHoc(districtId, captruongId);
            var schoolJson = JsonConvert.SerializeObject(schools,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Json(new ReturnFormat(200, "success", schoolJson), JsonRequestBehavior.AllowGet);
        }
        [Route("getSchoolsChuaGiaHan/{districtId}/{captruongId}")]
        [HttpGet]
        public ActionResult GetSchoolsByDistrictAndCapHocWithDaTaoLapAndChuaGiaHanThisYear(int districtId, int captruongId)
        {
            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHoc(districtId, captruongId).Where(s => s.IsDaTaoMoi == true).ToList();
            List<School> schoolDaGiaHan = hopDongRepository.GetHopDongsByYear(DateTime.Now.Year).Select(s => s.School).ToList();

            List<School> schoolChuaGiaHan = schools.Where(s => !schoolDaGiaHan.Any(s1 => s.Id == s1.Id)).ToList();                      
            var schoolJson = JsonConvert.SerializeObject(schoolChuaGiaHan,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Json(new ReturnFormat(200, "success", schoolJson), JsonRequestBehavior.AllowGet);
        }
        [Route("getSchoolByDistrictAndCapTruongWithIsTaoLapFalse/{districtId}/{captruongId}")]
        [HttpGet]
        public ActionResult GetSchoolByDistrictAnCapTruongWithIsTaoLapFalse(int districtId, int captruongId)
        {

            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHocWithIsDaTaoLapFalse(districtId, captruongId);
            var schoolJson = JsonConvert.SerializeObject(schools,
            Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            return Json(new ReturnFormat(200, "success", schoolJson), JsonRequestBehavior.AllowGet);
        }
    }
}