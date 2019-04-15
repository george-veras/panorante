using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using EuFaco.DataAccess.DAOs;
using EuFaco.DataAccess.Models;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace EuFaco.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /CreateProfile/

        public ActionResult Index()
        {
            DAOUnidadeFederativa daoUnidadeFederativa = null;
            DAOPublicacao daoPublicacao = null;
            HomePageViewModel homeViewModel = null;
            try
            {
                homeViewModel = new HomePageViewModel();

                daoPublicacao = new DAOPublicacao();
                homeViewModel.Publicacoes = daoPublicacao.ListarPublicacoes();
                                
                //daoUnidadeFederativa = new DAOUnidadeFederativa();
                //homeViewModel.UFs = daoUnidadeFederativa.ListarUnidadesFederativas();

                return View("NovaHome", "_LayoutVisitante", homeViewModel);
                //return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult LogarPeloFacebook(FacebookAccountViewModel facebookLogin)
        {
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            UsuarioProfissional usuarioProfissional = null;
            string urlGetOpenGraphAccesToken;
            HttpWebRequest accessTokenRequest = null;
            HttpWebResponse accessTokenResponse = null;
            string accessTokenResponseText = null;
            string accessToken = null;
            string urlCheckUserAccessToken;
            string checkAccessTokenResponseText;
            DAOFacebookAccount daoFacebookAccount = null;
            FacebookAccount facebookAccount = null;
            DAOUsuario daoUsuario = null;
            Usuario usuario = null;
            DAOUsuarioParticular daoUsuarioParticular = null;
            UsuarioParticular usuarioParticular = null;
            try
            {
                urlGetOpenGraphAccesToken = @"https://graph.facebook.com/oauth/access_token?type=client_cred&client_id=1753687224853096&client_secret=6af778d15f3ce0ffa2b4e1acdcede3a9";
                accessTokenRequest = (HttpWebRequest)WebRequest.Create(urlGetOpenGraphAccesToken);
                accessTokenResponse = (HttpWebResponse)accessTokenRequest.GetResponse();
                using (var accessTokenResponseReader = new System.IO.StreamReader(accessTokenResponse.GetResponseStream(), Encoding.UTF8))
                {
                    accessTokenResponseText = accessTokenResponseReader.ReadToEnd();
                }
                accessToken = accessTokenResponseText.Substring(13); //Buscar uma maneira melhor de fazer isto (extrair o token da string).

                urlCheckUserAccessToken = String.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", facebookLogin.AccessToken, accessToken);
                HttpWebRequest checkUserAccessTokenRequest = (HttpWebRequest)WebRequest.Create(urlCheckUserAccessToken);
                HttpWebResponse checkUserAccessTokenResponse = (HttpWebResponse)checkUserAccessTokenRequest.GetResponse();
                if (checkUserAccessTokenResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var checkAccessTokenResponseReader = new System.IO.StreamReader(checkUserAccessTokenResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        checkAccessTokenResponseText = checkAccessTokenResponseReader.ReadToEnd();
                    }

                    Newtonsoft.Json.Linq.JObject jsonCheckAccessTokenResponse = JsonConvert.DeserializeObject(checkAccessTokenResponseText) as Newtonsoft.Json.Linq.JObject;
                    if (jsonCheckAccessTokenResponse["data"]["is_valid"].ToString() == "True")
                    {
                        if (jsonCheckAccessTokenResponse["data"]["app_id"].ToString() == "1753687224853096")
                        {
                            if (jsonCheckAccessTokenResponse["data"]["user_id"].ToString() == facebookLogin.Id)
                            {
                                daoFacebookAccount = new DAOFacebookAccount();
                                // Tenta obter um "FacebookAccount" no banco de dados, checando se é um usuário retornante.
                                facebookAccount = daoFacebookAccount.ObterFacebookAccount(facebookLogin.Id);
                                if (facebookAccount != null) // Se o "facebookAccount" entrar na condição de "não nulo", ou seja, foi encontrada uma instância com o mesmo "FacebookId" no banco de dados, o usuário em questão é considerado retornante, não é a primeira vez que ele loga via Facebook.
                                {
                                    // Obtém o complemento dos dados, caso o usuário seja um particular (obtém-se na tabela "UsuariosParticulares").
                                    if (facebookAccount.Usuario.Perfil == Usuario.TipoPerfil.UsuarioParticular)
                                    {
                                        daoUsuarioParticular = new DAOUsuarioParticular();
                                        usuarioParticular = daoUsuarioParticular.ObterUsuarioParticular(facebookAccount.Usuario.Id);
                                        usuarioParticular.Facebook = facebookAccount;
                                        facebookAccount.Usuario = usuarioParticular;

                                        Session["Usuario"] = usuarioProfissional;

                                        return Json(new
                                        {
                                            info = "redirect",
                                            data = Url.Action("UsuarioParticular", "Perfil")
                                        });
                                    } // Obtém o complemento dos dados, caso o usuário seja um profissional (obtém-se na tabela "UsuáriosProfissionais").
                                    else if (facebookAccount.Usuario.Perfil == Usuario.TipoPerfil.UsuarioProfissional)
                                    {
                                        daoUsuarioProfissional = new DAOUsuarioProfissional();
                                        usuarioProfissional = daoUsuarioProfissional.ObterUsuarioProfissional(facebookAccount.Usuario.Id);
                                        usuarioProfissional.Facebook = facebookAccount;
                                        facebookAccount.Usuario = usuarioProfissional;

                                        Session["Usuario"] = usuarioProfissional;

                                        return Json(new
                                        {
                                            info = "redirect",
                                            data = Url.Action("UsuarioProfissional", "Perfil")
                                        });
                                    }
                                }
                                else // Neste caso, não foi encontrado nenhum registro no banco com este "FacebookId", assumindo que este usuário entrou pelo Facebook pela primeira vez.
                                {
                                    daoUsuario = new DAOUsuario();
                                    // Busca-se por um registro de usuário com o e-mail retornado pelo Facebook, para descobrir se esta pessoa já tinha um cadastro tradicional (entrava com e-mail e senha anteriormente).
                                    usuario = daoUsuario.ObterUsuarioPorEmail(facebookLogin.Email);
                                    if (usuario != null)
                                    {
                                        // Neste caso, apesar de o usuário ter logado pela primeira vez via Facebook, o e-mail que o Facebook retornou já consta em um perfil na tabela "Usuarios" (ele já havia logado via e-mail/senha tradicionalmente).
                                        // Este usuário deve ser informado disto e informar a senha referente ao e-mail encontrado para poder prosseguir no site.
                                        Session["Usuario"] = usuario;

                                        return Json(new
                                        {
                                            info = "exigirSenhaEmailEncontrado"
                                        });
                                    }
                                    else
                                    {
                                        // Neste caso, o usuário será tratado como um usuário inédito, ou seja, é a primeira vez que o mesmo logou no sistema e escolheu o fazer via Facebook, pois o seu "FaceBookId" não consta na tabela "FacebookAccounts" e seu e-mail obtido do Facebook também não consta na tabela de "Usuarios". Este usuário será incluído com o perfil "Indefinido" (temporariamente), e perguntado com qual tipo de perfil ele deseja prosseguir.
                                        usuario = new Usuario();
                                        usuario.Nome = facebookLogin.Nome;
                                        usuario.Email = facebookLogin.Email;
                                        usuario.Facebook = new FacebookAccount();
                                        usuario.Facebook.FacebookId = facebookLogin.Id;
                                        usuario.Facebook.Nome = facebookLogin.Nome;
                                        usuario.Facebook.Email = facebookLogin.Email;
                                        usuario.Facebook.AccessToken = facebookLogin.AccessToken;
                                        usuario.Perfil = Usuario.TipoPerfil.PerfilIndefinido;
                                        if (daoUsuario.IncluirUsuarioComFacebook(usuario))
                                        {
                                            Session["Usuario"] = usuario;
                                        }
                                        return Json(new
                                        {
                                            info = "solicitarDefinicaoPerfil"
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                return Json(new
                {
                    info = "naoDefinido"
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult DefinirPerfil(string tipoPerfil)
        {
            Usuario usuario = (Usuario)Session["Usuario"];
            if (usuario.Perfil == Usuario.TipoPerfil.PerfilIndefinido)
            {
                if (tipoPerfil == "Particular")
                {
                    DAOUsuarioParticular daoUsuarioParticular = new DAOUsuarioParticular();
                    UsuarioParticular usuarioParticular = daoUsuarioParticular.IncluirComplementoUsuarioParticular(usuario);
                    if (usuarioParticular != null)
                    {
                        Session["Usuario"] = usuarioParticular;
                        return Json(new
                        {
                            info = "redirect",
                            data = Url.Action("UsuarioParticular", "Perfil")
                        });
                    }
                }
                else if (tipoPerfil == "Profissional")
                {
                    DAOUsuarioProfissional daoUsuarioProfissional = new DAOUsuarioProfissional();
                    UsuarioProfissional usuarioProfissional = daoUsuarioProfissional.IncluirComplementoUsuarioProfissional(usuario);
                    if (usuarioProfissional != null)
                    {
                        Session["Usuario"] = usuarioProfissional;
                        return Json(new
                        {
                            info = "redirect",
                            data = Url.Action("UsuarioProfissional", "Perfil")
                        });
                    }
                }
            }
            return new JsonResult();
        }

        public JsonResult ConfirmarUsuario(string senha)
        {
            Usuario usuario = (Usuario)Session["Usuario"];
            if (usuario.Senha == senha)
            {
                if (usuario.Perfil == Usuario.TipoPerfil.UsuarioParticular)
                {
                    DAOUsuarioParticular daoUsuarioParticular = new DAOUsuarioParticular();
                    UsuarioParticular usuarioParticular = daoUsuarioParticular.CompletarPerfilUsuarioParticular(usuario);
                    if (usuarioParticular != null)
                    {
                        Session["Usuario"] = usuarioParticular;
                        return Json(new
                        {
                            info = "redirect",
                            data = Url.Action("UsuarioParticular", "Perfil")
                        });
                    }
                }
                else if (usuario.Perfil == Usuario.TipoPerfil.UsuarioProfissional)
                {
                    DAOUsuarioProfissional daoUsuarioProfissional = new DAOUsuarioProfissional();
                    UsuarioProfissional usuarioProfissional = daoUsuarioProfissional.CompletarPerfilUsuarioProfissional(usuario);
                    if (usuarioProfissional != null)
                    {
                        Session["Usuario"] = usuarioProfissional;
                        return Json(new
                        {
                            info = "redirect",
                            data = Url.Action("UsuarioProfissional", "Perfil")
                        });
                    }
                }
            }
            return new JsonResult();
        }

        //[HttpPost]
        public ActionResult CadastrarUsuarioProfissional(CadastroViewModel novoUsuario)
        {
            DAOUsuarioProfissional daoUsuarioProfissional = null;
            UsuarioProfissional usuarioProfissional = null;
            try
            {
                daoUsuarioProfissional = new DAOUsuarioProfissional();

                usuarioProfissional = new UsuarioProfissional();
                usuarioProfissional.Nome = novoUsuario.Nome;
                usuarioProfissional.Sobrenome = novoUsuario.Sobrenome;
                usuarioProfissional.Email = novoUsuario.Email;
                usuarioProfissional.Senha = novoUsuario.Senha;
                //usuarioProfissional.DataNascimento = novoUsuario.DataNascimento;
                //usuarioProfissional.Sexo = novoUsuario.Sexo;
                //usuarioProfissional.Perfil = Usuario.TipoPerfil.UsuarioProfissional;

                if (daoUsuarioProfissional.IncluirUsuarioProfissional(usuarioProfissional))
                {
                    Session["Usuario"] = usuarioProfissional;
                }
                
                return Json(new
                {
                    info = "redirect",
                    data = Url.Action("UsuarioProfissional", "Perfil")
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CadastrarUsuarioParticular(CadastroViewModel novoUsuario)
        {
            DAOUsuarioParticular daoUsuarioParticular = null;
            UsuarioParticular usuarioParticular = null;
            try
            {
                daoUsuarioParticular = new DAOUsuarioParticular();

                usuarioParticular = new UsuarioParticular();
                usuarioParticular.Nome = novoUsuario.Nome;
                usuarioParticular.Email = novoUsuario.Email;
                usuarioParticular.Senha = novoUsuario.Senha;

                if (daoUsuarioParticular.IncluirUsuarioParticular(usuarioParticular))
                {
                    Session["Usuario"] = usuarioParticular;
                }

                return Json(new
                {
                    info = "redirect",
                    data = Url.Action("UsuarioParticular", "Perfil")
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ActionResult LogarUsuarioProfissional(Login login)
        {
            DAOUsuarioProfissional daoUsuarioProfissional;
            UsuarioProfissional usuario;
            try
            {
                daoUsuarioProfissional = new DAOUsuarioProfissional();
                usuario = daoUsuarioProfissional.ObterUsuarioProfissional(login.Email, login.Senha);

                if (usuario != null)
                {
                    Session["Usuario"] = usuario;
                    return Json(Url.Action("UsuarioProfissional", "Perfil"));
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LogarUsuario(Login login)
        {
            Usuario usuario;
            UsuarioParticular usuarioParticular;
            UsuarioProfissional usuarioProfissional;
            DAOUsuario daoUsuario;
            DAOUsuarioParticular daoUsuarioParticular;
            DAOUsuarioProfissional daoUsuarioProfissional;
            string actionName;
            try
            {
                actionName = null;
                daoUsuario = new DAOUsuario();
                usuario = daoUsuario.ObterUsuario(login.Email, login.Senha);

                if (usuario != null)
                {
                    switch (usuario.Perfil)
                    {
                        case Usuario.TipoPerfil.UsuarioParticular:
                            daoUsuarioParticular = new DAOUsuarioParticular();
                            usuarioParticular = daoUsuarioParticular.CompletarPerfilUsuarioParticular(usuario);
                            Session["Usuario"] = usuarioParticular;
                            actionName = "UsuarioParticular";
                            break;
                        case Usuario.TipoPerfil.UsuarioProfissional:
                            daoUsuarioProfissional = new DAOUsuarioProfissional();
                            usuarioProfissional = daoUsuarioProfissional.CompletarPerfilUsuarioProfissional(usuario);
                            Session["Usuario"] = usuarioProfissional;
                            actionName = "UsuarioProfissional";
                            break;
                        default:
                            actionName = String.Empty;
                            break;
                    }
                }

                return Json(new
                {
                    info = "redirect",
                    data = Url.Action(actionName, "Perfil")
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LogarUsuarioParticular(Login login)
        {
            DAOUsuarioParticular daoUsuarioParticular;
            UsuarioParticular usuario;
            try
            {
                daoUsuarioParticular = new DAOUsuarioParticular();
                usuario = daoUsuarioParticular.ObterUsuarioParticular(login.Email, login.Senha);

                if (usuario != null)
                {
                    Session["Usuario"] = usuario;
                    return Json(Url.Action("UsuarioParticular", "Perfil"));
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EnviarSenhaPorEmail(string email)
        {
            try
            {
                //SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
                //SmtpClient client = new SmtpClient("pop-mail.outlook.com");
                //SmtpClient client = new SmtpClient("mail.live.com");
                SmtpClient client = new SmtpClient("smtp.live.com");
                client.Credentials = new NetworkCredential("georgerushman@hotmail.com", "041085");
                client.Port = 587;
                //client.Port = 25;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("georgerushman@hotmail.com");
                mailMessage.To.Add("george.veras.valentim@gmail.com");
                mailMessage.Subject = "Hello There";
                mailMessage.Body = "Hello my friend!";

                client.Send(mailMessage);
                //client.SendMailAsync(mailMessage);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class Login
        {
            private string _Email;
            private string _Senha;

            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }
            
            public string Senha
            {
                get { return _Senha; }
                set { _Senha = value; }
            }
        }

        public class FacebookAccountViewModel
        {
            private string _Id;
            private string _Nome;
            private string _Email;
            private string _AccessToken;

            public string Id
            {
                get
                {
                    return _Id;
                }

                set
                {
                    _Id = value;
                }
            }

            public string Nome
            {
                get
                {
                    return _Nome;
                }

                set
                {
                    _Nome = value;
                }
            }

            public string Email
            {
                get
                {
                    return _Email;
                }

                set
                {
                    _Email = value;
                }
            }

            public string AccessToken
            {
                get
                {
                    return _AccessToken;
                }

                set
                {
                    _AccessToken = value;
                }
            }
        }

        public class CadastroViewModel
        {
            private string _Nome;
            private string _Sobrenome;
            private string _Email;
            private string _Senha;
            private string _ConfirmacaoSenha;
            private string _Sexo;
            private DateTime _DataNascimento;

            public string Nome
            {
                get { return _Nome; }
                set { _Nome = value; }
            }

            public string Sobrenome
            {
                get { return _Sobrenome; }
                set { _Sobrenome = value; }
            }

            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            public string Senha
            {
                get { return _Senha; }
                set { _Senha = value; }
            }

            public string ConfirmacaoSenha
            {
                get
                {
                    return _ConfirmacaoSenha;
                }

                set
                {
                    _ConfirmacaoSenha = value;
                }
            }

            public string Sexo
            {
                get { return _Sexo; }
                set { _Sexo = value; }
            }

            public DateTime DataNascimento
            {
                get { return _DataNascimento; }
                set { _DataNascimento = value; }
            }
        }

        public class HomePageViewModel
        {
            private List<UnidadeFederativa> _UFs;
            private List<Publicacao> _Publicacoes;

            public List<UnidadeFederativa> UFs
            {
                get
                {
                    return _UFs;
                }

                set
                {
                    _UFs = value;
                }
            }

            public List<Publicacao> Publicacoes
            {
                get
                {
                    return _Publicacoes;
                }

                set
                {
                    _Publicacoes = value;
                }
            }

            public HomePageViewModel()
            {
                
            }
        }

        public class Cadastro
        {
            private string _Nome;
            private string _Sobrenome;
            private string _Email;
            private string _Senha;
            private string _Localidade;
            private string _Sexo;
            private DateTime _DataNascimento;

            public DateTime DataNascimento
            {
                get { return _DataNascimento; }
                set { _DataNascimento = value; }
            }

            public string Sexo
            {
                get { return _Sexo; }
                set { _Sexo = value; }
            }

            public string Localidade
            {
                get { return _Localidade; }
                set { _Localidade = value; }
            }

            public string Nome
            {
                get { return _Nome; }
                set { _Nome = value; }
            }

            public string Sobrenome
            {
                get { return _Sobrenome; }
                set { _Sobrenome = value; }
            }

            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            public string Senha
            {
                get { return _Senha; }
                set { _Senha = value; }
            }
        }
    }

    public class ViewModelNovoUsuarioParticular
    {
        private string _Nome;
        private string _Email;
        private string _Senha;
        
        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Senha
        {
            get { return _Senha; }
            set { _Senha = value; }
        }
    }
}
