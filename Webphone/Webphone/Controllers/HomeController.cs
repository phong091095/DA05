using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using Webphone._Models;
using Webphone.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Webphone.Extensions;


namespace Webphone.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly C4AsmContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, C4AsmContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("")]
        [Route("Home/Index/{id?}")]
        [Route("Index/{id?}")]
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }

        // Đăng ký
        [Route("Register/{id?}")]
        [Route("Home/Register/{id?}")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(string email, string username)
        {

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username))
            {
                TempData["ErrorMessage"] = "Email and username are required.";
                return RedirectToAction("Register");
            }
            C4AsmContext db = new C4AsmContext();
            // Kiểm tra email và tên người dùng đã tồn tại chưa

            if (db.LoginInfos.Any(u => u.Email == email))
            {
                TempData["ErrorMessage"] = "Email is already registered.";
                return RedirectToAction("Register");
            }

            if (db.LoginInfos.Any(u => u.UserName == username))
            {
                TempData["ErrorMessage"] = "Username is already taken.";
                return RedirectToAction("Register");
            }

            string randomPassword = RandomPasswordGenerator.Generate(8);
            // Xác thực mật khẩu
            string hashedPassword = HashPassword(randomPassword);

            // Thêm người dùng mới vào cơ sở dữ liệu
            LoginInfo newuser = new LoginInfo()
            {
                UserName = username,
                Email = email,
                Passw = hashedPassword,
                LoginRole = 1,
                LoginStatus = false
            };

            db.LoginInfos.Add(newuser);
            db.SaveChanges();

            // Gửi email thông báo về mật khẩu mới
            SendEmail(email, "Your WebPhone password", $"Your password: {randomPassword}");

            TempData["SuccessMessage"] = "Registration successful. Please login with your credentials.";

            return RedirectToAction("Login");
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("ttphong0910@gmail.com", "PhongTran");
            var toAddress = new MailAddress(toEmail);
            const string fromPassword = "xcffwnznorcsmgsy";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            smtp.Send(message);
        }
        public static class RandomPasswordGenerator
        {
            private const string AllowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";

            public static string Generate(int length)
            {
                var rng = RandomNumberGenerator.Create();
                var result = new char[length];
                var charsLength = AllowedChars.Length;

                byte[] data = new byte[4];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(data);
                    uint randomResult = BitConverter.ToUInt32(data, 0);
                    result[i] = AllowedChars[(int)(randomResult % charsLength)];
                }

                string randomPassword = new string(result);

                return randomPassword;
            }
        }
        public string HashPassword(string password)
        {
            // Tạo một đối tượng SHA256 để hash mật khẩu
            using (SHA256 sha256 = SHA256.Create())
            {
                // Chuyển đổi mật khẩu thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);

                // Tạo hash từ mật khẩu
                byte[] hashedBytes = sha256.ComputeHash(inputBytes);

                // Chuyển đổi hash thành chuỗi hexa
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                // Trả về hash đã được chuyển đổi thành chuỗi hexa
                return builder.ToString();
            }
        }
        //Đăng nhập
        [Route("Login/{id?}")]
        [Route("Home/Login/{id?}")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> checkLogin(string username, string password)
        {
            using (C4AsmContext db = new C4AsmContext())
            {
                var user = db.LoginInfos.FirstOrDefault(u => u.UserName == username);
                if (user != null && VerifyPw(password, user.Passw) && user.LoginStatus == true)
                {
                    if (user.LoginRole == 1)
                    {
                        HttpContext.Session.SetString("Username", user.UserName);
                        HttpContext.Session.SetString("UserId", user.LoginId.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.LoginRole == 2)
                    {
                        HttpContext.Session.SetString("Mnuser", user.UserName);
                        return RedirectToAction("Index", "Products");
                    }
                }
                else if (user != null && user.LoginStatus == false)
                {
                    TempData["ErrorMessage"] = "Tài khoản của bạn đã bị vô hiệu hóa.";
                    return RedirectToAction("Login");
                }
            }

            TempData["ErrorMessage"] = "Sai tên đăng nhập hoặc mật khẩu.";
            return RedirectToAction("Login");
        }



        public IActionResult ChangePassword(string username)
        {
            HttpContext.Session.SetString("CPUsername", username);
            return View();
        }
        [HttpPost]
        public IActionResult ChangePw(string newpw, string confirmpw)
{
            string username = HttpContext.Session.GetString("CPUsername");
            if (!string.IsNullOrEmpty(username))
            {
                using (C4AsmContext db = new C4AsmContext())
                {
                    var user = db.LoginInfos.FirstOrDefault(u => u.UserName == username);
                    if (user != null)
                    {
                        if (newpw.Length < 8)
                        {
                            TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất 8 ký tự.";
                            return RedirectToAction("ChangePassword", new { username = username });
                        }
                        if (newpw != confirmpw)
                        {
                            TempData["ErrorMessage"] = "Mật khẩu không trùng khớp.";
                            return RedirectToAction("ChangePassword", new { username = username });
                        }

                        if (newpw.Length >= 8 && newpw == confirmpw)
                        {
                            // Cập nhật mật khẩu mới và trạng thái đăng nhập
                            user.Passw = HashPassword(newpw);
                            user.LoginStatus = true;
                            db.SaveChanges();
                            
                            // Xóa session hiện tại
                            HttpContext.Session.Clear();
                            TempData["SuccessMessage"] = "Password changed successfully";
                            // Chuyển hướng về trang đăng nhập
                            return RedirectToAction("Login", "Home");
                        }
                    }
                }
            }
        TempData["ErrorMessage"] = "Không tìm thấy người dùng hoặc thông tin không hợp lệ.";
        return RedirectToAction("Index", "Home");
}
        [Route("ForgotPw/{id?}")]
        [Route("Home/ForgotPw/{id?}")]
        public IActionResult ForgotPw()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FgPw(string email, string username)
        {
            string randomPassword = RandomPasswordGenerator.Generate(8);
            // Xác thực mật khẩu
            string hashedPassword = HashPassword(randomPassword);
            using(C4AsmContext db = new C4AsmContext())
            {
                var user = db.LoginInfos.FirstOrDefault(u=>u.UserName == username);
                if (user != null)
                {
                    if(user.Email== email)
                    {
                        user.Passw = hashedPassword;
                        user.LoginStatus = false;
                        db.SaveChanges();
                    }
                }
            }
            SendEmail(email, "Your WebPhone password", $"Your password: {randomPassword}");

            TempData["SuccessMessage"] = "New password has been sent to email. Please log in to change a new password.";
            return RedirectToAction("Login", "Home");
        }
        private bool VerifyPw(string inputpw, string dbpw)
        {
            string hashpw = HashPassword(inputpw);
            return hashpw == dbpw;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ProDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(m => m.ProId == id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy giỏ hàng từ session
            List<Product> cart;
            if (HttpContext.Session.GetObject<List<Product>>("Cart") == null)
            {
                cart = new List<Product>();
            }
            else
            {
                cart = HttpContext.Session.GetObject<List<Product>>("Cart");
            }

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
            var existingProduct = cart.FirstOrDefault(p => p.ProId == id);
            if (existingProduct != null)
            {
                // Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên
                existingProduct.Quantity++;
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại trong giỏ hàng, thêm sản phẩm vào giỏ hàng với số lượng là 1
                product.Quantity = 1;
                cart.Add(product);
            }

            // Lưu lại giỏ hàng vào session
            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index", "Cart"); // Chuyển hướng đến trang giỏ hàng
        }
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResults = _context.Products
                .Where(p => p.ProName.Contains(searchString))
                .ToList();
                return View("SearchResults", searchResults);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<Product>>("Cart");
            if (cart != null)
            {
                var productToUpdate = cart.FirstOrDefault(p => p.ProId == productId);
                if (productToUpdate != null)
                {
                    productToUpdate.Quantity = quantity;
                    HttpContext.Session.SetObject("Cart", cart);
                }
            }

            return Ok();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
