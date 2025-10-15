using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Agricultural_Consultant.Models;

namespace Online_Agricultural_Consultant.Controllers
{
    public class UserController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        Random rand = new Random();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(string userid, string password)
        {
            TempData["userloginmsg"] = "invalid Id or Password !!";
            if (db.tblFarmerDetails.Any(a => a.FarmerId == userid && a.Password == password))
            {
                Session["UserID"] = userid;
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        public ActionResult view_Profile(string ID)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string id = Session["UserID"].ToString();
            return View(db.tblFarmerDetails.Where(a => a.FarmerId == ID).FirstOrDefault());
        }


        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }

        #region MyRegion
        public ActionResult Regular_Issue()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Regular_Issue(tblRegularIssue obj)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            obj.CreateDate = DateTime.Now;
            obj.FarmerID = Session["UserID"].ToString();
            obj.Status = "pending";
            db.tblRegularIssues.Add(obj);
            db.SaveChanges();
            TempData["msg_issue"] = "Issue Submit success fully !!";
            return View();
        }

        public ActionResult view_regular_issue_history()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View(db.tblRegularIssues.OrderByDescending(a => a.IssueId).ToList());
        }

        public ActionResult give_feedback(int ID_, string Feedback)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            var data = db.tblRegularIssues.Where(a => a.IssueId == ID_).FirstOrDefault();
            if (data != null)
            {
                data.FeedBack = Feedback;
                db.Entry<tblRegularIssue>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["mmm1"] = "<script>alert('Feedback Added Successfully !!')</script>";
            }
            return RedirectToAction("view_regular_issue_history");
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Multi_Login", "Home");
        }
        #endregion


        #region Query
        public ActionResult Query()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            return View();
        }
        [HttpPost]
        public ActionResult Query(tblQueryDetail obj)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            obj.FarmerId = Session["UserID"].ToString();
            obj.CreationDate = DateTime.Now;
            db.tblQueryDetails.Add(obj);
            db.SaveChanges();
            TempData["MSGQuery"] = "Your Query Is Submitted, We Will Inform you Soon! :-) ";
            return View();
        }

        public ActionResult view_QueryHistory()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View(db.tblQueryDetails.OrderByDescending(a => a.QueryNo).ToList());
        }
        #endregion

        #region applyrealtimeAssistent
        public ActionResult applyrealtimeAssistent()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult applyrealtimeAssistent(tblSiteVisit obj)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            obj.FarmerId = Session["UserID"].ToString();
            obj.CreationDate = DateTime.Now;
            obj.ApplicationId = "APPL" + rand.Next(9999, 1000000);
            db.tblSiteVisits.Add(obj);
            db.SaveChanges();
            TempData["sv_msg"] = "Request has been submit.";
            return View();
        }

        public ActionResult view_applyrealtimeAssistent()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string id = Session["UserID"].ToString();
            return View(db.tblSiteVisits.Where(a => a.FarmerId == id).OrderByDescending(a => a.ID).ToList());
        }
        public ActionResult view_RTA_details(int ID)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View(db.tblSiteVisits.Where(a => a.ID == ID).FirstOrDefault());
        }

        public ActionResult give_feedback_RTA(int ID_, string Feedback)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }

            var data = db.tblSiteVisits.Where(a => a.ID == ID_).FirstOrDefault();
            if (data != null)
            {
                data.Feedback = Feedback;
                data.ModificationDate = DateTime.Now;
                db.Entry<tblSiteVisit>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            TempData["MSG__"] = "Feedback Added!!";
            return RedirectToAction("view_RTA_details", new { ID = ID_ });
        }

        #endregion

        #region apply_soil_test
        public ActionResult apply_soil_test()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string FID = Session["UserID"].ToString();
            var Data = db.tblFarmerDetails.Where(a => a.FarmerId == FID).FirstOrDefault();
            return View(Data);
        }
        [HttpPost]
        public ActionResult apply_soil_test(tblFarmerDetail objj)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            tblSoilTesting obj = new tblSoilTesting
            {
                FarmerId = Session["UserID"].ToString(),
                ApplicationDate = DateTime.Now,
                TestId = "TEST"+rand.Next(9999,100000),

            };
            db.tblSoilTestings.Add(obj);
            db.SaveChanges();
            TempData["sv_msg1"] = "Request has been submit.";
            return RedirectToAction("apply_soil_test");
        }

        public ActionResult view_soiltest_history()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string FID = Session["UserID"].ToString();
            return View(db.tblSoilTestings.Where(a=>a.FarmerId==FID).OrderByDescending(a=>a.ID));
        }
        #endregion

        public ActionResult ViewStatus()
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewStatus(string StatusType, string AppID)
        {
            if (Session["UserID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Multi_Login", "Home");
            }
            string ID = Session["UserID"].ToString();
            string Meassge = "";
            if (StatusType=="SiteVisit")
            {
                var data = db.tblSiteVisits.Where(a => a.ApplicationId == AppID).FirstOrDefault();
                if (data!=null)
                {
                    if (data.AssignedAsst != null)
                    {
                        var dadada = db.tblExpertDetails.Where(a => a.ExpertId == data.AssignedAsst).FirstOrDefault().FirstName;
                        TempData["Status"] = "<script>alert('Your Application is Solved By " + dadada + " on ')</script>";

                    }
                    else
                    {
                        TempData["Status"] = "<script>alert('Your Application Status Is Pending !')</script>";
                    }
                }
                else
                {
                    TempData["Status"] = "<script>alert('Your Application Is Invalid ,Please Contact To Kisan Kendra')</script>";

                }
                return View();
            }
            if (StatusType == "SoilTest")
            {
                var data = db.tblSoilTestings.Where(a => a.TestId == AppID).FirstOrDefault();
                if (data != null)
                {
                    if (data.TestedBy != null)
                    {
                        var dadada = db.tblExpertDetails.Where(a => a.ExpertId == data.TestedBy).FirstOrDefault().FirstName;
                        TempData["Status"] = "<script>alert('Your Application is Assiend to " + dadada + "')</script>";

                    }
                    else
                    {
                        TempData["Status"] = "<script>alert('Your Application Status Is Pending !')</script>";
                    }
                }
                else
                {
                    TempData["Status"] = "<script>alert('Your Application Is Invalid ,Please Contact To Kisan Kendra')</script>";

                }

                return View();
            }
            else
            {
                TempData["Status"] = "<script>alert('Invalid ID')</script>";
            }
            return View();
        }
    }
}