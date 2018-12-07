using CongThongTinDienTu.Models.DAO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CongThongTinDienTu.Utils
{
    public class ExportExcel
    {
        public static Task GenerateXLSHopDong(List<HopDong> hopdong, string filepath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    //Create the worksheet 
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("hopdongcttdt");
                    ws.Cells[2, 1].Value = "DANH SÁCH HỢP ĐỒNG CỔNG THÔNG TIN ĐIỆN TỬ";
                    ws.Cells["A2:N2"].Merge = true;
                    ws.Cells[6, 1].Value = "STT";
                    ws.Cells[6, 2].Value = "SỐ HỢP ĐỒNG";
                    ws.Cells[6, 3].Value = "TÊN TRƯỜNG";
                    ws.Cells[6, 4].Value = "CẤP";
                    ws.Cells[6, 5].Value = "QUẬN/HUYỆN";
                    ws.Cells[6, 6].Value = "DVQL";
                    ws.Cells[6, 7].Value = "NGÀY KÍ";
                    ws.Cells[6, 8].Value = "NGÀY HL";
                    ws.Cells[6, 9].Value = "HTTT";
                    ws.Cells[6, 10].Value = "LOẠI HĐ";
                    ws.Cells[6, 11].Value = "SỐ NĂM";
                    ws.Cells[6, 12].Value = "SỐ TIỀN";
                    ws.Cells[6, 13].Value = "NGÀY ĐÓNG";
                    ws.Cells[6, 14].Value = "MÃ TRƯỜNG";
                    for (int i = 0; i < hopdong.Count(); i++)
                    {
                        ws.Cells[i + 7, 1].Value = i + 1;
                        ws.Cells[i + 7, 2].Value = hopdong.ElementAt(i).MaHopDong;
                        ws.Cells[i + 7, 3].Value = hopdong.ElementAt(i).School.TenTruong;
                        ws.Cells[i + 7, 4].Value = hopdong.ElementAt(i).School.CapTruong.TenVietTat;
                        ws.Cells[i + 7, 5].Value = hopdong.ElementAt(i).School.Ward.District.Type + ' ' + hopdong.ElementAt(i).School.Ward.District.Name;
                        ws.Cells[i + 7, 6].Value = hopdong.ElementAt(i).School.DVQL.Ten;
                        ws.Cells[i + 7, 7].Value = hopdong.ElementAt(i).NgayKiHD.Value.ToString("dd/MM/yyyy");
                        ws.Cells[i + 7, 8].Value = hopdong.ElementAt(i).NgayHieuLucHD.Value.ToString("dd/MM/yyyy");
                        ws.Cells[i + 7, 9].Value = hopdong.ElementAt(i).IsThanhToanBangTienMat == true? "TM" : "CK";
                        ws.Cells[i + 7, 10].Value = hopdong.ElementAt(i).IsTaoMoi == true? "Tạo mới" : "Duy trì";
                        ws.Cells[i + 7, 11].Value = hopdong.ElementAt(i).SoNam;
                        ws.Cells[i + 7, 12].Value = hopdong.ElementAt(i).SoTien;
                        ws.Cells[i + 7, 13].Value = hopdong.ElementAt(i).NgayThanhToan == null? "" : hopdong.ElementAt(i).NgayThanhToan.Value.ToString("dd/MM/yyyy");
                        ws.Cells[i + 7, 14].Value = hopdong.ElementAt(i).School.MaTruong;
                    }
                    using (ExcelRange rng = ws.Cells["A2:N2"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Size = 18;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Font.Color.SetColor(Color.Red);
                    }
                    using (ExcelRange rng = ws.Cells["A3:N3"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Size = 14;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Font.Color.SetColor(Color.Red);
                    }
                    using (ExcelRange rng = ws.Cells["A4:N4"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Size = 14;
                        rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rng.Style.Font.Color.SetColor(Color.Red);
                    }
                    using (ExcelRange rng = ws.Cells["A6:N6"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;        //Set Pattern for the background to Solid 
                        rng.Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);  //Set color to blue 
                        rng.Style.Font.Color.SetColor(Color.Black);
                        rng.AutoFitColumns();
                    }
                    pck.SaveAs(new FileInfo(filepath));
                }
            });
        }
    }
}