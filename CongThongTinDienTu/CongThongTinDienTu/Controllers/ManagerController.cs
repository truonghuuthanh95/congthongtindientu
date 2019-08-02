using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Implements;
using CongThongTinDienTu.Repositories.Interfaces;
using CongThongTinDienTu.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        IAccountPermissionRepository accountPermissionRepository;

        public ManagerController(IHopDongRepository hopDongRepository, ISchoolRepository schoolRepository, ICapTruongRepository capTruongRepository, IDistrictRepository districtRepository, IAccountPermissionRepository accountPermissionRepository)
        {
            this.hopDongRepository = hopDongRepository;
            this.schoolRepository = schoolRepository;
            this.capTruongRepository = capTruongRepository;
            this.districtRepository = districtRepository;
            this.accountPermissionRepository = accountPermissionRepository;
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
            if (account.DVQLId != 25)
            {
                int SoLuongChuaCoCongThongTin = hopDongRepository.GetSoLuongChuaCoCongThongTin(account.DVQLId);
                ViewBag.SchoolsChuaCoCongThongTin = SoLuongChuaCoCongThongTin;
                ViewBag.SchoolsChuaDongTien = hopDongRepository.GetSoLuongChuaThanhToan(account.DVQLId);
                ViewBag.SchoolsChuaGiaHan = hopDongRepository.GetSoLuongChuaGiaHan(account.DVQLId);
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
            int permissionId = 2;
            
            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
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
        public int GetMaxSoHopDong(int year)
        {
            using (var _db = new CongThongTinDienTuDB())
            {
                var max = _db.HopDongs.Where(s => s.NgayHieuLucHD.Value.Year == year).Max(s => s.MaHopDong);
                if (max == null)
                {
                    return 1;
                }
                return Convert.ToInt32(max);
            }
            
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
            int permissionId = 2;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return Json(new ReturnFormat(403, "Access Denied", null), JsonRequestBehavior.AllowGet);
            }
            
            HopDong hopDong = new HopDong();
            using (var _db = new CongThongTinDienTuDB())
            {               
                hopDong.CreatedAt = DateTime.Now;
                hopDong.NguoiTiepNhan = account.Id;
                hopDong.GhiChu = hopDongDTO.GhiChu;
                hopDong.NgayHieuLucHD = hopDongDTO.NgayHieuLucHD;
                hopDong.NgayKiHD = hopDongDTO.NgayKiHD;
                hopDong.SchoolId = hopDongDTO.SchoolId;
                hopDong.SoNam = hopDongDTO.SoNam;
                hopDong.IsThanhToanBangTienMat = hopDongDTO.IsThanhToanBangTienMat;
                hopDong.NgayThanhToan = hopDongDTO.NgayThanhToan;
                hopDong.IsTaoMoi = false;
                hopDong.SoTien = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TienDuyTry"]) * hopDongDTO.SoNam;
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
            }           
            //HopDong hopDong = hopDongRepository.TaoMoiHopDongDuyTri(hopDongDTO, account.Id);
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
            int permissionId = 1;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
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
            int permissionId = 1;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
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
            int permissionId = 3;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return RedirectToRoute("login");
            }
            List<HopDong> hopDongs = hopDongRepository.GetHopDongsByYear(year);
            ViewBag.Year = year;
            return View(hopDongs);
        }
        [Route("capnhathopdong")]
        [HttpPost]
        public ActionResult CapNhatNgayDongTien(int id, string ngayDongTien, string httt, string ngayKiHD, string ngayHieuLuc)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return Json(new ReturnFormat(403, "Access Denied", null), JsonRequestBehavior.AllowGet);
            }
            //cap nhat hop dong
            int permissionId = 5;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return Json(new ReturnFormat(403, "Access Denied", null), JsonRequestBehavior.AllowGet);
            }
            HopDong hopDong = hopDongRepository.GetHopDongById(id);
            if (hopDong == null)
            {
                return Json(new ReturnFormat(404, "Not found", null), JsonRequestBehavior.AllowGet);
            }
            if (httt == "no")
            {
                hopDong.IsThanhToanBangTienMat = null;
            }
            else
            {
                hopDong.IsThanhToanBangTienMat = bool.Parse(httt);
            }
            if (ngayDongTien != null && ngayDongTien != "")
            {
                hopDong.NgayThanhToan = DateTime.ParseExact(ngayDongTien, "dd-MM-yyyy", null);
            }
            else
            {
                hopDong.NgayThanhToan = null;
            }
            
            hopDong.NgayHieuLucHD = DateTime.ParseExact(ngayHieuLuc, "dd-MM-yyyy", null);
            hopDong.NgayKiHD = DateTime.ParseExact(ngayKiHD, "dd-MM-yyyy", null);
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
            int permissionId = 6;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
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

            //School school = schoolRepository.GetSchoolById(schoolId);
            using (var db = new CongThongTinDienTuDB())
            {
                List<HopDong> HopDongs = db.HopDongs.Where(s => s.SchoolId == schoolId).ToList();
                KiemTraTruongDTO kiemTraTruongDTO = new KiemTraTruongDTO("000000", HopDongs);
                var hopDongJson = JsonConvert.SerializeObject(kiemTraTruongDTO,
               Formatting.None,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               });
                return Json(new ReturnFormat(200, "success", hopDongJson), JsonRequestBehavior.AllowGet);
            }           
            
        }
        [Route("downloadhopdong/{year}")]
        [HttpGet]
        public async Task<ActionResult> DownloadHopDong(int year)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            List<HopDong> HopDongs = hopDongRepository.GetHopDongsByYear(year);
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Utils/ds-hopdongcttdt.xlsx");
            await Utils.ExportExcel.GenerateXLSHopDong(HopDongs, filePath);          
            return File(filePath, "application/vnd.ms-excel", "ds-hopdongcttdt.xlsx");
        }
        [Route("gethopdongbyid/{id}")]
        [HttpGet]
        public ActionResult GetHopDingById(int id)
        {
            using (var db = new CongThongTinDienTuDB())
            {
                HopDong hopDong = db.HopDongs.Find(id);
                var hopDongJson = JsonConvert.SerializeObject(hopDong,
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
                return Json(new ReturnFormat(200, "success", hopDongJson), JsonRequestBehavior.AllowGet);
            }
             
            
        }
    }
}