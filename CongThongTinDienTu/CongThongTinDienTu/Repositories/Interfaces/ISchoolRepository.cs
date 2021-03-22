using CongThongTinDienTu.Models.DAO;
using CongThongTinDienTu.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CongThongTinDienTu.Repositories.Interfaces
{
    public interface ISchoolRepository
    {
        School GetSchoolByMaTruong(string matruong);
        School UpdateSchoolInfomation(SchoolDTO schoolDTO, int id);
        School GetSchoolById(int? id);
        List<School> GetSchoolsByDistrictAndCapHoc(int districtId , int caphoc);
        List<School> GetSchoolsByDistrictAndCapHocWithIsDaTaoLapFalse(int districtId, int caphoc);
        School GetSchoolByQIMaTruong(string qIMaTruong);
        List<School> GetSchoolsByDistrictAndCapHocWithLessDetail(int districtId, int caphoc);
    }
}