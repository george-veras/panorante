using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuFaco.DataAccess.DAOs;
using EuFaco.DataAccess.Models;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Specialized;
using EuFaco.ViewModel;

namespace EuFaco.Controllers
{
    public class PublicacoesController : Controller
    {
        public void RegistrarVisualizacaoImagem(string id)
        {
            DAOPublicacaoImagem daoPublicacaoImagem = null;
            int idPublicacaoImagem;
            try
            {
                if (Int32.TryParse(id, out idPublicacaoImagem))
                {
                    daoPublicacaoImagem = new DAOPublicacaoImagem();
                    daoPublicacaoImagem.IncrementarVisualizacoes(idPublicacaoImagem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Imagem")]
        public ActionResult AbrirPublicacaoImagem(int id)
        {
            Usuario usuario = null;
            DAOPublicacaoImagem daoPublicacaoImagem = null;
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            DAOComentarioPublicacao daoComentarioPublicacao = null;
            DAOEtiquetaPublicacao daoEtiquetaPublicacao = null;
            PublicacaoImagem publicacaoImagem = null;
            string masterName = null;
            try
            {
                daoPublicacaoImagem = new DAOPublicacaoImagem();
                publicacaoImagem = daoPublicacaoImagem.ObterPublicacaoImagem(id);

                daoUsuarioProfissional = new DAOUsuarioProfissional();
                publicacaoImagem.Autor = daoUsuarioProfissional.ObterUsuarioProfissional(publicacaoImagem.Autor.Id);

                daoComentarioPublicacao = new DAOComentarioPublicacao();
                daoPublicacaoImagem.PopularComentarios(publicacaoImagem);

                daoEtiquetaPublicacao = new DAOEtiquetaPublicacao();
                daoEtiquetaPublicacao.PopularEtiquetas(publicacaoImagem);

                usuario = (Usuario)Session["Usuario"];
                if (usuario == null)
                {
                    if (publicacaoImagem != null)
                        return View("PublicacaoImagemVisitante", "_LayoutVisitante", publicacaoImagem);
                    else
                        return Json("Deu ruim aqui"); // Tratar este caso.
                }

                switch (usuario.Perfil)
                {
                    case Usuario.TipoPerfil.UsuarioParticular:
                        masterName = "_LayoutUsuarioParticular";
                        break;
                    case Usuario.TipoPerfil.UsuarioProfissional:
                        masterName = "_LayoutUsuarioProfissional";
                        break;
                    default:
                        masterName = "_LayoutVisitante";
                        break;
                }

                if (publicacaoImagem != null)
                    return View("PublicacaoImagem", masterName, publicacaoImagem);
                else
                    return Json("Deu ruim aqui"); // Tratar esse caso.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Projeto")]
        public ActionResult AbrirPublicacaoProjeto(int id)
        {
            Usuario usuario = null;
            DAOPublicacaoProjeto daoPublicacaoProjeto = null;
            PublicacaoProjeto publicacaoProjeto = null;
            string masterName = null;
            try
            {
                daoPublicacaoProjeto = new DAOPublicacaoProjeto();
                publicacaoProjeto = daoPublicacaoProjeto.ObterPublicacaoProjeto(id);

                usuario = (Usuario)Session["Usuario"];
                if (usuario == null)
                {
                    if (publicacaoProjeto != null)
                        return View("PublicacaoProjetoVisitante", "_LayoutVisitante", publicacaoProjeto);
                    else
                        return Json("Deu ruim aqui"); // Tratar este caso.
                }

                switch (usuario.Perfil)
                {
                    case Usuario.TipoPerfil.UsuarioParticular:
                        masterName = "_LayoutUsuarioParticular";
                        break;
                    case Usuario.TipoPerfil.UsuarioProfissional:
                        masterName = "_LayoutUsuarioProfissional";
                        break;
                    default:
                        masterName = "_LayoutVisitante";
                        break;
                }

                if (publicacaoProjeto != null)
                    return View("PublicacaoProjeto", masterName, publicacaoProjeto);
                else
                    return Json("Deu ruim aqui"); // Tratar esse caso.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ComentarPublicacaoImagem(ComentarioPublicacaoViewModel comentario)
        {
            DAOComentarioPublicacao daoComentarioPublicacao = null;
            ComentarioPublicacao novoComentarioPublicacao = null;
            Usuario usuario = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                novoComentarioPublicacao = new ComentarioPublicacao();
                novoComentarioPublicacao.Texto = comentario.Texto;
                novoComentarioPublicacao.Autor = usuario;

                daoComentarioPublicacao = new DAOComentarioPublicacao();
                if (daoComentarioPublicacao.IncluirComentarioPublicacao(novoComentarioPublicacao))
                {
                    return Json(new
                    {
                        info = "true",
                        data = novoComentarioPublicacao.Id.ToString()
                    });
                }

                return Json(new
                {
                    info = "false",
                    data = ""
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("Publicacoes")]
        public ActionResult AbrirPublicacoes()
        {
            Usuario usuario = null;
            DAOPublicacao daoPublicacao = null;
            string masterName;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                daoPublicacao = new DAOPublicacao();
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
                return View("Publicacoes", masterName, daoPublicacao.ObterPublicacoes());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PublicarImagem()
        {
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (UsuarioProfissional)Session["Usuario"];

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
                return View("PublicarImagem", masterName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PublicarAntesDepois()
        {
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (UsuarioProfissional)Session["Usuario"];

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
                return View("PublicarAntesDepois", masterName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult PublicarProjeto()
        {
            Usuario usuario = null;
            string masterName;
            try
            {
                usuario = (UsuarioProfissional)Session["Usuario"];

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
                return View("PublicarProjeto", masterName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult SalvarPublicacaoImagem()
        {
            DAOPublicacaoImagem daoPublicacaoImagem = null;
            Imagem novaImagem = null;
            Usuario usuario = null;
            string pathImagem = null;
            PublicacaoImagem novaPublicacaoImagem = null;
            EtiquetaPublicacao etiqueta = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    HttpPostedFile postedFile = System.Web.HttpContext.Current.Request.Files["fileImagemPublicacao"];
                    Image imagem = Bitmap.FromStream(postedFile.InputStream);
                    pathImagem = @"\ImagensPublicacoes\" + postedFile.FileName;
                    imagem.Save(System.Web.HttpContext.Current.Server.MapPath("~") + @"\ImagensPublicacoes\" + postedFile.FileName);
                }
                else
                    throw new Exception();
                
                novaImagem = new Imagem();
                novaImagem.Dono = usuario;
                novaImagem.PathImagem = pathImagem;
                novaImagem.Legenda = System.Web.HttpContext.Current.Request.Form["txtLegenda"];

                novaPublicacaoImagem = new PublicacaoImagem();
                novaPublicacaoImagem.Autor = usuario;
                novaPublicacaoImagem.Imagem = novaImagem;
                
                string etiquetas = System.Web.HttpContext.Current.Request.Form["etiquetas"].ToString();
                foreach (string tagName in etiquetas.Split(','))
                {
                    etiqueta = new EtiquetaPublicacao();
                    etiqueta.Nome = tagName;
                    novaPublicacaoImagem.Etiquetas.Add(etiqueta);
                }

                daoPublicacaoImagem = new DAOPublicacaoImagem();
                if (daoPublicacaoImagem.IncluirPublicacaoImagem(novaPublicacaoImagem))
                    return Json("Aew salvou!");
                else
                    return Json("Deu ruim pra salvar a imagem hein...");
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public JsonResult SalvarPublicacaoAntesDepois()
        {
            DAOPublicacaoAntesDepois daoPublicacaoAntesDepois = null;
            Usuario usuario = null;
            string pathImagem1 = null;
            string pathImagem2 = null;
            PublicacaoAntesDepois novaPublicacaoAntesDepois = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    HttpPostedFile postedFile1 = System.Web.HttpContext.Current.Request.Files["imagemAntes"];
                    Image imagem1 = Bitmap.FromStream(postedFile1.InputStream);
                    pathImagem1 = @"\ImagensPublicacoes\" + postedFile1.FileName;
                    imagem1.Save(System.Web.HttpContext.Current.Server.MapPath("~") + @"\ImagensPublicacoes\" + postedFile1.FileName);

                    HttpPostedFile postedFile2 = System.Web.HttpContext.Current.Request.Files["imagemDepois"];
                    Image imagem2 = Bitmap.FromStream(postedFile2.InputStream);
                    pathImagem2 = @"\ImagensPublicacoes\" + postedFile2.FileName;
                    imagem2.Save(System.Web.HttpContext.Current.Server.MapPath("~") + @"\ImagensPublicacoes\" + postedFile2.FileName);
                }
                else
                    throw new Exception();

                novaPublicacaoAntesDepois = new PublicacaoAntesDepois();
                novaPublicacaoAntesDepois.Autor = usuario;
                novaPublicacaoAntesDepois.PathImagemAntes = pathImagem1;
                novaPublicacaoAntesDepois.PathImagemDepois = pathImagem2;
                novaPublicacaoAntesDepois.Titulo = System.Web.HttpContext.Current.Request.Form["txtTitulo"];
                novaPublicacaoAntesDepois.Texto = System.Web.HttpContext.Current.Request.Form["txtTexto"];
                //novaPublicacaoAntesDepois.Etiquetas.AddRange(System.Web.HttpContext.Current.Request.Form["tags"].ToString().Split(','));

                daoPublicacaoAntesDepois = new DAOPublicacaoAntesDepois();
                if (daoPublicacaoAntesDepois.IncluirPublicacaoAntesDepois(novaPublicacaoAntesDepois))
                    return Json("Aew salvou!");
                else
                    return Json("Deu ruim pra salvar a imagem hein...");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SalvarPublicacaoProjeto()
        {
            DAOPublicacaoProjeto daoPublicacaoProjeto = null;
            Usuario usuario = null;
            PublicacaoProjeto novaPublicacaoProjeto = null;
            string pathImagem;
            string pathImagemCapa;
            ItemPublicacao itemPublicacao = null;
            HttpPostedFile postedFile = null;
            Image imagem = null;
            EtiquetaPublicacao etiqueta = null;
            try
            {
                usuario = (Usuario)Session["Usuario"];

                novaPublicacaoProjeto = new PublicacaoProjeto();
                novaPublicacaoProjeto.Autor = usuario;
                novaPublicacaoProjeto.Titulo = System.Web.HttpContext.Current.Request.Form["txtTitulo"];

                postedFile = System.Web.HttpContext.Current.Request.Files["fileImagemCapa"];
                imagem = Bitmap.FromStream(postedFile.InputStream);
                pathImagemCapa = @"\ImagensPublicacoes\" + postedFile.FileName;
                imagem.Save(System.Web.HttpContext.Current.Server.MapPath("~") + @"\ImagensPublicacoes\" + postedFile.FileName);
                novaPublicacaoProjeto.PathImagemCapa = pathImagemCapa;

                int postedFilePosition;
                foreach (string elementName in Request.Form.AllKeys)
                {
                    if (elementName.StartsWith("txtParagrafo"))
                    {
                        itemPublicacao = new ItemPublicacao("Paragrafo", Request.Form[elementName]);
                        novaPublicacaoProjeto.Conteudo.Add(itemPublicacao);
                    }
                    else if (elementName.StartsWith("hiddenImagePosition"))
                    {
                        postedFilePosition = Int32.Parse(Request.Form[elementName]);
                        postedFile = System.Web.HttpContext.Current.Request.Files[postedFilePosition];
                        imagem = Bitmap.FromStream(postedFile.InputStream);
                        pathImagem = @"\ImagensPublicacoes\" + postedFile.FileName;
                        imagem.Save(System.Web.HttpContext.Current.Server.MapPath("~") + @"\ImagensPublicacoes\" + postedFile.FileName);
                        itemPublicacao = new ItemPublicacao("Imagem", pathImagem);
                        novaPublicacaoProjeto.Conteudo.Add(itemPublicacao);
                    }
                }

                string etiquetas = System.Web.HttpContext.Current.Request.Form["etiquetas"].ToString();
                foreach (string tagName in etiquetas.Split(','))
                {
                    etiqueta = new EtiquetaPublicacao();
                    etiqueta.Nome = tagName;
                    novaPublicacaoProjeto.Etiquetas.Add(etiqueta);
                }

                daoPublicacaoProjeto = new DAOPublicacaoProjeto();
                if (daoPublicacaoProjeto.IncluirPublicacaoProjeto(novaPublicacaoProjeto))
                    return Json("Aew salvou!");
                else
                    return Json("Deu ruim pra salvar a imagem hein...");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}