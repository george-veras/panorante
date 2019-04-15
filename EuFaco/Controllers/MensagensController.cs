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
    public class MensagensController : Controller
    {
        public ActionResult Index()
        {
            DAOMensagem daoMensagem = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                daoMensagem = new DAOMensagem();

                switch (usuario.Perfil)
                {
                    case Usuario.TipoPerfil.UsuarioParticular:
                        masterName = "_LayoutUsuarioParticular";
                        break;
                    case Usuario.TipoPerfil.UsuarioProfissional:
                        masterName = "_LayoutUsuarioProfissional";
                        break;
                    default:
                        masterName = "_LayoutUsuarioProfissional";
                        break;
                }
                return View("Index", masterName, daoMensagem.ObterMensagens(usuario));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
