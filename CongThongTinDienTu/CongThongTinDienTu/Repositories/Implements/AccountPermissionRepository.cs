using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Implements
{
    public class AccountPermissionRepository : IAccountPermissionRepository
    {
        CongThongTinDienTuDB _db;

        public AccountPermissionRepository(CongThongTinDienTuDB db)
        {
            _db = db;
        }

        public List<Account_Permission> GetAccount_PermissionsByAccountId(int id)
        {
            List<Account_Permission> account_Permissions = _db.Account_Permission.Where(s => s.AccountId == id).ToList();
            return account_Permissions;
        }
    }
}