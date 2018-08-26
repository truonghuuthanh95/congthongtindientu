using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using CongThongTinDienTu.Repositories.Interfaces;
using CongThongTinDienTu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongThongTinDienTu.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        // GET: Account
        [Route("login", Name ="login")]
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [Route("requestLogin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestLogin(string username, string password)
        {
            Account account = accountRepository.CheckUsernamePassword(username, password);
            if (account == null)
            {
                return Json(new ReturnFormat(404, "Tên truy cập hoặc mật khẩu không đúng", null));
            }
            else if (account.IsActive == false)
            {
                return Json(new ReturnFormat(403, "Tài khoản hiện đang khóa", null));
            }
            Session.Add(CommonConstant.USER_SESSION, account);
            return Json(new ReturnFormat(200, "Success", null));
        }
        [Route("logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToRoute("login");
        }
    }
}