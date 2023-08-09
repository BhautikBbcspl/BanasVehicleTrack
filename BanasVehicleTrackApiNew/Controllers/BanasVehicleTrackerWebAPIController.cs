using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using BanasVehicleTrackViewModel.AuditorDepartmentApp;
using BanasVehicleTrackViewModel.Master;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;


namespace BanasVehicleTrackApiNew.Controllers
{

    public class BanasVehicleTrackerWebAPIController : ApiController
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region==> Login
        [Route("api/BanasVehicleTrackerWebAPI/LoginRtr")]
        [HttpPost]
        public HttpResponseMessage LoginRtr(EmployeeViewModel empp)
        {
            try
            {
                //empp.Password = generalFunctions.Encrypt(empp.Password, true).ToString().Trim();
                string re = db.BanasLoginRtr_NEW(empp.EmployeeCode).ToString();
                //string re = db.BanasLoginRtr_NEW(empp.EmployeeCode, empp.Password).ToString();
                if (re != null)
                {
                    var uc = db.BanasLoginRtr_NEW(empp.EmployeeCode).FirstOrDefault();
                    //var uc = db.BanasLoginRtr_NEW(empp.EmployeeCode, empp.Password).FirstOrDefault();

                    var response2 = Request.CreateResponse(HttpStatusCode.OK, uc);
                    response2.Content = new StringContent(JsonConvert.SerializeObject(uc), Encoding.UTF8, "application/json");
                    //return response2;
                    if (uc.ToString()=="Is Auditor")
                    {
                        string query3 = "{ \"LoginResult\":\"Is Auditor\"}";
                        var jObject = JObject.Parse(query3);
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }
                    else
                    {
                        string query3 = "{ \"LoginResult\":\"Is Not Auditor\"}";
                        var jObject = JObject.Parse(query3);
                        var response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                        return response;
                    }

                }
                else
                {
                    string query3 = "{ \"LoginResult\":\"Invalid Contact No or Password\"}";
                    var jObject = JObject.Parse(query3);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                    return response;

                }
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid Contact No or Password"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                response.Content = new StringContent(JsonConvert.SerializeObject(ERROR), Encoding.UTF8, "application/json");
                return response;
            }
            finally
            {
                db.Dispose();
            }
        }
        #endregion

        #region==> Fetch Open GatePass
        [Route("api/BanasVehicleTrackerWebAPI/PendingVisitGatePass")]
        [HttpPost]
        public HttpResponseMessage PendingVisitGatePass(PendingGatePass pnd)
        {
            try
            {

                string re = db.BanasAuditorVisitGatepassRetrieve_NEW(pnd.Action, pnd.UserCode, "").ToString();
                if (re != null)
                {
                    var PendingGatePass = db.BanasAuditorVisitGatepassRetrieve_NEW(pnd.Action, pnd.UserCode, "").ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, PendingGatePass);
                    return response2;
                }
                else
                {
                    string query3 = "{ \"LoginResult\":\"Invalid User\"}";
                    var jObject = JObject.Parse(query3);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            { db.Dispose(); }
        }
        #endregion

        #region ==> Insert Operate Visit
        [Route("api/BanasVehicleTrackerWebAPI/OperateVisit")]
        [HttpPost]
        public HttpResponseMessage OperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(aov.OdometerImage);

                string fileName = Guid.NewGuid().ToString() + ".jpg";

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/OdometerImage");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                };
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                aov.OdometerImage = "/Uploads/OdometerImage/" + fileName;
                //aov.OdometerImage = filePath;

                string re = db.BanasAuditorOperateVisitMasterIns(aov.GatePassId, aov.Latitude, aov.Longitude, aov.Odometer, aov.OdometerImage, aov.CreateDate, aov.CreateUser, aov.CenterId,"insert",aov.Remark,aov.LocationName, aov.UserGivenLocation).FirstOrDefault();

                var response = Request.CreateResponse(HttpStatusCode.OK, re);
                return response;
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            {
                db.Dispose();
            }
        }
        #endregion

