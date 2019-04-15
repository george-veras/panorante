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
    public class PerfilController : Controller
    {
        [ActionName("UsuarioProfissional")]
        public ActionResult PerfilTemplateUsuarioProfissional()
        {
            UsuarioProfissional usuarioProfissional = null;
            DAOPublicacao daoPublicacao = null;
            try
            {
                usuarioProfissional = (UsuarioProfissional)Session["Usuario"];

                daoPublicacao = new DAOPublicacao();
                daoPublicacao.PopularPublicacoes(usuarioProfissional);
                return View("PerfilUsuarioProfissional", usuarioProfissional);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Sair")]
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect(Url.Action("Index", "Home"));
        }

        public JsonResult UploadImagem()
        {
            DAOImagem daoImagem = null;
            Imagem novaImagem = null;
            Usuario usuario = null;
            string pathImagem = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    HttpPostedFile pic = System.Web.HttpContext.Current.Request.Files["datafile"];
                    Image imagem = Bitmap.FromStream(pic.InputStream);
                    pathImagem = @"\ImagensUsuarios\" + pic.FileName;
                    imagem.Save(@"C:\Users\george.valentim\Documents\visual studio 2012\Projects\EuFaco.DAL\EuFacoMvc\ImagensUsuarios\" + pic.FileName);
                }
                else
                    throw new Exception();

                novaImagem = new Imagem();
                novaImagem.Dono = usuario;
                novaImagem.PathImagem = pathImagem;

                daoImagem = new DAOImagem();
                if (daoImagem.IncluirImagem(novaImagem))
                    return Json("Aew salvou!");
                else
                    return Json("Deu ruim pra salvar a imagem hein...");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarLegendaFoto(LegendaViewModel legenda)
        {
            Usuario usuario = null;
            DAOImagem daoImagem = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoImagem = new DAOImagem();
                daoImagem.EditarLegenda(Int32.Parse(legenda.Id), legenda.Texto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("UsuarioProfissional1")]
        public ActionResult AbrirPerfilUsuarioProfissional()
        {
            UsuarioProfissional usuario = null;
            try
            {
                usuario = (UsuarioProfissional)Session["Usuario"];

                return View("PerfilUsuarioProfissional", usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("UsuarioParticular")]
        public ActionResult AbrirPerfilUsuarioParticular()
        {
            UsuarioParticular usuario = null;
            try
            {
                usuario = (UsuarioParticular)Session["Usuario"];

                return View("PerfilUsuarioParticular", usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Profissional")]
        public ActionResult AbrirPerfilPublicoProfissional(int id)
        {
            UsuarioProfissional profissional = null;
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            Usuario usuario = null;
            string masterName = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                if (usuario != null)
                {
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
                }
                else
                {
                    masterName = "_LayoutVisitante";
                }

                daoUsuarioProfissional = new DAOUsuarioProfissional();
                profissional = daoUsuarioProfissional.ObterUsuarioProfissional(id);
                return View("Profissional", masterName, profissional);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EnviarMensagem(MensagemViewModel mensagem)
        {
            DAOMensagem daoMensagem = null;
            Mensagem novaMensagem = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                novaMensagem = new Mensagem();
                novaMensagem.Remetente = usuario;
                novaMensagem.Destinatario.Id = Int32.Parse(mensagem.IdDestinatario);
                novaMensagem.Conteudo = mensagem.Conteudo;

                daoMensagem = new DAOMensagem();
                if (daoMensagem.IncluirMensagem(novaMensagem))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult SalvarAvaliacao(AvaliacaoViewModel avaliacao)
        {
            DAOAvaliacao daoAvaliacao = null;
            Avaliacao novaAvaliacao = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                novaAvaliacao = new Avaliacao();
                novaAvaliacao.Avaliador = usuario;
                novaAvaliacao.Avaliado.Id = Int32.Parse(avaliacao.IdAvaliado);
                novaAvaliacao.Resumo = avaliacao.Resumo;
                novaAvaliacao.Texto = avaliacao.Texto;
                novaAvaliacao.Nota = byte.Parse(avaliacao.Nota);

                daoAvaliacao = new DAOAvaliacao();
                if (daoAvaliacao.IncluirAvaliacao(novaAvaliacao))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarDadosGerais(DadosGerais dados)
        {
            DAOUsuario daoUsuario = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoUsuario = new DAOUsuario();
                daoUsuario.EditarNomeEmail(dados.Nome, dados.Email, usuario.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditarResumoProfissional(Info info)
        {
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoUsuarioProfissional = new DAOUsuarioProfissional();
                daoUsuarioProfissional.EditarResumo(info.Descricao, usuario.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ViewModels
        public class MensagemViewModel
        {
            private string _IdDestinatario;
            private string _Conteudo;

            public string IdDestinatario
            {
                get { return _IdDestinatario; }
                set { _IdDestinatario = value; }
            }

            public string Conteudo
            {
                get { return _Conteudo; }
                set { _Conteudo = value; }
            }
        }

        public class AvaliacaoViewModel
        {
            private string _IdAvaliado;
            private string _Resumo;
            private string _Texto;
            private string _Nota;
            
            public string IdAvaliado
            {
                get { return _IdAvaliado; }
                set { _IdAvaliado = value; }
            }

            public string Resumo
            {
                get { return _Resumo; }
                set { _Resumo = value; }
            }

            public string Texto
            {
                get { return _Texto; }
                set { _Texto = value; }
            }

            public string Nota
            {
                get { return _Nota; }
                set { _Nota = value; }
            }
        }

        public class DadosGerais
        {
            private string _Email;
            private string _Nome;

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
        }

        public class Info
        {
            private string _Descricao;

            public string Descricao
            {
                get { return _Descricao; }
                set { _Descricao = value; }
            }
        }

        public class LegendaViewModel
        {
            private string _Id;
            private string _Texto;

            public string Texto
            {
                get { return _Texto; }
                set { _Texto = value; }
            }

            public string Id
            {
                get { return _Id; }
                set { _Id = value; }
            }

        }
        #endregion
    }
}
