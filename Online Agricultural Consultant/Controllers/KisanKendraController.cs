using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Online_Agricultural_Consultant.Models;
namespace Online_Agricultural_Consultant.Controllers
{
    public class KisanKendraController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult KisanLogin(string UserID, string Password)
        {
            if (db.tblKisaanKendraDetails.Any(a => a.KKId == UserID && a.Password == Password))
            {
                Session["KKId"] = UserID;
                return RedirectToAction("Dashboard");
            }
            TempData["MSG"] = "Invalid AdminID or Password !";
            return RedirectToAction("Multi_Login", "Home");
        }

        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Multi_Login", "Home");
        }
        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {
            if (Session["KKId"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }
        #endregion

        #region ViewFarmerRegistration
        public ActionResult view_farmer_registration()
        {
            if (Session["KKId"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string UserID = Session["KKId"].ToString();
            var data = db.tblKisaanKendraDetails.Where(a => a.KKId == UserID).FirstOrDefault();
            if (data != null)
            {
                return View(db.tblFarmerDetails.Where(a => a.Block == data.Block).ToList());
            }
            return RedirectToAction("Multi_Login", "Home");
        }

        public ActionResult GetfarmerAccessAccess(string FarmerID)
        {
            if (Session["KKId"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var Data = db.tblFarmerDetails.Where(a => a.FarmerId == FarmerID).FirstOrDefault();
            if (Data!=null)
            {
                Session["UserID"] = Data.FarmerId;
                return RedirectToAction("Dashboard","User");
            }
            return View("Index");
        }
        #endregion

        public ActionResult user_registration_()
        {
            if (Session["KKId"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string UserID = Session["KKId"].ToString();
            var data = db.tblKisaanKendraDetails.Where(a => a.KKId == UserID).FirstOrDefault();
            if (data != null)
            {
                Session["Block"] = data.Block;
                return RedirectToAction("user_registration","Home");
            }
            return View("Index");
        }
    }
}