        #region ==> Insert Final Operate Visit
        [Route("api/BanasVehicleTrackerWebAPI/FinalOperateVisit")]
        [HttpPost]
        public HttpResponseMessage FinalOperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(aov.LastOdometerImage);

                string fileName = Guid.NewGuid().ToString() + ".jpg";

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/OdometerImage");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                };
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                aov.LastOdometerImage = "/Uploads/OdometerImage/" + fileName;
                //aov.LastOdometerImage = filePath;

                string re = db.BanasAuditorFinalOperateVisitMasterIns(aov.MidwayDeparture, aov.GatePassId, aov.LastOdometer, aov.LastOdometerImage, aov.UserCode, aov.Latitude, aov.Longitude, aov.CreateDate, aov.CreateUser,aov.CenterId ,"insert", aov.Remark, aov.LocationName, aov.UserGivenLocation).FirstOrDefault();
                var response = Request.CreateResponse(HttpStatusCode.OK, re);
                return response;
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;

            }
            finally
            { db.Dispose(); }
        }
        #endregion

        #region ==> Retrive Auditor Visits
        [Route("api/BanasVehicleTrackerWebAPI/RetrieveOperateVisit")]
        [HttpPost]
        public HttpResponseMessage RetrieveOperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {

                string re = db.BanasAuditorOperateVisitMasterRtr(aov.GatePassId, aov.Action).ToString();
                if (re != null)
                {
                    var AudVisits = db.BanasAuditorOperateVisitMasterRtr(aov.GatePassId, aov.Action).ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, AudVisits);
                    return response2;
                }
                else
                {
                    string query3 = "{ \"Result\":\"Invalid User\"}";
                    var jObject = JObject.Parse(query3);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            { db.Dispose(); }
        }
        #endregion

        #region ==> Retrive Auditor Final Visits
        [Route("api/BanasVehicleTrackerWebAPI/RetrieveFinalOperateVisit")]
        [HttpPost]
        public HttpResponseMessage RetrieveFinalOperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {

                string re = db.BanasAuditorFinalOperateVisitMasterRtr(aov.GatePassId, aov.Action).ToString();
                if (re != null)
                {
                    var AudVisits = db.BanasAuditorFinalOperateVisitMasterRtr(aov.GatePassId, aov.Action).ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, AudVisits);
                    return response2;
                }
                else
                {
                    string query3 = "{ \"Result\":\"Invalid User\"}";
                    var jObject = JObject.Parse(query3);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            { db.Dispose(); }
        }
        #endregion

        #region ==> Encryption/Decryption
        public class generalFunctions
        {
            public static string dateconvert(string strdate)
            {
                string str = "";
                str = strdate.Split('-').ElementAt(2) + "-" + strdate.Split('-').ElementAt(1) + "-" + strdate.Split('-').ElementAt(0);
                return str;
            }
            public static string Encrypt(string toEncrypt, bool useHashing)
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
                string key = "bbcspl";

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            public static string Decrypt(string cipherString, bool useHashing)
            {
                byte[] keyArray;
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                string key = "bbcspl";

                if (useHashing)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }

        }
        #endregion

        #region ==> Retrive Centers
        //44337
        [Route("api/BanasVehicleTrackerWebAPI/RetrieveCenters")]
        [HttpPost]
        public HttpResponseMessage RetrieveCenters(CenterMasterViewModel aov)
        {
            try
            {

                string re = db.BanasCenterMasterRetrieve(aov.Action).ToString();
                if (re != null)
                {
                    var CenterList = db.BanasCenterMasterRetrieve(aov.Action).ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, CenterList);
                    return response2;
                }
                else
                {
                    string query3 = "{ \"Result\":\"Invalid\"}";
                    var jObject = JObject.Parse(query3);
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (Exception)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            { db.Dispose(); }
        }
        #endregion
    }
}