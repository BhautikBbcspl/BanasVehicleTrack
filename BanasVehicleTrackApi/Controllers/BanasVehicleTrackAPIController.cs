using BanasVehicleTrackDbClasses;
using BanasVehicleTrackViewModel;
using BanasVehicleTrackViewModel.AuditorDepartmentApp;
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


namespace BanasVehicleTrackApi.Controllers
{

    public class BanasVehicleTrackAPIController : ApiController
    {
        banasVehicleTrackingEntities db = new banasVehicleTrackingEntities();

        #region==> Login
        [Route("api/BanasVehicleTracking/LoginRtr")]
        [HttpPost]
        public HttpResponseMessage LoginRtr(EmployeeViewModel empp)
        {
            try
            {
                empp.Password = generalFunctions.Encrypt(empp.Password, true).ToString().Trim();
                string re = db.BanasLoginRtr(empp.EmployeeCode, empp.Password).ToString();
                if (re != null)
                {
                    var uc = db.BanasLoginRtr(empp.EmployeeCode, empp.Password).ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, uc);
                    return response2;
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
            catch (Exception ex)
            {
                List<FetchErrorclass> ERROR = new List<FetchErrorclass>();
                FetchErrorclass error = new FetchErrorclass
                {
                    FetchResult = Convert.ToString("Invalid Contact No or Password"),
                };
                ERROR.Add(error);

                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ERROR);
                return response;
            }
            finally
            { db.Dispose(); }
        }
        #endregion

        #region==> Fetch Open GatePass
        [Route("api/BanasVehicleTracking/PendingVisitGatePass")]
        [HttpPost]
        public HttpResponseMessage PendingVisitGatePass(PendingGatePass pnd)
        {
            try
            {

                string re = db.BanasAuditorVisitGatepassRetrieve("auditorGPOpen", pnd.UserCode, "").ToString();
                if (re != null)
                {
                    var PendingGatePass = db.BanasAuditorVisitGatepassRetrieve("auditorGPOpen", pnd.UserCode, "").ToList();
                    var response2 = Request.CreateResponse(HttpStatusCode.OK, PendingGatePass);
                    return response2;
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
            catch (Exception ex)
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
        [Route("api/BanasVehicleTracking/OperateVisit")]
        [HttpPost]
        public HttpResponseMessage OperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(aov.OdometerImage);

                string fileName = Guid.NewGuid().ToString() + ".jpg";

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/OdometerImages");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                };
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                //aov.OdometerImage = "~/Uploads/OdometerImage/" + fileName;
                aov.OdometerImage = filePath;

                string re = db.BanasAuditorOperateVisitMasterIns(aov.GatePassId, aov.Latitude, aov.Longitude, aov.Odometer, aov.OdometerImage, aov.CreateDate, aov.CreateUser, "insert").FirstOrDefault();

                var response = Request.CreateResponse(HttpStatusCode.OK, re);
                return response;
            }
            catch (Exception ex)
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
        [Route("api/BanasVehicleTracking/FinalOperateVisit")]
        [HttpPost]
        public HttpResponseMessage FinalOperateVisit(AuditorOperateVisitViewModel aov)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(aov.LastOdometerImage);

                string fileName = Guid.NewGuid().ToString() + ".jpg";

                string folderPath = HttpContext.Current.Server.MapPath("~/Uploads/OdometerImages");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                };
                string filePath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(filePath, imageBytes);

                //aov.LastOdometerImage = "~/Uploads/OdometerImage/" + fileName;
                aov.LastOdometerImage = filePath;

                string re = db.BanasAuditorFinalOperateVisitMasterIns(aov.MidwayDeparture, aov.GatePassId, aov.LastOdometer, aov.LastOdometerImage, aov.UserCode,  aov.Latitude, aov.Longitude, aov.CreateDate, aov.CreateUser, "insert").FirstOrDefault();
                var response = Request.CreateResponse(HttpStatusCode.OK, re);
                return response;
            }
            catch (Exception ex)
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

    }
}