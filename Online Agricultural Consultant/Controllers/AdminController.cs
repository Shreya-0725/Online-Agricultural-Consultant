using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Agricultural_Consultant.Models;

namespace Online_Agricultural_Consultant.Controllers
{
    public class AdminController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        Random rand = new Random();
        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string AdminUserID, string Password)
        {
            if (db.tblAdmins.Any(a=>a.AdminUserID== AdminUserID && a.Password == Password))
            {
                Session["ADMIN_ID"] = AdminUserID;
                return RedirectToAction("Dashboard");
            }
            TempData["MSG"] = "Invalid AdminID or Password !";
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult View_Profile(string ID)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            
            return View(db.tblStaffDetails.Where(a=>a.EmpId==ID).FirstOrDefault());
        }

        public ActionResult View_Expert_Profile(string ID)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }

            return View(db.tblExpertDetails.Where(a => a.ExpertId == ID).FirstOrDefault());
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
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
           
            return View();
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        #endregion



        #region ViewFarmerRegistration
        public ActionResult view_farmer_registration()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View(db.tblFarmerDetails.OrderByDescending(a=>a.ID).ToList());
        }
        #endregion

        #region Manange Experts
        public ActionResult view_expert_registration()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View(db.tblExpertDetails.OrderByDescending(a => a.ID).ToList());
        }
        #endregion


        #region manage staff
        public ActionResult add_staff()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult add_staff(tblStaffDetail obj, HttpPostedFileBase Image)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            obj.EmpId = "STAFF" + rand.Next(9999, 1000000);
            obj.Password = GeneratePasswords.generate();
            obj.Image = "/ExpertDocuments/" + FIleUpload.UploadImage(Image, "ExpertDocuments");
            obj.CreatedDate = DateTime.Now;
            db.tblStaffDetails.Add(obj);
            db.SaveChanges();
            TempData["staff_registration_msg"] = "Registration Success";
            return View();
        }


        public ActionResult view_staff_list()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View(db.tblStaffDetails.ToList());
        }
        #endregion

        #region EnqueryDetails
        public ActionResult EnqueryDetails()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View(db.tblQueryDetails.ToList());
        }
        #endregion


        #region rta_assign
        public ActionResult rta_assign()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            ViewBag.Experts = db.tblExpertDetails.ToList();
            return View(db.tblSiteVisits.ToList());
        }

        public ActionResult assign_expert(int ID_, string expert)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }

            var data = db.tblSiteVisits.Where(a => a.ID == ID_).FirstOrDefault();
            if (data != null)
            {
                data.AssignedAsst = expert;
                data.ModificationDate = DateTime.Now;
                db.Entry<tblSiteVisit>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            TempData["MSG__"] = "Expert Assigned !!";
            return RedirectToAction("rta_assign");
        }

        #endregion

        #region Kisan Kendra
        public ActionResult add_new_Details()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            ViewBag.States = db.tblStates.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult add_new_Details(tblKisaanKendraDetail obj)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            obj.KKId ="KK"+ GeneratePasswords.generate();
            obj.Password = GeneratePasswords.generate();
            obj.Status = "active";
            db.tblKisaanKendraDetails.Add(obj);
            db.SaveChanges();
            TempData["mmmm"] = "Data Added";
            return RedirectToAction("add_new_Details");
        }

        public ActionResult view_all_kk_details()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            return View(db.tblKisaanKendraDetails.ToList());
        }

        public ActionResult DeleteKKdetails(int ID)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            var Data = db.tblKisaanKendraDetails.Where(a=>a.ID==ID).FirstOrDefault();
            if (Data!=null)
            {
                db.tblKisaanKendraDetails.Remove(Data);
                db.SaveChanges();
                TempData["kkmsg"] = "<script>alert('Date Removed Successfuly !')</script>";
            }
            return RedirectToAction("view_all_kk_details");
        }
        #endregion

        #region MyRegion
        public ActionResult Soiltest_Expert_assign()
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }
            ViewBag.Experts = db.tblExpertDetails.ToList();
            return View(db.tblSoilTestings.ToList());
        }

        public ActionResult assign_Soiltest_Expert_(int ID_, string expert)
        {
            if (Session["ADMIN_ID"] == null)
            {
                TempData["MSG"] = "Session Expired Or Invalid URL ! Login Again. !";
                return RedirectToAction("Index");
            }

            var data = db.tblSoilTestings.Where(a => a.ID == ID_).FirstOrDefault();
            if (data != null)
            {
                data.TestedBy = expert;
                db.Entry<tblSoilTesting>(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            TempData["MSG__1"] = "Expert Assigned !!";
            return RedirectToAction("Soiltest_Expert_assign");
        }
        #endregion


        public ActionResult articals()
        {
            return View(db.tblArticles.ToList());
        }
    }
}