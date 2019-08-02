using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using CongThongTinDienTu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongThongTinDienTu.Controllers
{
    [RoutePrefix("pgd")]
    public class ManagementUnitController : Controller
    {
        IAccountPermissionRepository accountPermissionRepository;
        IHopDongRepository hopDongRepository;

        public ManagementUnitController(IAccountPermissionRepository accountPermissionRepository, IHopDongRepository hopDongRepository)
        {
            this.accountPermissionRepository = accountPermissionRepository;
            this.hopDongRepository = hopDongRepository;
        }


        // GET: ManagementUnit
        [Route("danhsachhopdong/{year}")]
        public ActionResult Index(int year)
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            int permissionId = 4;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return RedirectToRoute("login");
            }
            using (var _db = new CongThongTinDienTuDB())
            {
                List<HopDong> hopDongs = _db.HopDongs.Include("School.Ward.District").Include("School.DVQL").Include("School.CapTruong").Where(s => s.School.DVQLId == account.DVQLId && s.NgayKiHD.Value.Year == year).OrderBy(s => s.NgayKiHD).ToList();
                ViewBag.Year = year;
                return View(hopDongs);
            }
            
        }
        [Route("danhsachtruongchuacocttdt")]
        public ActionResult DanhSachTruongChuaCTTDT()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            int permissionId = 4;

            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return RedirectToRoute("login");
            }
            List<School> schools = hopDongRepository.GetSchoolsChuaCoCongThongTin(account.DVQLId);
            ViewBag.SchoolsChuaCoCTTDT = schools;
            return View();
        }
        [Route("danhsachtruongchuathanhtoan")]
        public ActionResult DanhSachChuaThanhToan()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            int permissionId = 4;
            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return RedirectToRoute("login");
            }
            List<HopDong> hopDongs = hopDongRepository.GetSchoolsChuaDongTien(account.DVQLId);
            ViewBag.ChuaDongTiens = hopDongs;
            return View();
        }
        [Route("danhsachtruongchualamhopdonggiahan")]
        public ActionResult DanhSachChuaLamHopDongGiaHan()
        {
            Account account = (Account)Session[CommonConstant.USER_SESSION];
            if (account == null)
            {
                return RedirectToRoute("login");
            }
            int permissionId = 4;
            List<Account_Permission> account_Permissions = accountPermissionRepository.GetAccount_PermissionsByAccountId(account.Id);
            if (account_Permissions.Where(s => s.PermissionId == permissionId).SingleOrDefault() == null)
            {
                return RedirectToRoute("login");
            }
            List<School> schools = hopDongRepository.GetSchoolsChuaGiaHan(account.DVQLId);
            ViewBag.SchoolChuaGiaHans = schools;
            return View();
        }
    }
}