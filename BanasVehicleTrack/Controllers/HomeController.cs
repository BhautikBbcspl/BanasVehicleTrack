using BanasVehicleTrack.GeneralClasses;
using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace BanasVehicleTrack.Controllers
{
    public class HomeController : GeneralClass
    {

        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        public ActionResult Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                HttpCookie reqCookies = Request.Cookies["EmployeeMaster"];
                if (reqCookies == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                DashboardViewModel model = new DashboardViewModel();
                model.EmployeeCode = LoggedUserDetails.EmployeeCode;
                model.DashboardCounts = db.BanasAdminDashboardCountRtr("Count", model.EmployeeCode).ToList();
                //model.DashboardList = db.BanasAdminDashboardListRtr("List").ToList();
                model.DashboardList = db.BanasAdminDashboardListRtr("List",LoggedUserDetails.EmployeeCode).OrderByDescending(item => item.CreateDate).Take(10).ToList();
                return View(model);
            }
        }

        #region ==> Login / LogOut / ChangePassword
        public ActionResult Login()
        {
            HttpCookie reqCookies = Request.Cookies["EmployeeMaster"];
            if (reqCookies != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeViewModel emp)
        {

            if (ModelState.IsValid)
            {
                //string dec =generalFunctions.Decrypt("CfOe3W0JyRTTWmn4gv01qg==", true);
                string passmatch = generalFunctions.Encrypt(emp.Password, true).ToString().Trim();
                string IsValidUser = db.BanasLoginCheck(emp.EmployeeCode, passmatch).FirstOrDefault();
                if (IsValidUser.Equals("1"))
                {
                    EmployeeViewModel em = new EmployeeViewModel();
                    FormsAuthentication.RedirectFromLoginPage(emp.EmployeeCode, true);

                    LoginMaster LoginMaster = db.LoginMasters.Where(x => x.Username == emp.EmployeeCode && x.UserPassword == passmatch).FirstOrDefault();
                    EmployeeMaster employeeMaster = db.EmployeeMasters.Where(u => u.EmployeeCode == LoginMaster.Username).FirstOrDefault();

                    BanasRoleMasterRetrieve_Result Role = db.BanasRoleMasterRetrieve("all", "").Where(x => x.RoleId == LoginMaster.RoleId).FirstOrDefault();
                    HttpCookie EmployeeMaster = new HttpCookie("EmployeeMaster");

                    if (LoginMaster.RoleId.ToString() == "2")
                    {
                        EmployeeMaster["Companycode"] = Convert.ToString("");
                        if (LoginMaster.Username != null)
                        {
                            //EmployeeMaster["EmployeeName"] = LoginMaster.EmployeeName;
                            EmployeeMaster["EmployeeCode"] = LoginMaster.Username;
                            EmployeeMaster["CompanyCode"] = LoginMaster.Companycode;
                            EmployeeMaster["RoleName"] = Convert.ToString(Role.RoleName);
                            EmployeeMaster["RoleId"] = Convert.ToString(LoginMaster.RoleId);
                        }
                        Response.Cookies.Add(EmployeeMaster);
                        return RedirectToAction("SecurityDashboard", "SecurityDepartment");
                    }
                    else if (LoginMaster.RoleId.ToString() == "3")
                    {
                        EmployeeMaster["Companycode"] = Convert.ToString("");
                        if (LoginMaster.Username != null)
                        {
                            //EmployeeMaster["EmployeeName"] = LoginMaster.EmployeeName;
                            EmployeeMaster["EmployeeCode"] = LoginMaster.Username;
                            EmployeeMaster["CompanyCode"] = LoginMaster.Companycode;
                            EmployeeMaster["RoleName"] = Convert.ToString(Role.RoleName);
                            EmployeeMaster["RoleId"] = Convert.ToString(LoginMaster.RoleId);

                            EmployeeMaster["DepartmentId"] = Convert.ToString(employeeMaster.DepartmentId);
                        }
                        Response.Cookies.Add(EmployeeMaster);
                        return RedirectToAction("AuditorDashboard", "AuditorDepartment");
                    }
                    else
                    {

                        EmployeeMaster["Companycode"] = Convert.ToString("");
                        if (LoginMaster.Username != null)
                        {
                            //EmployeeMaster["EmployeeName"] = LoginMaster.EmployeeName;
                            EmployeeMaster["EmployeeCode"] = LoginMaster.Username;
                            EmployeeMaster["CompanyCode"] = LoginMaster.Companycode;
                            EmployeeMaster["RoleName"] = Convert.ToString(Role.RoleName);
                            EmployeeMaster["RoleId"] = Convert.ToString(LoginMaster.RoleId);
                            //EmployeeMaster["DepartmentId"] = Convert.ToString(employeeMaster.DepartmentId);

                            if(employeeMaster.DepartmentId==null)
                            {
                                EmployeeMaster["DepartmentId"] = "admin";
                            }
                            else
                            {
                                EmployeeMaster["DepartmentId"] = Convert.ToString(employeeMaster.DepartmentId);
                            }
                        }
                        Response.Cookies.Add(EmployeeMaster);
                        return RedirectToAction("Dashboard", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                    return View("Login");
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();

            if (Request.Cookies["EmployeeMaster"] != null)
            {
                Response.Cookies["EmployeeMaster"].Expires = DateTime.Now.AddDays(-1);
            }

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ChangePassword()
        {
            HttpCookie reqCookies = Request.Cookies["EmployeeMaster"];
            ChangePasswordViewModel cp = new ChangePasswordViewModel()
            {
                CompanyCode = reqCookies["Companycode"].ToString(),
                EmployeeCode = reqCookies["EmployeeCode"].ToString(),
            };
            return View(cp);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel cpvm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cpvm.CreateDate = generalFunctions.getTimeZoneDatetimedb();

                    string asq = generalFunctions.Encrypt(cpvm.Password, true).ToString().Trim();
                    string passmatch = generalFunctions.Encrypt(cpvm.ConfirmPassword, true).ToString().Trim();


                    //string msg = db.BanasChangePassword(cpvm.EmployeeCode, cpvm.Password, cpvm.ConfirmPassword, cpvm.CompanyCode, "", "").FirstOrDefault();
                    string msg = db.BanasChangePassword(cpvm.EmployeeCode, asq, passmatch, cpvm.CompanyCode, "", "").FirstOrDefault();

                    ViewBag.Message = msg;
                    if (msg.Contains("successfully"))
                    {
                        ViewBag.Message = msg.ToUpper();
                        Success(msg, true);
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        ViewBag.Message = msg.ToUpper();
                        Danger(msg, true);
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                catch (Exception)
                {
                    //Danger(ex.Message.ToString(), true);
                    return RedirectToAction("Dashboard", "Home");
                }

            }
            return View();
        }
        #endregion

        #region => Menu
        public ActionResult GetMenu()
        {
            try
            {
                var result = db.BanasMenuRtr(LoggedUserDetails.RoleId).ToList();

                return View("Menu", result);
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
                return Content("Error");
            }
        }
        #endregion
    }
}