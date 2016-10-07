using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JDV.Controllers
{
    public class ModuleMasterController : Controller
    {
        // GET: ModuleMaster
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FacultyMaster()
        {
            return View();
        }

        public ActionResult BatchMaster()
        {
            return View();
        }

        public ActionResult StaffMaster()
        {
            return View();
        }

        public ActionResult studentMaster()
        {
            return View();
        }

        public ActionResult LayoutMaster()
        {
            return View();
        }
        public ActionResult exLayoutCheck()
        {
            return View();
        }
    }
}