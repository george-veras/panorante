using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOPublicacaoImagem
    {
        public void IncrementarVisualizacoes(int idPublicacaoImagem)
        {
            string connectionString;
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            try
            {
                query = new StringBuilder();
                query.Append("UPDATE PublicacoesImagem SET ");
                query.Append("	QntdVisualizacoes = QntdVisualizacoes + 1 ");
                query.Append("WHERE Id = @IdPublicacao");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdPublicacao", idPublicacaoImagem);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PopularComentarios(PublicacaoImagem publicacaoImagem)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            ComentarioPublicacao comentarioPublicacao = null;
            Usuario autor = null;
            try
            {
                publicacaoImagem.Comentarios = new List<ComentarioPublicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    C.Id, ");
                query.Append("    C.Texto, ");
                query.Append("    C.IdAutor, ");
                query.Append("    C.DataHoraComentada, ");
                query.Append("    C.IdPublicacao, ");
                query.Append("    U.Nome AS NomeAutor ");
                query.Append("FROM ComentariosPublicacao C ");
                query.Append("    INNER JOIN Usuarios U ON C.IdAutor = U.Id ");
                query.Append("WHERE IdPublicacao = @IdPublicacao");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdPublicacao", publicacaoImagem.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    comentarioPublicacao = new ComentarioPublicacao();
                                    comentarioPublicacao.Id = (int)dataReader["Id"];
                                    comentarioPublicacao.Texto = dataReader["Texto"].ToString();
                                    comentarioPublicacao.DataHoraComentada = (DateTime)dataReader["DataHoraComentada"];

                                    autor = new Usuario();
                                    autor.Id = (int)dataReader["IdAutor"];
                                    autor.Nome = dataReader["NomeAutor"].ToString();
                                    comentarioPublicacao.Autor = autor;

                                    publicacaoImagem.Comentarios.Add(comentarioPublicacao);
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
        }

        public PublicacaoImagem ObterPublicacaoImagem(int idPublicacaoImagem)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            PublicacaoImagem publicacaoImagem = null;
            Imagem imagem = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    PI.Id, ");
                query.Append("    PI.IdImagem, ");
                query.Append("    PI.QntdVisualizacoes, ");
                query.Append("    P.DataHoraPublicado, ");
                query.Append("    P.IdUsuario AS IdPublicador, ");
                query.Append("    I.Legenda AS LegendaImagem, ");
                query.Append("    I.IdUsuario, ");
                query.Append("    I.DataHoraInclusao AS DataHoraInclusaoImagem, ");
                query.Append("    I.PathImagem, ");
                query.Append("    U.Nome AS NomePublicador ");
                query.Append("FROM PublicacoesImagem PI ");
                query.Append("    INNER JOIN Publicacoes P ON PI.Id = P.Id ");
                query.Append("    INNER JOIN Imagens I ON PI.IdImagem = I.Id ");
                query.Append("    INNER JOIN Usuarios U ON P.IdUsuario = U.Id ");
                query.Append("WHERE PI.Id = @Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", idPublicacaoImagem);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                publicacaoImagem = new PublicacaoImagem();
                                publicacaoImagem.Id = (int)dataReader["Id"];
                                publicacaoImagem.QuantidadeVisualizacoes = (int)dataReader["QntdVisualizacoes"];
                                publicacaoImagem.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];

                                imagem = new Imagem();
                                imagem.Id = (int)dataReader["IdImagem"];
                                imagem.Legenda = dataReader["LegendaImagem"].ToString();
                                imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusaoImagem"];
                                imagem.PathImagem = dataReader["PathImagem"].ToString();
                                publicacaoImagem.Imagem = imagem;

                                usuario = new Usuario();
                                usuario.Id = (int)dataReader["IdPublicador"];
                                usuario.Nome = dataReader["NomePublicador"].ToString();
                                publicacaoImagem.Autor = usuario;
                            }
                        }
                    }
                }

                return publicacaoImagem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public bool IncluirPublicacaoImagem(PublicacaoImagem novaPublicacaoImagem)
        {
            string connectionString = null;
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraRegistrada;
            DAOEtiquetaPublicacao daoEtiquetaPublicacao = null;
            EtiquetaPublicacao etiquetaPublicacao = null;
            try
            {
                dataHoraRegistrada = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO Imagens ( ");
                query.Append("	Legenda, ");
                query.Append("	IdUsuario, ");
                query.Append("	DataHoraInclusao, ");
                query.Append("	PathImagem ");
                query.Append(") VALUES ( ");
                query.Append("	@Legenda, ");
                query.Append("	@IdUsuario, ");
                query.Append("	@DataHoraPublicado, ");
                query.Append("	@PathImagem); ");
                query.Append("SET @IdImagem = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO Publicacoes ( ");
                query.Append("	IdTipo, ");
                query.Append("	DataHoraPublicado, ");
                query.Append("	IdUsuario ");
                query.Append(") VALUES ( ");
                query.Append("	@IdTipo, ");
                query.Append("	@DataHoraPublicado, ");
                query.Append("	@IdUsuario); ");
                query.Append("SET @IdPublicacao = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO PublicacoesImagem ( ");
                query.Append("	Id, ");
                query.Append("	IdImagem ");
                query.Append(") VALUES ( ");
                query.Append("	@IdPublicacao, ");
                query.Append("	@IdImagem); ");
                query.Append("COMMIT");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        command.Parameters.AddWithValue("@IdUsuario", novaPublicacaoImagem.Autor.Id);
                        command.Parameters.AddWithValue("@DataHoraPublicado", dataHoraRegistrada);
                        command.Parameters.AddWithValue("@PathImagem", novaPublicacaoImagem.Imagem.PathImagem);
                        command.Parameters.AddWithValue("@IdTipo", (int)novaPublicacaoImagem.Tipo);
                        command.Parameters.Add("@IdImagem", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@IdPublicacao", SqlDbType.Int).Direction = ParameterDirection.Output;
                        if (String.IsNullOrEmpty(novaPublicacaoImagem.Imagem.Legenda))
                            command.Parameters.AddWithValue("@Legenda", String.Empty);
                        else
                            command.Parameters.AddWithValue("@Legenda", novaPublicacaoImagem.Imagem.Legenda);

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            novaPublicacaoImagem.Imagem.Id = (int)command.Parameters["@IdImagem"].Value;
                            novaPublicacaoImagem.Id = (int)command.Parameters["@IdPublicacao"].Value;
                            novaPublicacaoImagem.DataHoraPublicado = dataHoraRegistrada;
                        }
                        else
                            return false;
                    }
                }

                daoEtiquetaPublicacao = new DAOEtiquetaPublicacao();
                foreach (EtiquetaPublicacao etiqueta in novaPublicacaoImagem.Etiquetas)
                {
                    etiquetaPublicacao = daoEtiquetaPublicacao.ObterEtiqueta(etiqueta.Nome);
                    if (etiquetaPublicacao == null)
                    {
                        etiquetaPublicacao = new EtiquetaPublicacao();
                        etiquetaPublicacao.Nome = etiqueta.Nome;
                        daoEtiquetaPublicacao.IncluirEtiqueta(etiquetaPublicacao);
                    }
                    daoEtiquetaPublicacao.IncluirAssociacao(novaPublicacaoImagem.Id, etiquetaPublicacao.Id);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}