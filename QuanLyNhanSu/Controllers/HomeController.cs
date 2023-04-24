using QuanLyNhanSu.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyNhanSu.Controllers
{
    public class HomeController : Controller
    {
        Data db = new Data();
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] != null)
            {
                ViewBag.NV = db.NhanViens.AsQueryable().ToList().Count();
                ViewBag.PB = db.PhongBans.AsQueryable().ToList().Count();
                //ViewBag.NV = db.ThongTins.AsQueryable().Where(g => g.IdCV != 4).ToList().Count();
                //ViewBag.KH = db.ThongTins.AsQueryable().Where(g => g.IdCV == 4).ToList().Count();
                //ViewBag.Tin = db.TinTucs.AsQueryable().ToList().Count();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        //đăng nhập, đăng xuất
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            if (ModelState.IsValid)
            {
                var user1 = db.TaiKhoans.FirstOrDefault(u => u.TenDangNhap.Equals(TaiKhoan) && u.MatKhau.Equals(MatKhau));
                if (user1 != null)
                {
                    var newCookie = new HttpCookie("myCookieadmin", user1.Id.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Response.AppendCookie(newCookie);
                    if (user1.IdCV != 0)
                    {
                        var nv = db.NhanViens.FirstOrDefault(g => g.IdTK == user1.Id);
                        Session["HoTenadmin"] = nv.HoTen;
                        Session["ID_TKadmin"] = user1.Id;
                        Session["PQadmin"] = user1.IdCV;
                        Session["Phong"] = db.PhongBans.FirstOrDefault(g => g.TenPhongBan.Contains("nhân sự")).Id;
                        Session["PB"] = db.NhanViens.FirstOrDefault(g => g.IdTK == user1.Id).IdPB;

                        ViewBag.PQ = user1.IdCV;
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        ViewBag.error = "Thông tin đăng nhập không hợp lệ!!!";
                    }
                }

                else
                {
                    ViewBag.error = "Thông tin đăng nhập không hợp lệ!!!";
                }

            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            if (Request.Cookies["myCookieadmin"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookieadmin"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return RedirectToAction("Login", "Home");
        }
    }
}