using QuanLyNhanSu.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyNhanSu.Controllers
{
    public class NhanVienController : Controller
    {
        private Data db = new Data();

        // GET: NhanVien
        public ActionResult Index(int xoa = 0)
        {
            ViewBag.xoa = xoa;
            var nhanViens = db.NhanViens.Include(n => n.PhongBan).Include(n => n.TrinhDoHocVan).Include(n => n.TaiKhoan);
            if (xoa == 0)
            {
                nhanViens = nhanViens.Where(g => g.TaiKhoan.TinhTrang == true);
            }
            else
            {
                nhanViens = nhanViens.Where(g => g.TaiKhoan.TinhTrang == false);
            }
            if (Session["ID_TKadmin"] != null)
            {
                return View(nhanViens.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: NhanVien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanVien/Create
        public ActionResult Create()
        {
            ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan");
            ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo");
            ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
            ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HoTen,Email,SDT,GioiTinh,NgaySinh,QueQuan,DanToc,BacLuong,ChuyenNganh,IdTDHV,IdPB,IdTK,Luong,STK,NganHang")] NhanVien nhanVien)
        {
            try
            {
                var email = db.NhanViens.FirstOrDefault(g => g.Email.Contains(nhanVien.Email));
                if (email != null)
                {
                    ViewBag.email = "Email bị trùng yêu cầu nhập lại";
                    ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan");
                    ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo");
                    ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
                    ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
                    return View(nhanVien);
                }
                if (nhanVien.SDT.Length != 10)
                {
                    ViewBag.SDT = "Số điện thoại sai định dạng";
                }
                var sdt = db.NhanViens.FirstOrDefault(g => g.SDT.Contains(nhanVien.SDT));
                if (sdt != null)
                {
                    ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan");
                    ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo");
                    ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
                    ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
                    ViewBag.SDT = "Số điện thoại bị trùng yêu cầu nhập lại";
                    return View(nhanVien);
                }
                TaiKhoan tk = new TaiKhoan();
                tk.TenDangNhap = nhanVien.Email;
                tk.MatKhau = "quanlynhansu123@";
                tk.TinhTrang = true;
                tk.IdCV = Convert.ToInt32(Request["IdCV"]);
                nhanVien.GioiTinh = Request["GioiTinh"] == "0" ? true : false;
                db.TaiKhoans.Add(tk);
                nhanVien.IdTK = tk.Id;
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan", nhanVien.IdPB);
                ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo", nhanVien.IdTDHV);
                ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
                ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap", nhanVien.IdTK);
                return View(nhanVien);
            }


        }

        // GET: NhanVien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPB = db.PhongBans.AsQueryable().ToList();
            ViewBag.IdTDHV = db.TrinhDoHocVans.AsQueryable().ToList();
            ViewBag.IdCV = db.ChucVus.AsQueryable().ToList();
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HoTen,Email,SDT,GioiTinh,NgaySinh,QueQuan,DanToc,BacLuong,ChuyenNganh,IdTDHV,IdPB,IdTK,Luong,STK,NganHang")] NhanVien nhanVien)
        {
            try
            {
                TaiKhoan tk = db.TaiKhoans.Find(nhanVien.IdTK);
                tk.IdCV = Convert.ToInt32(Request["IdCV"]);
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan");
                ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo");
                ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
                ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
                return View(nhanVien);
            }

        }
        public ActionResult HopDong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Id = id;
            HopDong nhanVien = db.HopDongs.FirstOrDefault(g => g.IdNV == id);
            return View(nhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HopDong([Bind(Include = "Id,IdNV,LoaiHD,NgayKi,NgayKT")] HopDong hopDong)
        {
            try
            {
                if (hopDong.Id == null)
                {
                    var idtk =
                    db.HopDongs.Add(hopDong);
                }
                else
                {
                    db.HopDongs.AddOrUpdate(hopDong);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(hopDong);
            }

        }


        // GET: NhanVien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            TaiKhoan tk = db.TaiKhoans.Find(nhanVien.IdTK);
            tk.TinhTrang = false;
            db.TaiKhoans.AddOrUpdate(tk);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Reset(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan tk = db.TaiKhoans.Find(id);
            tk.TinhTrang = true;
            db.TaiKhoans.AddOrUpdate(tk);
            db.SaveChanges();
            return RedirectToAction("Index", "NhanVien", new { xoa = 1 });
        }
        public ActionResult DeleteEnd(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            TaiKhoan tk = db.TaiKhoans.Find(nhanVien.IdTK);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            db.NhanViens.Remove(nhanVien);
            db.TaiKhoans.Remove(tk);
            db.SaveChanges();
            return RedirectToAction("Index", "NhanVien", new { xoa = 1 });
        }
        public ActionResult EditTT(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPB = db.PhongBans.AsQueryable().ToList();
            ViewBag.IdTDHV = db.TrinhDoHocVans.AsQueryable().ToList();
            ViewBag.IdCV = db.ChucVus.AsQueryable().ToList();
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTT([Bind(Include = "Id,HoTen,Email,SDT,GioiTinh,NgaySinh,QueQuan,DanToc,BacLuong,ChuyenNganh,IdTDHV,IdPB,IdTK,Luong,STK,NganHang")] NhanVien nhanVien)
        {
            try
            {
                nhanVien.GioiTinh = Request["GioiTinh"] == "0" ? true : false;
                nhanVien.NgaySinh = Convert.ToDateTime(Request["NgaySinh"]);
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                Session["HoTenadmin"] = nhanVien.HoTen;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.IdPB = new SelectList(db.PhongBans, "Id", "TenPhongBan");
                ViewBag.IdTDHV = new SelectList(db.TrinhDoHocVans, "Id", "TrinhDo");
                ViewBag.IdTK = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
                ViewBag.IdCV = new SelectList(db.ChucVus, "Id", "TenCV");
                return View(nhanVien);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
