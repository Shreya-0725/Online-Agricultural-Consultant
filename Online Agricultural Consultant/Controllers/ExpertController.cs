using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Agricultural_Consultant.Models;

namespace Online_Agricultural_Consultant.Controllers
{
    public class ExpertController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string AdminUserID, string Password)
        {
            if (db.tblExpertDetails.Any(a => a.ExpertId == AdminUserID && a.Password == Password))
            {
                Session["Expert_ID"] = AdminUserID;
                return RedirectToAction("Dashboard");
            }
            TempData["MSG"] = "Invalid AdminID or Password !";
            return RedirectToAction("Dashboard");
        }
        #endregion

        #region ErrorPage
        public ActionResult ErrorPage()
        {
            return View();
        }
        #endregion

        public ActionResult View_Profile()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string ID = Session["Expert_ID"].ToString();
            return View(db.tblExpertDetails.Where(a => a.ExpertId == ID).FirstOrDefault());
        }


        #region Dashboard
        public ActionResult Dashboard()
        {
            if (Session["Expert_ID"] == null)
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

        #region Articles
        public ActionResult add_articles()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult add_articles(tblArticle obj, HttpPostedFileBase File)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            obj.CreatedBy = Session["Expert_ID"].ToString();
            obj.CreationDate = DateTime.Now;
            obj.Image = "/ExpertDocuments/" + FIleUpload.UploadImage(File, "ExpertDocuments");
            db.tblArticles.Add(obj);
            db.SaveChanges();
            TempData["article_msg"] = "Article added Successfuly";
            return View();
        }

        public ActionResult view_articles()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string id = Session["Expert_ID"].ToString();
            return View(db.tblArticles.Where(a=>a.CreatedBy==id).ToList());
        }
        #endregion

        #region Solve regular Issue
        public ActionResult solve_regular_issue()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View(db.tblRegularIssues.OrderByDescending(a=>a.IssueId).ToList());
        }

        public ActionResult AddSolution(int ID_, string Solution)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblRegularIssues.Where(a => a.IssueId == ID_).FirstOrDefault();
            if (data!=null)
            {
                data.Solution = Solution;
                data.SolvedBy= Session["Expert_ID"].ToString();
                data.SolutionDate = DateTime.Now;
                data.ModificationDate = DateTime.Now;
                data.Status = "Solved";
                db.Entry<tblRegularIssue>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["mmm"] = "<script>alert('Solution Provide Successfully !!')</script>";
            }
            return RedirectToAction("solve_regular_issue");
        }
        #endregion
        #region view_visit_request
        public ActionResult view_visit_request()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var ID_ = Session["Expert_ID"].ToString();
            var data = db.tblSiteVisits.Where(a => a.AssignedAsst == ID_).ToList();
            return View(data);
        }

        public ActionResult ViewDetails_asssign_RTA(int ID)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblSiteVisits.Where(a => a.ID == ID).FirstOrDefault();
            return View(data);
        }

        public ActionResult update_RTA(tblSiteVisit obj)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblSiteVisits.Where(a => a.ID == obj.ID).FirstOrDefault();
            if (data != null)
            {
                data.VisitDate = obj.VisitDate;
                data.SolutionDate = DateTime.Now;
                data.ModificationDate = DateTime.Now;
                data.VisitIntervalInHrs = obj.VisitIntervalInHrs;
                data.ClosingDate = obj.ClosingDate;
                db.Entry<tblSiteVisit>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["MSG__"] = "Data Update Successfully !!";
            }
            return RedirectToAction("ViewDetails_asssign_RTA", new {ID=obj.ID });
        }
        #endregion


        #region View SoilTest
        public ActionResult view_soiltest_request()
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var ID_ = Session["Expert_ID"].ToString();
            var data = db.tblSoilTestings.Where(a => a.TestedBy == ID_).ToList();
            return View(data);
        }

        public ActionResult view_soiltested_History(int ID)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblSoilTestings.Where(a => a.ID == ID).FirstOrDefault();
            return View(data);
        }

        public ActionResult update_Soiltest_Data(tblSoilTesting obj)
        {
            if (Session["Expert_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblSoilTestings.Where(a => a.ID == obj.ID).FirstOrDefault();
            if (data != null)
            {
                data.Acidity = obj.Acidity;
                data.SolutionDate = DateTime.Now;
                data.PhLevel = obj.PhLevel;
                data.SoilType = obj.SoilType;
                data.Nutrient = obj.Nutrient;
                data.ClosingDate = DateTime.Now;
                data.HealthCardNo = obj.HealthCardNo;
                db.Entry<tblSoilTesting>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["MSG__"] = "Data Update Successfully !!";
            }
            return RedirectToAction("view_soiltest_request", new { ID = obj.ID });
        }
        #endregion
    }
}