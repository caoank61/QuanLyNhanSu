using QuanLyNhanSu.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace QuanLyNhanSu.Controllers
{
    public class TrinhDoHocVanController : Controller
    {
        private Data db = new Data();

        // GET: TrinhDoHocVan
        public ActionResult Index()
        {
            if (Session["ID_TKadmin"] != null)
            {
                return View(db.TrinhDoHocVans.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: TrinhDoHocVan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrinhDoHocVan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TrinhDo")] TrinhDoHocVan trinhDoHocVan)
        {
            if (ModelState.IsValid)
            {
                db.TrinhDoHocVans.Add(trinhDoHocVan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // POST: TrinhDoHocVan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TrinhDo")] TrinhDoHocVan trinhDoHocVan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trinhDoHocVan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trinhDoHocVan);
        }

        // GET: TrinhDoHocVan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            if (trinhDoHocVan == null)
            {
                return HttpNotFound();
            }
            return View(trinhDoHocVan);
        }

        // POST: TrinhDoHocVan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrinhDoHocVan trinhDoHocVan = db.TrinhDoHocVans.Find(id);
            var listnv = db.NhanViens.Where(g => g.IdPB == id).ToList();
            foreach (var item in listnv)
            {
                NhanVien nv = db.NhanViens.Find(item.Id);
                db.NhanViens.Remove(nv);
            }
            db.TrinhDoHocVans.Remove(trinhDoHocVan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
