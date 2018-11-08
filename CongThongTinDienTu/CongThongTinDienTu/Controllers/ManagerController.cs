using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Interfaces;
using CongThongTinDienTu.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongThongTinDienTu.Controllers
{
    [RoutePrefix("quanly")]
    public class ManagerController : Controller
    {
        IHopDongRepository hopDongRepository;
        ISchoolRepository schoolRepository;
        ICapTruongRepository capTruongRepository;
        IDistrictRepository districtRepository;

        public ManagerController(IHopDongRepository hopDongRepository, ISchoolRepository schoolRepository, ICapTruongRepository capTruongRepository, IDistrictRepository districtRepository)
        {
            this.hopDongRepository = hopDongRepository;
            this.schoolRepository = schoolRepository;
            this.capTruongRepository = capTruongRepository;
            this.districtRepository = districtRepository;
        }
        // GET: Manager
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            return View();
        }
        
        [Route("hopdongduytri")]
        [HttpGet]
        public ActionResult ThemMoiHopDongGiaHan()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            List<District> districts = districtRepository.GetDistrictsByProvinceId(79);
            List<CapTruong> captruongs = capTruongRepository.GetCapTruongs();
            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHoc(760, 1).Where(s => s.IsDaTaoMoi == true).ToList();
            //List<School> schoolDaGiaHan = hopDongRepository.GetHopDongsByYear(DateTime.Now.Year).Select(s => s.School).ToList();
            //List<School> schoolChuaGiaHan = schools.Where(s => !schoolDaGiaHan.Any(s1 => s.Id == s1.Id)).ToList();
            HopDongOneViewModel hopDongOneViewModel = new HopDongOneViewModel(districts, captruongs, schools);
            return View(hopDongOneViewModel);
        }
        [Route("themmoihopdonggiahan")]
        [HttpPost]
        public ActionResult ThemMoiHopDongDuyTry(HopDongDTO hopDongDTO)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return Json(new ReturnFormat(403, "Access Denied", null), JsonRequestBehavior.AllowGet);
            }
            HopDong hopDong = hopDongRepository.TaoMoiHopDongDuyTri(hopDongDTO, account.Id);
            var hopDongJson = JsonConvert.SerializeObject(hopDong,
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
            return Json(new ReturnFormat(200, "success", hopDongJson), JsonRequestBehavior.AllowGet);
        }
        [Route("hopdongtaolap")]
        [HttpGet]
        public ActionResult HopDongTaoLap()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }

            List<District> districts = districtRepository.GetDistrictsByProvinceId(79);
            List<CapTruong> captruongs = capTruongRepository.GetCapTruongs();
            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHoc(760, 1);
            HopDongOneViewModel hopDongOneViewModel = new HopDongOneViewModel(districts, captruongs, schools);
            return View(hopDongOneViewModel);
        }
        [Route("themmoihopdongtaomoi")]
        [HttpPost]
        public ActionResult ThemmoihopDongTaoMoi(HopDongDTO hopDongDTO)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return Json(new ReturnFormat(403, "Access Denied", null), JsonRequestBehavior.AllowGet);
            }
            hopDongDTO.SoNam = 1;
            HopDong hopDong = hopDongRepository.TaoMoiHopDongTaoLap(hopDongDTO, account.Id);
            var hopDongJson = JsonConvert.SerializeObject(hopDong,
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
            return Json(new ReturnFormat(200, "success", hopDongJson), JsonRequestBehavior.AllowGet);
        }
        [Route("danhsachhopdong/{year}")]
        [HttpGet]
        public ActionResult DanhSachHopDongTheoNam(int year)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            List<HopDong> hopDongs = hopDongRepository.GetHopDongsByYear(year);
            ViewBag.Year = year;
            return View(hopDongs);
        }
        [Route("capnhatngaydongtien/{id}/{ngaydongtien}/{httt}")]
        [HttpGet]
        public ActionResult CapNhatNgayDongTien(int id, string ngayDongTien, bool httt)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return Json(new ReturnFormat(403, "Access denied", null), JsonRequestBehavior.AllowGet);
            }
            HopDong hopDong = hopDongRepository.GetHopDongById(id);
            if (hopDong == null)
            {
                return Json(new ReturnFormat(404, "Not found", null), JsonRequestBehavior.AllowGet);
            }
            hopDong.NgayThanhToan =  DateTime.ParseExact(ngayDongTien, "dd-MM-yyyy", null);
            hopDong.IsThanhToanBangTienMat = httt;
            HopDong hopDongDaCapNhat = hopDongRepository.CapNhatHopDong(hopDong);
            if (hopDongDaCapNhat == null)
            {
                return Json(new ReturnFormat(400, "Update failed", null), JsonRequestBehavior.AllowGet);
            }
            return Json(new ReturnFormat(200, "success", null), JsonRequestBehavior.AllowGet);
        }
        [Route("kiemtratruong")]
        [HttpGet]
        public ActionResult TimKiemTruong()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            List<District> districts = districtRepository.GetDistrictsByProvinceId(79);
            List<CapTruong> captruongs = capTruongRepository.GetCapTruongs();
            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHoc(760, 1).ToList();
            ViewBag.Districts = districts;
            ViewBag.CapTruongs = captruongs;
            ViewBag.Schools = schools;
            return View();
        }
        [Route("gethopdongbyschoolId/{schoolId}")]
        [HttpGet]
        public ActionResult HopDongTruong(int schoolId)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return Json(new ReturnFormat(403, "Access denied", null), JsonRequestBehavior.AllowGet);
            }
            string MaTruong = schoolRepository.GetSchoolById(schoolId).MaTruong;
            List<HopDong> HopDongs = hopDongRepository.GetHopDongsBySchoolId(schoolId);
            var hopDongJson = JsonConvert.SerializeObject(new KiemTraTruongDTO(MaTruong, HopDongs),
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
            return Json(new ReturnFormat(200, "success", hopDongJson), JsonRequestBehavior.AllowGet);
        }
    }
}