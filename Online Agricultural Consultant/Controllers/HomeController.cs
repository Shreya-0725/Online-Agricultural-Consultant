using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Online_Agricultural_Consultant.Models;

namespace Online_Agricultural_Consultant.Controllers
{
    public class HomeController : Controller
    {
        Online_Agricultural_Consultant_DBEntities db = new Online_Agricultural_Consultant_DBEntities();
        Random rand = new Random();
        public ActionResult Index()
        {
            ViewData["Articals"] = db.tblArticles.ToList();


            return View();
        }


        public ActionResult Multi_Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Multi_Login(string UserID, string Password, string UserType)
        {
            if (UserType== "Expert")
            {
                if (db.tblExpertDetails.Any(a => a.ExpertId == UserID && a.Password == Password))
                {
                    Session["Expert_ID"] = UserID;
                    return RedirectToAction("Dashboard","Expert");
                }
            }
            else if (UserType == "Staff")
            {
                if (db.tblStaffDetails.Any(a => a.EmpId == UserID && a.Password == Password))
                {
                    Session["Staff_ID"] = UserID;
                    return RedirectToAction("Dashboard","Staff");
                }
            }
            else if (UserType == "Farmer")
            {
                if (db.tblFarmerDetails.Any(a => a.FarmerId == UserID && a.Password == Password))
                {
                    Session["UserID"] = UserID;
                    return RedirectToAction("Dashboard","User");
                }
            }
            else if (UserType == "Kisan Kendra")
            {
                if (db.tblKisaanKendraDetails.Any(a => a.KKId == UserID && a.Password == Password))
                {
                    Session["KKId"] = UserID;
                    return RedirectToAction("Dashboard","KisanKendra");
                }
            }
            TempData["userloginmsg"] = "Invalid UserID or Password";
            return RedirectToAction("Multi_Login");
        }



        public ActionResult user_registration()
        {
            ViewBag.States = db.tblStates.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult user_registration(tblFarmerDetail obj)
        {

            obj.FarmerId = "FARM" + rand.Next(9999, 1000000);
            obj.Password = GeneratePasswords.generate();
            obj.CreatedDate = DateTime.Now.ToString();
            db.tblFarmerDetails.Add(obj);
            db.SaveChanges();
            TempData["user_registration_msg"] = "Registration Success";
            return RedirectToAction("SuccessPage", new {FramerID=obj.FarmerId, FullName=obj.FirstName,Pass=obj.Password });
        }

        public ActionResult SuccessPage(string FramerID, string FullName, string Pass)
        {
            ViewBag.farmerID = FramerID;
            ViewBag.FullName = FullName;
            ViewBag.Pass = Pass;
            return View();
        }

        #region JsonCode
        public JsonResult getDistrictByStateCode(int statecode)
        {
            var list = db.tblDistricts.Where(a => a.StateCode == statecode).ToList();
            var Result = new { list };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        #endregion




        #region Expert Section
        public ActionResult expert_registration()
        {
            ViewBag.States = db.tblStates.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult expert_registration(tblExpertDetail obj, HttpPostedFileBase Image)
        {
            obj.ExpertId= "EXP" + rand.Next(9999, 1000000);
            obj.Password = GeneratePasswords.generate();
            obj.Image = "/ExpertDocuments/" + FIleUpload.UploadImage(Image, "ExpertDocuments");
            obj.CreateDate = DateTime.Now;
            db.tblExpertDetails.Add(obj);
            db.SaveChanges();
            TempData["user_registration_msg"] = "Registration Success";
            return RedirectToAction("SuccessPage", new { FramerID = obj.ExpertId, FullName = obj.FirstName, Pass = obj.Password });

        }
        #endregion


        #region KisanKendra
        public ActionResult KK_Register()
        {
            ViewBag.States = db.tblStates.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult KK_Register(tblKisaanKendraDetail obj)
        {
            obj.KKId = "KK" + GeneratePasswords.generate();
            obj.Status = "active";
            db.tblKisaanKendraDetails.Add(obj);
            db.SaveChanges();
            TempData["mmmm"] = "<script>alert('Registration Successful !')</script>";
            return RedirectToAction("SuccessPage", new { FramerID = obj.KKId, FullName = "", Pass = obj.Password });
        }
        #endregion


        public ActionResult aboutus()
        {
            return View();
        }

        public ActionResult contactus()
        {
            return View();
        }
        public ActionResult articals()
        {
            return View(db.tblArticles.ToList());
        }

    }
}