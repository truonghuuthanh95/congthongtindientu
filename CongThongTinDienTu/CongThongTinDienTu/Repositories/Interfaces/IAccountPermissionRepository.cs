using CongThongTinDienTu.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Interfaces
{
    public interface IAccountPermissionRepository
    {
        List<Account_Permission> GetAccount_PermissionsByAccountId(int id);
    }
}