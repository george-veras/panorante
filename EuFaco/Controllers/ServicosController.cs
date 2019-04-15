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
    public class ServicosController : Controller
    {
        public ActionResult PesquisarServicos()
        {
            DAOServico daoServico = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                daoServico = new DAOServico();

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
                return View("PesquisarServicos", masterName, daoServico.ObterServicos());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Servico")]
        public ActionResult AbrirServico(int id)
        {
            DAOServico daoServico = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoServico = new DAOServico();
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
                return View("Servico", masterName, daoServico.ObterServico(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("ServicosSolicitados")]
        public ActionResult AbrirServicosSolicitados()
        {
            DAOServico daoServico = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoServico = new DAOServico();
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
                return View("ServicosSolicitados", masterName, daoServico.ObterServicosSolicitados(usuario));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult SolicitarServico()
        {
            Usuario usuario = null;
            string masterName = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

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
                return View("SolicitarServico", masterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PublicarServico(ServicoViewModel novoServico)
        {
            Usuario usuario = null;
            Servico servico = null;
            DAOServico daoServico = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                servico = new Servico();
                servico.Titulo = novoServico.Titulo;
                servico.Descricao = novoServico.Descricao;
                servico.Solicitante = usuario;

                daoServico = new DAOServico();
                if (daoServico.IncluirServico(servico))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Candidatar(CandidaturaViewModel novaCandidatura)
        {
            DAOCandidatura daoCandidatura = null;
            Candidatura candidatura = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                candidatura = new Candidatura();
                candidatura.Mensagem = novaCandidatura.Mensagem;
                candidatura.Candidato = usuario;
                candidatura.ServicoCandidatado.Id = int.Parse(novaCandidatura.IdServico);

                daoCandidatura = new DAOCandidatura();
                if (daoCandidatura.IncluirCandidatura(candidatura))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class CandidaturaViewModel
        {
            private string _IdServico;
            private string _Mensagem;

            public string Mensagem
            {
                get { return _Mensagem; }
                set { _Mensagem = value; }
            }

            public string IdServico
            {
                get { return _IdServico; }
                set { _IdServico = value; }
            }
        }
    }

    public class ServicoViewModel
    {
        private string _Titulo;
        private string _Descricao;

        public string Titulo
        {
            get
            {
                return _Titulo;
            }

            set
            {
                _Titulo = value;
            }
        }

        public string Descricao
        {
            get
            {
                return _Descricao;
            }

            set
            {
                _Descricao = value;
            }
        }
    }
}
