using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Configuration;
using EuFaco.DataAccess.Models;
using EuFaco.DataAccess.DAOs;

namespace EuFaco.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                return View("Index", "_LayoutUsuarioParticular");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}