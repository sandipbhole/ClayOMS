using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COD = Clay.OMS.Data;

using COM = Clay.OMS.Message;
using PagedList;
using System.Web.Script.Serialization;
using System.Net;

namespace ClayOMS.Controllers
{
    public class FacultyController : Controller
    {

        COD.FacultyDAL facultyDAL = new COD.FacultyDAL();
        // GET: FacultyMaster
       public ActionResult Faculty()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetFacultyList(string sortOrder,  int ? page, string paging, string facultyName, bool _search, string Dean)  //Gets the todo Lists.
      {
            //// int recordCount = 0;

            COM.Faculty requestSetFaculty = new COM.Faculty();
            requestSetFaculty.facultyName = facultyName;
            requestSetFaculty.dean = Dean;
            requestSetFaculty.activated = true;
                //List<CM.Faculty> responseDevice = new List<CM.Faculty>();
            //ViewBag.currentSort = sortOrder;
            //if (String.IsNullOrEmpty(sortOrder))
            //    sortOrder = "createDateSortDesc";

            //ViewBag.createDateSort = sortOrder == "createDateSortDesc" ? "createDateSort" : "createDateSortDesc";
            //ViewBag.deviceTypeSort = sortOrder == "deviceTypeSort" ? "deviceTypeSortDesc" : "deviceTypeSort";
            //ViewBag.productSerialSort = sortOrder == "productSerialSort" ? "productSerialSortDesc" : "productSerialSort";
            //ViewBag.deviceDetailsSort = sortOrder == "deviceDetailsSort" ? "deviceDetailsSortDesc" : "deviceDetailsSort";
            //ViewBag.statusSort = sortOrder == "statusDeviceSort" ? "statusDeviceSortDesc" : "statusDeviceSort";


            int pageSize = 0;
            if (string.IsNullOrEmpty(paging))
                pageSize = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PageSize"]); //mC.CommonMobikonIMS.selectedPageSize;
            else
                pageSize = Convert.ToInt16(paging);

            if (facultyName != null)
                page = 1;
            if (Dean != null)
                page = 1;
         
            //if (!string.IsNullOrEmpty(facultyName))
            //    ViewBag.productSerialFilter = facultyName;
            //else
            //{
            //   // Dean = Dean;
            //   // ViewBag.productSerialFilter = FacultyName;
            //}
            int pageNumber = (page ?? 1);
            try
            {


                List<COM.Faculty> responseGetFaculty = facultyDAL.GetFaculty(requestSetFaculty);

                var Faculty = responseGetFaculty.Select(
                       faculty => new
                       {
                           facultyID = faculty.facultyID,
                           code = faculty.code,
                           facultyName = faculty.facultyName,
                           dean = faculty.dean,
                           activated = faculty.activated,
                           yearOfEstablishment = faculty.yearOfEstablishment,
                           updateUser = faculty.updateUser,
                           updateDate = (Convert.ToDateTime(faculty.updateDate)).ToString("dd/MM/yyyy"),
                       }
                    );

             

                var jsonData = new
                {
                    

                    page,
                  
                    rows = Faculty
                };
                string json = new JavaScriptSerializer().Serialize(jsonData);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Logger.Logger.Error(ex);
                // BusinessHelper.LogException(ex, "", "Client Master - GetClient", 1);
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InsertFaculty()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertFaculty(COM.Faculty requestSetFaculty, string command)
        {
            bool result = false;
            //if (Session["UserName"] == null)
            //    return RedirectToAction("Login", "Login");

            if (command == "Cancel")
                return RedirectToAction("CreateDevice");

            if (command == "Close")
                return RedirectToAction("MobikonIMS", "MobikonIMS");

            if (command == "Save")
            {
                requestSetFaculty.addUser = "Shweta";
              //  requestSetDevice.userName = Session["UserName"].ToString();

          

                if (ModelState.IsValid)
                {
                    //if ((Session["Role"].ToString() != "Sales" || Session["Role"].ToString() != "Accounts" || Session["Role"].ToString() != "Administrator") && requestSetDevice.status == "Blocked")
                    //{
                    //    ViewBag.message = "Not authorized to create blocked status device.";
                    //    return View(requestSetFaculty);
                    //}

                    //if (true == facultyDAL.InsertFaculty(requestSetFaculty))
                    //{
                    //    ViewBag.message = "Device already exists.";
                    //    return View(requestSetFaculty);
                    //}

                    result = facultyDAL.InsertFaculty(requestSetFaculty);
                    if (result == true)
                        return RedirectToAction("Faculty");
                }
                if (result == false)
                    ViewBag.message = "Device not saved successfully.";

                return View(requestSetFaculty);
            }
            return RedirectToAction("Login", "Login");
        }


       
        public ActionResult UpdateFaculty(long facultyID)
        {
            COM.Faculty requestSetFaculty = new COM.Faculty();
            requestSetFaculty.facultyID = facultyID;
            //if (Session["UserName"] == null)
            //    return RedirectToAction("Login", "Login");

            //if (string.IsNullOrEmpty(clientName))
            if (facultyID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.userName = "Welcome " + Session["UserName"] + " (" + Session["Role"] + ")";
            //ViewBag.loginDateTime = Session["LoginDateTime"];
            //ViewBag.menuName = "Edit Brand";

            requestSetFaculty = facultyDAL.FetchFaculty(requestSetFaculty);
           
            return View(requestSetFaculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFaculty(COM.Faculty requestSetFaculty, string command)
        {

            requestSetFaculty.updateUser = "Shweta";
            //if (Session["UserName"] == null)
            //    return RedirectToAction("Login", "Login");


            //if (command == "Cancel")
            //{
            //    return RedirectToAction("EditClient", "Client", new { clientID = requestSetClient.clientID });
            //}

            //if (command == "Close")
            //    return RedirectToAction("Client");

            bool updateFaculty = false;

            if (command == "Save")
            {
                //requestSetClient.userName = Session["UserName"].ToString();
                //ViewBag.countryName = countryDAL.GetCountryNameList(selectedCountryID: requestSetClient.countryID, selectedCountryName: requestSetClient.countryName);
                //ViewBag.cityName = cityDAL.GetCityNameList(selectedCityID: requestSetClient.cityID, selectedCityName: requestSetClient.cityName);

                if (ModelState.IsValid)
                {
                    updateFaculty = facultyDAL.UpdateFaculty(requestSetFaculty);
                    if (updateFaculty == true)
                        return RedirectToAction("Faculty");
                    else
                        ViewBag.Message = "Brand not updated successfully.";
                }

                return View(requestSetFaculty);
            }
            return RedirectToAction("Login", "Login");
        }
    }
}