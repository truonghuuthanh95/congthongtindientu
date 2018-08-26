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
            List<School> schoolDaGiaHan = hopDongRepository.GetHopDongsByYear(DateTime.Now.Year).Select(s => s.School).ToList();

            List<School> schoolChuaGiaHan = schools.Where(s => !schoolDaGiaHan.Any(s1 => s.Id == s1.Id)).ToList();
            HopDongOneViewModel hopDongOneViewModel = new HopDongOneViewModel(districts, captruongs, schoolChuaGiaHan);
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
            HopDong hopDong = hopDongRepository.TaoMoiHopDong(hopDongDTO, 2, false);
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
            List<School> schools = schoolRepository.GetSchoolsByDistrictAndCapHocWithIsDaTaoLapFalse(760, 1);
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
            HopDong hopDong = hopDongRepository.TaoMoiHopDong(hopDongDTO, 2, true);
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
    }
}