using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebBanHang.Models.Emtity;

namespace WebBanHang.Models.EmailHelper
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tmooquiz40@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("info@rainpuddleslabradoodles.com", "Mydoodles!");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                // log exception
            }
            return false;
        }

        public bool Notify(VoucherOrder voucher)
        {
            string link = "https://localhost:44310/Home/DetailOrder/" + voucher.id;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tmooquiz40@gmail.com");
            mailMessage.To.Add(new MailAddress(voucher.email));
            var body = "<p>Số xác nhận: " + voucher.id+ "</p> "+
"<p> Xin chào " + voucher.Name.Trim() + "</p> " +
"<p> Chúng tôi vui mừng thông báo cho bạn biết rằng chúng tôi đã nhận được đơn đặt hàng của bạn. " +
"<p> Khi gói hàng của bạn được vận chuyển, chúng tôi sẽ gửi cho bạn một email có số theo dõi và liên kết để bạn có thể xem chuyển động của gói hàng của mình. </p>" +
"<p> Nếu bạn có bất kỳ câu hỏi nào, hãy liên hệ với chúng tôi tại đây hoặc gọi cho chúng tôi theo 0379854894</p>" +
"<p>Chúng tôi ở đây để giúp đỡ!</p> " +

"<p> Trả hàng: Nếu bạn muốn trả lại (các) sản phẩm của mình, vui lòng xem<a href =" + link + " > Tại đây </a> hoặc liên hệ với chúng tôi";
            mailMessage.Subject = "Thông báo đơn hàng mới từ shop Karma shop";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("info@rainpuddleslabradoodles.com", "Mydoodles!");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                // log exception
            }
            return false;
        }

        public bool SenContract(Contract contract)
        {
           // string link = "https://localhost:44310/Home/DetailOrder/" + voucher.id;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tmooquiz40@gmail.com");
            mailMessage.To.Add(new MailAddress("Htuan8288@gmail.com"));
            var body = " " +
"<p> Xin chào,bạn có một thông tin liên hệ từ khách hàng "+contract.Name+"</p> " +
"<p> Thông tin tin nhắn " +
"<p> "+ contract .Message+ " </p>" +
"<p> Email khách hàng " +contract.Email+"</p>";
            mailMessage.Subject = contract.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("info@rainpuddleslabradoodles.com", "Mydoodles!");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailTwoFactorCode(string userEmail, string code)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tmooquiz40@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Two Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("info@rainpuddleslabradoodles.com", "Mydoodles!");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch
            {
                // log exception
            }
            return false;
        }
    }
}