using DatabaseLayer;
using MangaCentralLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangaCentralLibrary.Controllers
{
    public class PurchaseController : Controller
    {
        // GET: Purchase
        private MangaCentralLibraryEntities1 db = new MangaCentralLibraryEntities1();
        public ActionResult NewPurchase()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var temppur = db.PurTemDetailsTables.ToList();
            return View(temppur);

        }

        [HttpPost]
        public ActionResult AddItem(int BID, int Qty, float Price)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var find = db.PurTemDetailsTables.Where(i => i.BookID == BID).FirstOrDefault();
            if (find == null)
            {
                if (BID > 0 && Qty > 0 && Price > 0)
                {
                    var newitem = new PurTemDetailsTable()
                    {
                        BookID = BID,
                        Qty = Qty,
                        UnitPrice = Price
                    };
                    db.PurTemDetailsTables.Add(newitem);
                    db.SaveChanges();
                    ViewBag.Message = "Book Added Successfully";
                }
            }
            else
            {
                ViewBag.Message = "Already Exist! Plz Check.";
            }
            return RedirectToAction("NewPurchase");
        }
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var book = db.PurTemDetailsTables.Find(id);
            if (book != null)
            {
                db.Entry(book).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                ViewBag.Message = "Deleted Successfully";
                return RedirectToAction("NewPurchase");
            }
            ViewBag.Message = "Some Unexpected Error Occured";
            return View("NewPurchase");

        }
        [HttpGet]
        public ActionResult GetBooks()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<BookMV> list = new List<BookMV>();
            var booklist = db.BookTables.ToList();
            foreach (var item in booklist)
            {
                list.Add(new BookMV { BookName = item.BookName, BookID = item.BookID });
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

    }
}
