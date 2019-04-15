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
    public class BuscadorController : Controller
    {
        [ActionName("Profissionais")]
        public ActionResult AbrirBuscadorProfissionais()
        {
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                daoUsuarioProfissional = new DAOUsuarioProfissional();
                
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
                return View("BuscarProfissionais", masterName, daoUsuarioProfissional.ObterProfissionais());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult PesquisarProfissionaisPorNome(string nome)
        {
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            Usuario usuario = null;
            List<UsuarioProfissional> listaProfissionais = null;
            Profissa profissionalViewModel = null;
            List<Profissa> listaViewModel = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                daoUsuarioProfissional = new DAOUsuarioProfissional();

                listaProfissionais = daoUsuarioProfissional.ObterProfissionaisPorNome(nome);
                listaViewModel = new List<Profissa>();
                foreach (UsuarioProfissional profissional in listaProfissionais)
                {
                    profissionalViewModel = new Profissa();
                    profissionalViewModel.Id = profissional.Id.ToString();
                    profissionalViewModel.Nome = profissional.Nome;
                    profissionalViewModel.Email = profissional.Email;
                    profissionalViewModel.Resumo = profissional.Resumo;
                    profissionalViewModel.DataHoraCadastro = profissional.DataHoraCadastro.ToString("dd/MM/yyyy");

                    listaViewModel.Add(profissionalViewModel);
                }
                
                return Json(listaViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class Profissa
        {
            private string _Nome;
            private string _Resumo;
            private string _Email;
            private string _DataHoraCadastro;
            private string _Id;

            public string DataHoraCadastro
            {
                get { return _DataHoraCadastro; }
                set { _DataHoraCadastro = value; }
            }

            public string Id
            {
                get { return _Id; }
                set { _Id = value; }
            }

            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            public string Nome
            {
                get { return _Nome; }
                set { _Nome = value; }
            }

            public string Resumo
            {
                get { return _Resumo; }
                set { _Resumo = value; }
            }
        }
    }
}