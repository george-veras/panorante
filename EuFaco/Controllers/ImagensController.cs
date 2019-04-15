using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using EuFaco.DataAccess.Models;
using EuFaco.DataAccess.DAOs;

namespace EuFaco.Controllers
{
    public class ImagensController : Controller
    {
        [ActionName("Imagem")]
        public ActionResult AbrirImagem(int id)
        {
            DAOImagem daoImagem = null;
            Imagem imagem = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (UsuarioProfissional)Session["Usuario"];

                daoImagem = new DAOImagem();
                imagem = daoImagem.ObterImagem(id);

                switch (usuario.Perfil)
                {
                    case Usuario.TipoPerfil.UsuarioParticular:
                        masterName = "_LayoutUsuarioParticular";
                        break;
                    case Usuario.TipoPerfil.UsuarioProfissional:
                        masterName = "_LayoutUsuarioProfissional";
                        break;
                    default:
                        masterName = "_LayoutUsuarioParticular";
                        break;
                }
                return View("Imagem", masterName, imagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
