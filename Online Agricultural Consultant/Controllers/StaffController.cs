using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Agricultural_Consultant.Models;

namespace Online_Agricultural_Consultant.Controllers
{
    public class StaffController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string AdminUserID, string Password)
        {
            if (db.tblStaffDetails.Any(a => a.EmpId == AdminUserID && a.Password == Password))
            {
                Session["Staff_ID"] = AdminUserID;
                return RedirectToAction("Dashboard");
            }
            TempData["MSG"] = "Invalid AdminID or Password !";
            return RedirectToAction("Dashboard");
        }
        #endregion

        public ActionResult staff_profile()
        {
            if (Session["Staff_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string ID = Session["Staff_ID"].ToString();
            return View(db.tblStaffDetails.Where(a => a.EmpId == ID ).FirstOrDefault());
        }

        #region ErrorPage
        public ActionResult ErrorPage()
        {
            return View();
        }
        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {
            if (Session["Staff_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
               return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
           return RedirectToAction("Multi_Login", "Home");
        }
        #endregion


        #region MyRegion
        public ActionResult view_Query()
        {
            if (Session["Staff_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
               return RedirectToAction("Multi_Login", "Home");
            }


            return View(db.tblQueryDetails.Where(a=>a.Solution==null).ToList());
        }

        public ActionResult AddSolution(int ID_, string Solution)
        {
            if (Session["Staff_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
               return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblQueryDetails.Where(a => a.QueryNo == ID_).FirstOrDefault();
            if (data != null)
            {
                data.Solution = Solution;
                data.SolvedBy = Session["Staff_ID"].ToString();
                data.SolutionDate = DateTime.Now;
                db.Entry<tblQueryDetail>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["mmm2"] = "<script>alert('Solution Provide Successfully !!')</script>";
            }
            return RedirectToAction("view_Query");
        }


        public ActionResult view_solved_Query()
        {
            if (Session["Staff_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
               return RedirectToAction("Multi_Login", "Home");
            }
            string id = Session["Staff_ID"].ToString();
            var data = db.tblQueryDetails.Where(a => a.SolvedBy == id).ToList();
            return View(data);
        }
        #endregion
    }
}