using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongThongTinDienTu.Controllers
{
    public class InHopDongController : Controller
    {
        
        ISchoolRepository schoolRepository;
        IDistrictRepository districtRepository;
        IWardRepository wardRepository;
        IHopDongRepository hopDongRepository;

        public InHopDongController(ISchoolRepository schoolRepository, IDistrictRepository districtRepository, IWardRepository wardRepository, IHopDongRepository hopDongRepository)
        {
            this.schoolRepository = schoolRepository;
            this.districtRepository = districtRepository;
            this.wardRepository = wardRepository;
            this.hopDongRepository = hopDongRepository;
        }


        // GET: InHopDong
        [Route("kiemtramatruong",Name ="kiemtramatruong")]
        [HttpGet]
        public ActionResult KiemTraMaTruong()
        {
            return View();
        }
        [Route("kiemtramatruong/{matruong}")]
        [HttpGet]
        public ActionResult CheckValidSchool(string matruong)
        {
            School school = schoolRepository.GetSchoolByMaTruong(matruong.Trim());
            if (school == null)
            {
                return Json(new ReturnFormat(404, "Không tìm thấy trường với mã " + matruong.Trim() + ". Vui lòng kiểm tra lại.", null), JsonRequestBehavior.AllowGet);
            }           
            return Json(new ReturnFormat(200, "success", school.MaTruong), JsonRequestBehavior.AllowGet);
        }

        [Route("hopdongduytrymau/{matruong}")]
        [HttpGet]
        public ActionResult HopDongMau(string matruong)
        {
            School school = schoolRepository.GetSchoolByMaTruong(matruong.Trim());
            if (school == null)
            {
                return RedirectToRoute("kiemtramatruong");
            }
            HopDong hopDong = hopDongRepository.GetLastestHopDongBySchoolId(school.Id);
            //hopDong.NgayHieuLucHD.Value.AddYears(1);
            
                 
            ViewBag.LastestHopDong = hopDong;
            return View(school);
        }

        [Route("thongtintruong/{matruong}")]
        public ActionResult ThongTinDonVi(string matruong)
        {
            
            School school = schoolRepository.GetSchoolByMaTruong(matruong.Trim());
            if (school == null || school.IsDaTaoMoi == false)
            {
                return RedirectToRoute("kiemtramatruong");
            }
            List<District> districts = districtRepository.GetDistrictsByProvinceId(79);
            List<Ward> wards;
            if (school.WardId == null)
            {
                wards = wardRepository.GetWardsByDistrictId(760);
            }
            wards = wardRepository.GetWardsByDistrictId(school.Ward.DistrictID);
            ThongTinTruongOneViewModel thongTinTruongOneViewModel = new ThongTinTruongOneViewModel(school, districts, wards);
            ViewBag.HopDongDaKis = hopDongRepository.GetHopDongsBySchoolId(school.Id);
            return View(thongTinTruongOneViewModel);
        }
        [Route("hopdongtaolapmau/{matruong}")]
        [HttpGet]
        public ActionResult HopDongTaoLapMau(string matruong)
        {
            School school = schoolRepository.GetSchoolByMaTruong(matruong.Trim());
            if (school == null)
            {
                return RedirectToRoute("kiemtramatruong");
            }
            return View(school);
        }
    }
}