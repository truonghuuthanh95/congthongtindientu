using GuiMailCongThongTin.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuiMailCongThongTin
{
    class Program
    {
        static void Main(string[] args)
        {
            ConThongTinDB _db = new ConThongTinDB();
            //lay danh sach hop dong con 1 thang het han
            List<HopDong> hopDongsHetHan1thang = _db.HopDongs.Include("School")
                .Where(s => s.NgayHieuLucHD.Value.Month - DateTime.Now.Month == 1 && DateTime.Now.Year - s.NgayHieuLucHD.Value.Year == 1)
                .ToList();
            //lay danh sach hop dong da ki nam nay
            List<HopDong> hopDongsDaKiNamNay = _db.HopDongs
                .Where(s => s.NgayHieuLucHD.Value.Year == DateTime.Now.Year).ToList();
            
            //lay danh sach hop dong con 1 thang het han
            List<HopDong> hopDongsHetHan2thang = _db.HopDongs.Include("School")
                .Where(s => s.NgayHieuLucHD.Value.Month - DateTime.Now.Month == 2 && DateTime.Now.Year - s.NgayHieuLucHD.Value.Year == 1)
                .ToList();

            //lay cac hop dong chua ki nam nay de gui mail lan 1
            List<HopDong> hopDongsCanGuiMailLan1 = hopDongsHetHan2thang.Where(s => !hopDongsDaKiNamNay.Any(s2 => s2.SchoolId == s.SchoolId)).ToList();
            //lay cac hop dong chua ki nam nay de gui mail lan 2
            List<HopDong> hopDongsCanGuiMailLan2 = hopDongsHetHan1thang.Where(s => !hopDongsDaKiNamNay.Any(s2 => s2.SchoolId == s.SchoolId)).ToList();

            //lay danh sach hop dong het han
            List<HopDong> hopDongshethan = _db.HopDongs.Include("School").Where(s => s.NgayHieuLucHD.Value.Month - DateTime.Now.Month == -1 && DateTime.Now.Year - s.NgayHieuLucHD.Value.Year == 1).ToList();
            // lay danh sach het han hop dong de gui mail
            List<HopDong> hopDongshethanGuimail = hopDongshethan.Where(s => !hopDongsDaKiNamNay.Any(s2 => s2.SchoolId == s.SchoolId)).ToList();
            //gui mail sap het han lan 1
            foreach (var item in hopDongsCanGuiMailLan1)
            {
                LogEmail logEmail = new LogEmail();
                logEmail.NoidungGui = "Gửi mail báo sắp hết hạn hợp đồng lân 1";
                logEmail.SchoolId = item.SchoolId;
                try
                {
                    SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                    var mail = new MailMessage();
                    mail.From = new MailAddress("hotanminh@hcm.edu.vn");
                    mail.To.Add("truonghuuthanh95@gmail.com");
                    mail.Subject = "Test Task schedule";
                    mail.IsBodyHtml = true;
                    string htmlBody = "<p style='font-size: 14px;'>Kính gửi Quý đơn vị,</p> <br/> " +
                        "<p style='font-size: 14px;'><b>Trung tâm Thông tin và Chương trình Giáo dục – Sở GD&ĐT TP.HCM</b> xin thông báo đến Quý đơn vị <b>" + item.School.TenTruong + "</b> với mã trường <b>" + item.School.MaTruong + "</b>. Trang thông tin điện tử(website) của Quý đơn vị sẽ hết hạn hợp đồng vào ngày <b style='color: red; font-size: 15px;'>" +
                        +item.NgayHieuLucHD.Value.AddYears(1).Day + "/" + item.NgayHieuLucHD.Value.AddYears(1).Month + "/" + item.NgayHieuLucHD.Value.AddYears(1).Year + "</b>. Để tiếp tục sử dụng website, Quý đơn vị vui lòng thực hiện đầy đủ các bước gia hạn hợp đồng tại địa chỉ sau: <br/>" +
                        "1. Truy cập vào website <a href='http://hopdpngcttdt.hcm.edu.vn/'>http://hopdpngcttdt.hcm.edu.vn/</a></br>" +
                        "2. Điền mã trường sau đó xác nhận thông tin trường <br />" +
                        "3. In hồ sơ <br />" +
                        "<i>Lưu ý: In hồ sơ thành 2 bản, đơn vị thủ trường kí, sau đó mang hợp đồng tới 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2</i></p>" +
                        "<br/>" +
                        "<p style='font-size: 14px;'>Trong quá trình thực hiện, nếu đơn vị có vấn đề cần trao đổi, vui lòng liên hệ:</p>" +
                        "<p  style='text-align: center; font-size: 14px;'>SỞ GIÁO DỤC VÀ ĐÀO TẠO <br/>" +
                        "TRUNG TÂM THÔNG TIN VÀ CHƯƠNG TRÌNH GIÁO DỤC<br/>" +
                        "Địa chỉ: 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2 <br/>" +
                        "SĐT: (028) 38 291 875 – (A. Thông).</p><br/>" +
                        "<p style='font-size: 14px;'>Trân trọng thông báo.</p>";
                    mail.Body = htmlBody;
                    smtpClient.Port = 587;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential("cttdtsgd@hcm.edu.vn".Trim(), "asdf1324!#@".Trim());
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }
                catch (Exception)
                {

                    logEmail.IsSuccessful = false;
                    _db.SaveChanges();
                    continue;
                }
                logEmail.IsSuccessful = true;
                _db.LogEmails.Add(logEmail);
                _db.SaveChanges();


            }
            //gui mail sap het han lan 2
            foreach (var item in hopDongsCanGuiMailLan2)
            {
                LogEmail logEmail = new LogEmail();
                logEmail.NoidungGui = "Gửi mail báo sắp hết hạn hợp đồng lân 2";
                logEmail.SchoolId = item.SchoolId;
                try
                {
                    SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                    var mail = new MailMessage();
                    mail.From = new MailAddress("hotanminh@hcm.edu.vn");
                    mail.To.Add("truonghuuthanh95@gmail.com");
                    mail.Subject = "Test Task schedule";
                    mail.IsBodyHtml = true;
                    string htmlBody = "<p>Kính gửi Quý đơn vị,</p> <br/> " +
                        "<p style='font-size: 14px;'><b>Trung tâm Thông tin và Chương trình Giáo dục – Sở GD&ĐT TP.HCM</b> xin thông báo đến Quý đơn vị <b>" + item.School.TenTruong + "</b> với mã trường <b>" + item.School.MaTruong + "</b>. Trang thông tin điện tử(website) của Quý đơn vị sẽ hết hạn hợp đồng vào ngày <b style='color: red; font-size: 15px;'>" +
                        +item.NgayHieuLucHD.Value.AddYears(1).Day + "/" + item.NgayHieuLucHD.Value.AddYears(1).Month + "/" + item.NgayHieuLucHD.Value.AddYears(1).Year + "</b>. Để tiếp tục sử dụng website, Quý đơn vị vui lòng thực hiện đầy đủ các bước gia hạn hợp đồng tại địa chỉ sau: <br/>" +
                        "1. Truy cập vào website <a href='http://conthongtin.hcm.edu.vn/'>http://conthongtin.hcm.edu.vn/</a><br />" +
                        "2. Điền mã trường sau đó xác nhận thông tin trường <br />" +
                        "3. In hợp đồng <br />" +
                        "<i>Lưu ý: In hợp đồng thành 2 bản, đơn vị thủ trưởng kí, sau đó mang hợp đồng tới 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2</i></p>" +
                        "<br/>" +
                        "<p style='font-size: 14px;'>Trong quá trình thực hiện, nếu đơn vị có vấn đề cần trao đổi, vui lòng liên hệ:</p>" +
                        "<p  style='text-align: center; font-size: 14px;'>SỞ GIÁO DỤC VÀ ĐÀO TẠO <br/>" +
                        "TRUNG TÂM THÔNG TIN VÀ CHƯƠNG TRÌNH GIÁO DỤC<br/>" +
                        "Địa chỉ: 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2 <br/>" +
                        "SĐT: (028) 38 291 875 – (A. Thông).</p><br/>" +
                        "<p style='font-size: 14px;'>Trân trọng thông báo.</p>";
                    mail.Body = htmlBody;
                    smtpClient.Port = 587;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential("cttdtsgd@hcm.edu.vn".Trim(), "asdf1324!#@".Trim());
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }
                catch (Exception)
                {

                    logEmail.IsSuccessful = false;
                    _db.SaveChanges();
                    continue;
                }
                logEmail.IsSuccessful = true;
                _db.LogEmails.Add(logEmail);
                _db.SaveChanges();
            }
            //gui mail bao het han
            foreach (var item in hopDongshethanGuimail)
            {
                LogEmail logEmail = new LogEmail();
                logEmail.NoidungGui = "Gửi mail báo hết hạn hợp đồng";
                logEmail.SchoolId = item.SchoolId;
                try
                {
                    SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                    var mail = new MailMessage();
                    mail.From = new MailAddress("hotanminh@hcm.edu.vn");
                    mail.To.Add("truonghuuthanh95@gmail.com");
                    mail.Subject = "Test Task schedule";
                    mail.IsBodyHtml = true;
                    string htmlBody = "<p>Kính gửi Quý đơn vị,</p> <br/> " +
                        "<p style='font-size: 14px;'><b>Trung tâm Thông tin và Chương trình Giáo dục – Sở GD&ĐT TP.HCM</b> xin thông báo đến Quý đơn vị <b>" + item.School.TenTruong + "</b> với mã trường <b>" + item.School.MaTruong + "</b>. Trang thông tin điện tử(website) của Quý đơn vị đã hết hạn hợp đồng vào ngày <b style='color: red; font-size: 15px;'>" +
                        +item.NgayHieuLucHD.Value.AddYears(1).Day + "/" + item.NgayHieuLucHD.Value.AddYears(1).Month + "/" + item.NgayHieuLucHD.Value.AddYears(1).Year + "</b>. Để tiếp tục sử dụng website, Quý đơn vị vui lòng thực hiện đầy đủ các bước gia hạn hợp đồng tại địa chỉ sau: <br/>" +
                        "1. Truy cập vào website <a href='http://conthongtin.hcm.edu.vn/'>http://conthongtin.hcm.edu.vn/</a><br />" +
                        "2. Điền mã trường sau đó xác nhận thông tin trường <br />" +
                        "3. In hợp đồng <br />" +
                        "<i>Lưu ý: In hợp đồng thành 2 bản, đơn vị thủ trưởng kí, sau đó mang hợp đồng tới 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2</i></p>" +
                        "<br/>" +
                        "<p style='font-size: 14px;'>Trong quá trình thực hiện, nếu đơn vị có vấn đề cần trao đổi, vui lòng liên hệ:</p>" +
                        "<p  style='text-align: center; font-size: 14px;'>SỞ GIÁO DỤC VÀ ĐÀO TẠO <br/>" +
                        "TRUNG TÂM THÔNG TIN VÀ CHƯƠNG TRÌNH GIÁO DỤC<br/>" +
                        "Địa chỉ: 66 – 68 Lê Thánh Tôn, Phường Bến Nghé, Quận 1, TP.HCM – Phòng 10.2 <br/>" +
                        "SĐT: (028) 38 291 875 – (A. Thông).</p><br/>" +
                        "<p style='font-size: 14px;'>Trân trọng thông báo.</p>";
                    mail.Body = htmlBody;
                    smtpClient.Port = 587;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential("cttdtsgd@hcm.edu.vn".Trim(), "asdf1324!#@".Trim());
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mail);
                }
                catch (Exception)
                {

                    logEmail.IsSuccessful = false;
                    _db.SaveChanges();
                    continue;
                }
                logEmail.IsSuccessful = true;
                _db.LogEmails.Add(logEmail);
                _db.SaveChanges();
            }
        
    }
        
    }
}
