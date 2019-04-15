using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EuFaco.Controllers
{
    public class SobreController : Controller
    {
        // GET: Sobre
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SobreNos()
        {
            return View("SobreNos", "_LayoutVisitante");
        }
    }
}