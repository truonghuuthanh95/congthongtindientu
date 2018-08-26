using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        CongThongTinDienTuDB _db;

        public AccountRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public Account CheckUsernamePassword(string username, string password)
        {
            Account account = _db.Accounts.Where(s => s.Username.Trim() == username.Trim() && s.Password.Trim() == password.Trim()).SingleOrDefault();
            if (account != null)
            {
                account.Username = "";
                account.Password = "";
            }
            return account;
        }
    }
}