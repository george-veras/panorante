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
    public class PerguntasController : Controller
    {
        public ActionResult PerguntarComunidade()
        {
            Usuario usuario = null;
            string masterName;
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
                return View("PerguntarComunidade", masterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PublicarPergunta(PerguntaViewModel novaPergunta)
        {
            Pergunta pergunta = null;
            DAOPergunta daoPergunta = null;
            try
            {
                pergunta = new Pergunta();
                pergunta.Titulo = novaPergunta.Titulo;
                pergunta.Texto = novaPergunta.Texto;
                pergunta.Autor = (Usuario)Session["Usuario"];

                daoPergunta = new DAOPergunta();
                if (daoPergunta.IncluirPergunta(pergunta))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PesquisarPerguntas()
        {
            DAOPergunta daoPergunta = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                daoPergunta = new DAOPergunta();

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
                return View("PesquisarPerguntas", masterName, daoPergunta.ObterPerguntas());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [ActionName("Pergunta")]
        public ActionResult AbrirPergunta(int id)
        {
            DAOPergunta daoPergunta = null;
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];
                
                daoPergunta = new DAOPergunta();

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
                return View("Pergunta", masterName, daoPergunta.ObterPergunta(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SalvarResposta(RespostaViewModel novaResposta)
        {
            DAOResposta daoResposta = null;
            Usuario usuario = null;
            Resposta resposta = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                resposta = new Resposta();
                resposta.Autor = usuario;
                resposta.Texto = novaResposta.Texto;
                resposta.Pergunta.Id = int.Parse(novaResposta.IdPerguntaVinculada);

                daoResposta = new DAOResposta();
                if (daoResposta.IncluirResposta(resposta))
                {
                    novaResposta.Id = resposta.Id.ToString();
                    novaResposta.IdAutor = resposta.Autor.Id.ToString();
                    novaResposta.NomeAutor = resposta.Autor.Nome;
                    novaResposta.DataResposta = resposta.DataHoraRespondida.ToString("dd/MM/yyyy HH:mm");
                    return Json(novaResposta);
                }
                else
                    return Json(0); // Aprender como retorna uma resposta negativa caso a inserção dê errado.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SalvarComentarioResposta(ComentarioRespostaViewModel novoComentarioResposta)
        {
            DAOComentarioResposta daoComentarioResposta = null;
            Usuario usuario = null;
            ComentarioResposta modelComentarioResposta = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                modelComentarioResposta = new ComentarioResposta();
                modelComentarioResposta.Autor = usuario;
                modelComentarioResposta.Texto = novoComentarioResposta.Texto;
                modelComentarioResposta.RespostaComentada.Id = int.Parse(novoComentarioResposta.IdRespostaComentada);

                daoComentarioResposta = new DAOComentarioResposta();
                if (daoComentarioResposta.IncluirComentario(modelComentarioResposta))
                {
                    novoComentarioResposta.IdAutor = modelComentarioResposta.Autor.Id.ToString();
                    novoComentarioResposta.NomeAutor = modelComentarioResposta.Autor.Nome;
                    novoComentarioResposta.DataHora = modelComentarioResposta.DataHoraComentada.ToString("dd/MM/yyyy HH:mm");
                    return Json(novoComentarioResposta);
                }
                else
                    return Json(0); // Aprender como retorna uma resposta negativa caso a inserção dê errado.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class RespostaViewModel
    {
        private string _IdPerguntaVinculada;
        private string _Texto;
        private string _IdAutor;
        private string _DataResposta;
        private string _Id;
        private List<ComentarioRespostaViewModel> _Comentarios;
        private string _NomeAutor;

        public string NomeAutor
        {
            get { return _NomeAutor; }
            set { _NomeAutor = value; }
        }

        public List<ComentarioRespostaViewModel> Comentarios
        {
            get { return _Comentarios; }
            set { _Comentarios = value; }
        }

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string DataResposta
        {
            get { return _DataResposta; }
            set { _DataResposta = value; }
        }

        public string IdAutor
        {
            get { return _IdAutor; }
            set { _IdAutor = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public string IdPerguntaVinculada
        {
            get { return _IdPerguntaVinculada; }
            set { _IdPerguntaVinculada = value; }
        }
    }

    public class ComentarioRespostaViewModel
    {
        private string _Id;
        private string _Texto;
        private string _DataHora;
        private string _IdAutor;
        private string _IdRespostaComentada;
        private string _NomeAutor;

        public string NomeAutor
        {
            get { return _NomeAutor; }
            set { _NomeAutor = value; }
        }

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public string DataHora
        {
            get { return _DataHora; }
            set { _DataHora = value; }
        }

        public string IdAutor
        {
            get { return _IdAutor; }
            set { _IdAutor = value; }
        }

        public string IdRespostaComentada
        {
            get { return _IdRespostaComentada; }
            set { _IdRespostaComentada = value; }
        }
    }

    public class PerguntaViewModel
    {
        private string _Id;
        private string _Texto;
        private string _Titulo;
        private string _IdPublicador;
        private List<RespostaViewModel> _Respostas;

        public List<RespostaViewModel> Respostas
        {
            get { return _Respostas; }
            set { _Respostas = value; }
        }

        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public string Titulo
        {
            get { return _Titulo; }
            set { _Titulo = value; }
        }

        public string IdPublicador
        {
            get { return _IdPublicador; }
            set { _IdPublicador = value; }
        }
    }
}
