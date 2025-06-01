using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WbeMovieUser.Models;

namespace WbeMovieUser.Controllers
{
    public class AuthenticationController : Controller
    {
        Model1 movie = new Model1();
        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {
            try
            {
                var existingEmail = movie.users.FirstOrDefault(x => x.email == email);
                if (existingEmail == null)
                    return Json(new { success = false, message = "Email này không tồn tại" });
                bool hashedPassword = BCrypt.Net.BCrypt.Verify(password, existingEmail.password_hash);
                if (!hashedPassword)
                    return Json(new { success = false, message = "Mật khẩu không chính xác" });
                if(existingEmail.is_active == true)
                    return Json(new { success = false, message = "Tài khoản của bạn đã bị khóa" });
                if (existingEmail.last_login == null)
                {
                    existingEmail.last_login = DateTime.Now;
                    movie.SaveChanges();
                }
                Session["user"] = existingEmail;
                return Json(new { success = true, message = "Đăng nhập thành công!", userId = existingEmail.user_id });
            }
            catch (Exception ex) 
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Register(string userName, string email, HttpPostedFileBase avatar ,string password, string changePassword)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(changePassword))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
                }

                if (password != changePassword)
                {
                    return Json(new { success = false, message = "Mật khẩu và xác nhận mật khẩu không khớp." });
                }

                var existingEmail = movie.users.FirstOrDefault(x => x.email == email);
                if (existingEmail != null) 
                {
                    return Json(new { success = false, message = "Email này đã tồn tại" });
                }
                var existingUserName = movie.users.FirstOrDefault(x => x.username == userName);
                if(existingUserName != null)
                    return Json(new { success = false, message = "Tên người dùng này đã tồn tại" });

                string avatarPath = null;
                if (avatar != null)
                {
                    string fileName = Path.GetFileName(avatar.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                    avatar.SaveAs(path);
                    avatarPath = "/Content/image/" + fileName;
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                user newUser = new user
                {
                    username = userName,
                    email = email,
                    password_hash = hashedPassword,
                    avatar_url = avatarPath,
                    created_at = DateTime.Now,
                    is_active = false,
                };
                movie.users.Add(newUser);
                movie.SaveChanges();
                return Json(new { success = true, message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "HomePage");
        }


        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                var user = movie.users.FirstOrDefault(x => x.email == Email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Email này không tồn tại." });
                }

                // Tạo mật khẩu tạm thời (mã code)
                string temporaryCode = Guid.NewGuid().ToString().Substring(0, 8); 
                user.reset_password_token = temporaryCode;
                user.reset_password_expiry = DateTime.Now.AddMinutes(30);
                movie.SaveChanges();

                // Gửi email với mã code
                SendResetPasswordEmail(user.email, temporaryCode);

                return Json(new { success = true, message = "Mã khôi phục đã được gửi qua email. Vui lòng kiểm tra email của bạn." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


        [HttpPost]
        public ActionResult ResetPassword(string email, string code, string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
                }

                var user = movie.users.FirstOrDefault(x => x.email == email);
                if (user == null)
                {
                    return Json(new { success = false, message = "Email này không tồn tại." });
                }

                // Kiểm tra mã code
                if (user.reset_password_token != code)
                {
                    return Json(new { success = false, message = "Mã khôi phục không chính xác." });
                }

                // Kiểm tra thời gian hết hạn
                if (user.reset_password_expiry < DateTime.Now)
                {
                    return Json(new { success = false, message = "Mã khôi phục đã hết hạn. Vui lòng yêu cầu mã mới." });
                }

                // Kiểm tra mật khẩu mới và xác nhận mật khẩu
                if (newPassword != confirmPassword)
                {
                    return Json(new { success = false, message = "Mật khẩu mới và xác nhận mật khẩu không khớp." });
                }

                // Cập nhật mật khẩu mới
                user.password_hash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.reset_password_token = null; // Xóa mã sau khi sử dụng
                user.reset_password_expiry = null;
                movie.SaveChanges();

                return Json(new { success = true, message = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập bằng mật khẩu mới." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


        private void SendResetPasswordEmail(string email, string code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lephuocbinh.2000@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Yêu cầu đặt lại mật khẩu";
                mail.Body = $"Chào bạn,\n\nMã khôi phục mật khẩu của bạn là: {code}\nMã này có hiệu lực trong 30 phút.\n\nTrân trọng,\nWbeMovieUser";

                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("lephuocbinh.2000@gmail.com", "yhgp ditr uzfx xqpn\r\n");
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể gửi email: " + ex.Message);
            }
        }
    }
}