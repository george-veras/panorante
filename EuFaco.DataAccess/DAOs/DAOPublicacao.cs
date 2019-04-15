using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOPublicacao
    {
        public void PopularPublicacoes(UsuarioProfissional autor)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            PublicacaoImagem publicacaoImagem = null;
            PublicacaoProjeto publicacaoProjeto = null;
            PublicacaoAntesDepois publicacaoAntesDepois = null;
            Imagem imagem = null;
            try
            {
                autor.Publicacoes = new List<Publicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    P.Id, ");
                query.Append("    P.IdTipo, ");
                query.Append("    P.DataHoraPublicado, ");
                query.Append("    P.IdUsuario, ");
                query.Append("    PP.Titulo AS TituloProjeto, ");
                query.Append("    PP.Conteudo AS ConteudoPublicacaoProjeto, ");
                query.Append("    PAD.Titulo AS TituloAntesDepois, ");
                query.Append("    PAD.Texto AS TextoAntesDepois, ");
                query.Append("    PAD.PathImagemAntes, ");
                query.Append("    PAD.PathImagemDepois, ");
                query.Append("    PIM.IdImagem, ");
                query.Append("    IM.Legenda AS LegendaImagem, ");
                query.Append("    IM.DataHoraInclusao AS DataHoraInclusaoImagem, ");
                query.Append("    IM.PathImagem ");
                query.Append("FROM Publicacoes P ");
                query.Append("    LEFT JOIN PublicacoesProjeto PP ON P.Id = PP.Id ");
                query.Append("    LEFT JOIN PublicacoesAntesDepois PAD ON P.Id = PAD.Id ");
                query.Append("    LEFT JOIN PublicacoesImagem PIM ON P.Id = PIM.Id ");
                query.Append("    LEFT JOIN Imagens IM ON PIM.IdImagem = IM.Id ");
                query.Append("WHERE P.IdUsuario = @IdUsuario");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdUsuario", autor.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    switch ((Publicacao.TipoPublicacao)dataReader["IdTipo"])
                                    {
                                        case Publicacao.TipoPublicacao.PublicacaoImagem:
                                            publicacaoImagem = new PublicacaoImagem();
                                            publicacaoImagem.Id = (int)dataReader["Id"];
                                            publicacaoImagem.Autor = autor;
                                            publicacaoImagem.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];

                                            imagem = new Imagem();
                                            imagem.Id = (int)dataReader["IdImagem"];
                                            imagem.Legenda = dataReader["LegendaImagem"].ToString();
                                            imagem.PathImagem = dataReader["PathImagem"].ToString();
                                            imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusaoImagem"];
                                            publicacaoImagem.Imagem = imagem;

                                            autor.Publicacoes.Add(publicacaoImagem);
                                            break;
                                        case Publicacao.TipoPublicacao.PublicacaoProjeto:
                                            publicacaoProjeto = new PublicacaoProjeto();
                                            publicacaoProjeto.Id = (int)dataReader["Id"];
                                            publicacaoProjeto.Autor = autor;
                                            publicacaoProjeto.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                            publicacaoProjeto.Titulo = dataReader["TituloProjeto"].ToString();
                                            publicacaoProjeto.Conteudo = JsonConvert.DeserializeObject<List<ItemPublicacao>>(dataReader["ConteudoPublicacaoProjeto"].ToString());

                                            autor.Publicacoes.Add(publicacaoProjeto);
                                            break;
                                        case Publicacao.TipoPublicacao.PublicacaoAntesDepois:
                                            publicacaoAntesDepois = new PublicacaoAntesDepois();
                                            publicacaoAntesDepois.Id = (int)dataReader["Id"];
                                            publicacaoAntesDepois.Autor = autor;
                                            publicacaoAntesDepois.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                            publicacaoAntesDepois.Titulo = dataReader["TituloAntesDepois"].ToString();
                                            publicacaoAntesDepois.Texto = dataReader["TextoAntesDepois"].ToString();
                                            publicacaoAntesDepois.PathImagemAntes = dataReader["PathImagemAntes"].ToString();
                                            publicacaoAntesDepois.PathImagemDepois = dataReader["PathImagemDepois"].ToString();

                                            autor.Publicacoes.Add(publicacaoAntesDepois);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public List<Publicacao> ListarPublicacoes()
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Usuario autor = null;
            Imagem imagem = null;
            PublicacaoImagem publicacaoImagem = null;
            PublicacaoProjeto publicacaoProjeto = null;
            PublicacaoAntesDepois publicacaoAntesDepois = null;
            List<Publicacao> listaPublicacoes = null;
            try
            {
                listaPublicacoes = new List<Publicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    P.Id, ");
                query.Append("    P.IdTipo, ");
                query.Append("    P.DataHoraPublicado, ");
                query.Append("    P.IdUsuario AS IdAutorPublicacao, ");
                query.Append("    U.Nome AS NomeAutor, ");
                query.Append("    U.Email AS EmailAutor, ");
                query.Append("    PI.IdImagem, ");
                query.Append("    I.Legenda AS LegendaImagem, ");
                query.Append("    I.DataHoraInclusao, ");
                query.Append("    I.PathImagem, ");
                query.Append("    PP.Titulo AS TituloProjeto, ");
                query.Append("    PP.PathImagemCapa AS PathImagemCapaProjeto, ");
                query.Append("    PP.Conteudo AS ConteudoPublicacaoProjeto, ");
                query.Append("    PAD.PathImagemAntes, ");
                query.Append("    PAD.PathImagemDepois, ");
                query.Append("    PAD.Titulo AS TituloAntesDepois, ");
                query.Append("    PAD.Texto AS TextoAntesDepois ");
                query.Append("FROM Publicacoes P ");
                query.Append("    LEFT JOIN Usuarios U ON P.Id = U.Id ");
                query.Append("    LEFT JOIN PublicacoesImagem PI ON P.Id = PI.Id ");
                query.Append("    LEFT JOIN Imagens I ON PI.IdImagem = I.Id ");
                query.Append("    LEFT JOIN PublicacoesProjeto PP ON P.Id = PP.Id ");
                query.Append("    LEFT JOIN PublicacoesAntesDepois PAD ON P.Id = PAD.Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            listaPublicacoes = new List<Publicacao>();
                            while (dataReader.Read())
                            {
                                switch ((int)dataReader["IdTipo"])
                                {
                                    case (int)Publicacao.TipoPublicacao.PublicacaoImagem:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        imagem = new Imagem();
                                        imagem.Id = (int)dataReader["IdImagem"];
                                        imagem.Legenda = dataReader["LegendaImagem"].ToString();
                                        imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusao"];
                                        imagem.PathImagem = dataReader["PathImagem"].ToString();
                                        imagem.Dono = autor;

                                        publicacaoImagem = new PublicacaoImagem();
                                        publicacaoImagem.Id = (int)dataReader["Id"];
                                        publicacaoImagem.Autor = autor;
                                        publicacaoImagem.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoImagem.Imagem = imagem;

                                        listaPublicacoes.Add(publicacaoImagem);
                                        break;
                                    case (int)Publicacao.TipoPublicacao.PublicacaoProjeto:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        publicacaoProjeto = new PublicacaoProjeto();
                                        publicacaoProjeto.Id = (int)dataReader["Id"];
                                        publicacaoProjeto.Autor = autor;
                                        publicacaoProjeto.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoProjeto.Titulo = dataReader["TituloProjeto"].ToString();
                                        publicacaoProjeto.PathImagemCapa = dataReader["PathImagemCapaProjeto"].ToString();
                                        publicacaoProjeto.Conteudo = JsonConvert.DeserializeObject<List<ItemPublicacao>>(dataReader["ConteudoPublicacaoProjeto"].ToString());

                                        listaPublicacoes.Add(publicacaoProjeto);
                                        break;
                                    case (int)Publicacao.TipoPublicacao.PublicacaoAntesDepois:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        publicacaoAntesDepois = new PublicacaoAntesDepois();
                                        publicacaoAntesDepois.Id = (int)dataReader["Id"];
                                        publicacaoAntesDepois.Autor = autor;
                                        publicacaoAntesDepois.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoAntesDepois.PathImagemAntes = dataReader["PathImagemAntes"].ToString();
                                        publicacaoAntesDepois.PathImagemDepois = dataReader["PathImagemDepois"].ToString();
                                        publicacaoAntesDepois.Titulo = dataReader["TituloAntesDepois"].ToString();
                                        publicacaoAntesDepois.Texto = dataReader["TextoAntesDepois"].ToString();

                                        listaPublicacoes.Add(publicacaoAntesDepois);
                                        break;
                                }
                            }
                        }
                    }
                }

                return listaPublicacoes;
            }
            finally
            {
                if (query != null)
                    query = null;
            }
        }

        public List<Publicacao> ObterPublicacoes()
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Usuario autor = null;
            Imagem imagem = null;
            PublicacaoImagem publicacaoImagem = null;
            PublicacaoProjeto publicacaoProjeto = null;
            PublicacaoAntesDepois publicacaoAntesDepois = null;
            List<Publicacao> listaPublicacoes = null;
            try
            {
                listaPublicacoes = new List<Publicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    P.Id, ");
                query.Append("    P.IdTipo, ");
                query.Append("    P.DataHoraPublicado, ");
                query.Append("    P.IdUsuario AS IdAutorPublicacao, ");
                query.Append("    U.Nome AS NomeAutor, ");
                query.Append("    U.Email AS EmailAutor, ");
                query.Append("    PI.IdImagem, ");
                query.Append("    I.Legenda AS LegendaImagem, ");
                query.Append("    I.DataHoraInclusao, ");
                query.Append("    I.PathImagem, ");
                query.Append("    PP.Titulo AS TituloProjeto, ");
                query.Append("    PP.Conteudo AS ConteudoPublicacaoProjeto, ");
                query.Append("    PAD.PathImagemAntes, ");
                query.Append("    PAD.PathImagemDepois, ");
                query.Append("    PAD.Titulo AS TituloAntesDepois, ");
                query.Append("    PAD.Texto AS TextoAntesDepois ");
                query.Append("FROM Publicacoes P ");
                query.Append("    LEFT JOIN Usuarios U ON P.Id = U.Id ");
                query.Append("    LEFT JOIN PublicacoesImagem PI ON P.Id = PI.Id ");
                query.Append("    LEFT JOIN Imagens I ON PI.IdImagem = I.Id ");
                query.Append("    LEFT JOIN PublicacoesProjeto PP ON P.Id = PP.Id ");
                query.Append("    LEFT JOIN PublicacoesAntesDepois PAD ON P.Id = PAD.Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            listaPublicacoes = new List<Publicacao>();
                            while (dataReader.Read())
                            {
                                switch ((int)dataReader["IdTipo"])
                                {
                                    case (int)Publicacao.TipoPublicacao.PublicacaoImagem:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        imagem = new Imagem();
                                        imagem.Id = (int)dataReader["IdImagem"];
                                        imagem.Legenda = dataReader["LegendaImagem"].ToString();
                                        imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusao"];
                                        imagem.PathImagem = dataReader["PathImagem"].ToString();
                                        imagem.Dono = autor;

                                        publicacaoImagem = new PublicacaoImagem();
                                        publicacaoImagem.Id = (int)dataReader["Id"];
                                        publicacaoImagem.Autor = autor;
                                        publicacaoImagem.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoImagem.Imagem = imagem;

                                        listaPublicacoes.Add(publicacaoImagem);
                                        break;
                                    case (int)Publicacao.TipoPublicacao.PublicacaoProjeto:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        publicacaoProjeto = new PublicacaoProjeto();
                                        publicacaoProjeto.Id = (int)dataReader["Id"];
                                        publicacaoProjeto.Autor = autor;
                                        publicacaoProjeto.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoProjeto.Titulo = dataReader["TituloProjeto"].ToString();
                                        publicacaoProjeto.Conteudo = JsonConvert.DeserializeObject<List<ItemPublicacao>>(dataReader["ConteudoPublicacaoProjeto"].ToString());

                                        listaPublicacoes.Add(publicacaoProjeto);
                                        break;
                                    case (int)Publicacao.TipoPublicacao.PublicacaoAntesDepois:
                                        autor = new Usuario();
                                        autor.Id = (int)dataReader["IdAutorPublicacao"];
                                        autor.Nome = dataReader["NomeAutor"].ToString();
                                        autor.Email = dataReader["EmailAutor"].ToString();

                                        publicacaoAntesDepois = new PublicacaoAntesDepois();
                                        publicacaoAntesDepois.Id = (int)dataReader["Id"];
                                        publicacaoAntesDepois.Autor = autor;
                                        publicacaoAntesDepois.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                        publicacaoAntesDepois.PathImagemAntes = dataReader["PathImagemAntes"].ToString();
                                        publicacaoAntesDepois.PathImagemDepois = dataReader["PathImagemDepois"].ToString();
                                        publicacaoAntesDepois.Titulo = dataReader["TituloAntesDepois"].ToString();
                                        publicacaoAntesDepois.Texto = dataReader["TextoAntesDepois"].ToString();

                                        listaPublicacoes.Add(publicacaoAntesDepois);
                                        break;
                                }
                            }
                        }
                    }
                }

                return listaPublicacoes;
            }
            finally
            {
                if (query != null)
                    query = null;
            }
        }
    }
}
