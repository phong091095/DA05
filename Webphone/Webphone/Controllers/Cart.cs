using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Webphone._Models;
using Webphone.Extensions;

namespace Webphone.Controllers
{
    public class Cart : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly C4AsmContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Cart(ILogger<HomeController> logger, C4AsmContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateCart([FromBody] List<Product> updatedCart)
        {
            HttpContext.Session.SetObject("Cart", updatedCart);
            return Ok();
        }
        public IActionResult Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy giỏ hàng từ session
            List<Product> cart = HttpContext.Session.GetObject<List<Product>>("Cart");
            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }


            var productToRemove = cart.FirstOrDefault(p => p.ProId == id);
            if (productToRemove != null)
            {

                cart.Remove(productToRemove);

                HttpContext.Session.SetObject("Cart", cart);
            }


            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public IActionResult Checkout(string name, string address, string phone, decimal totalAmount, string email, string loginid)
        {
            List<Product> cart = HttpContext.Session.GetObject<List<Product>>("Cart");
            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                DateOnly invoiceDate = DateOnly.FromDateTime(currentDate);


                Invoice newin = new Invoice
                {
                    InvoiceDate = invoiceDate,
                    TotalAmount = totalAmount,
                    InvoiceStatus = "Đã thanh toán",
                    CustName = name,
                    CustAdd = address,
                    CustPhone = phone,
                };
                _context.Invoices.Add(newin);
                _context.SaveChanges();
                int InvoiceId = newin.InvoiceId;
                foreach (var item in cart)
                {
                    InvoiceDetail newindetail = new InvoiceDetail
                    {
                        InvoiceId = InvoiceId,
                        ProductId = item.ProId,
                        Quantity = item.Quantity,
                        Subtotal = item.Quantity * item.ProPrice
                    };
                    _context.InvoiceDetails.Add(newindetail);
                }
                _context.SaveChanges();
                if (loginid != null)
                {
                    Customer newcust = new Customer
                    {
                        CustName = name,
                        CustAd = address,
                        CustPhone = phone,
                        CustMail = email,
                        InvoiceId = InvoiceId,
                        LoginId = int.Parse(loginid)
                    };
                    _context.Customers.Add(newcust);
                    _context.SaveChanges();
                }
                else
                {
                    Customer newcust = new Customer
                    {
                        CustName = name,
                        CustAd = address,
                        CustPhone = phone,
                        CustMail = email,
                        InvoiceId = InvoiceId,
                        LoginId = null
                    };
                    _context.Customers.Add(newcust);
                    _context.SaveChanges();
                }
                TempData["CartSucces"] = "The order has been saved";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
