using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOPublicacaoProjeto
    {
        public void PopularPublicacoesProjeto(UsuarioProfissional autor)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            PublicacaoProjeto publicacaoProjeto = null;
            try
            {
                autor.Publicacoes = new List<Publicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  PP.Id, ");
                query.Append("	PP.Titulo, ");
                query.Append("	PP.Conteudo, ");
                query.Append("  PU.DataHoraPublicado ");
                query.Append("FROM PublicacoesProjeto PP ");
                query.Append("    INNER JOIN Publicacoes PU ON PP.Id = PU.Id ");
                query.Append("    INNER JOIN Usuarios US ON PU.IdUsuario = US.Id ");
                query.Append("WHERE US.Id = @Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", autor.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                publicacaoProjeto = new PublicacaoProjeto();
                                publicacaoProjeto.Id = (int)dataReader["Id"];
                                publicacaoProjeto.Titulo = dataReader["Titulo"].ToString();
                                publicacaoProjeto.Conteudo = JsonConvert.DeserializeObject<List<ItemPublicacao>>(dataReader["Conteudo"].ToString());
                                publicacaoProjeto.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];
                                autor.Publicacoes.Add(publicacaoProjeto);
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

        public PublicacaoProjeto ObterPublicacaoProjeto(int idPublicacaoProjeto)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            PublicacaoProjeto publicacaoProjeto = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  PP.Id, ");
                query.Append("	PP.Titulo, ");
                query.Append("	PP.Conteudo, ");
                query.Append("  PU.DataHoraPublicado, ");
                query.Append("  US.Id AS IdAutor, ");
                query.Append("  US.Nome AS NomeAutor, ");
                query.Append("  US.Email AS EmailAutor ");
                query.Append("FROM PublicacoesProjeto PP ");
                query.Append("    INNER JOIN Publicacoes PU ON PP.Id = PU.Id ");
                query.Append("    INNER JOIN Usuarios US ON PU.IdUsuario = US.Id ");
                query.Append("WHERE PP.Id = @Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", idPublicacaoProjeto);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                publicacaoProjeto = new PublicacaoProjeto();
                                publicacaoProjeto.Id = (int)dataReader["Id"];
                                publicacaoProjeto.Titulo = dataReader["Titulo"].ToString();
                                publicacaoProjeto.Conteudo = JsonConvert.DeserializeObject<List<ItemPublicacao>>(dataReader["Conteudo"].ToString());
                                publicacaoProjeto.DataHoraPublicado = (DateTime)dataReader["DataHoraPublicado"];

                                usuario = new Usuario();
                                usuario.Id = (int)dataReader["IdAutor"];
                                usuario.Nome = dataReader["NomeAutor"].ToString();
                                usuario.Email = dataReader["EmailAutor"].ToString();
                                publicacaoProjeto.Autor = usuario;
                            }
                        }
                    }
                }

                return publicacaoProjeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public bool IncluirPublicacaoProjeto(PublicacaoProjeto novaPublicacaoProjeto)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraRegistrada;
            string connectionString;
            DAOEtiquetaPublicacao daoEtiquetaPublicacao = null;
            EtiquetaPublicacao etiquetaPublicacao = null;
            try
            {
                dataHoraRegistrada = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO Publicacoes ( ");
                query.Append("	IdTipo, ");
                query.Append("	DataHoraPublicado, ");
                query.Append("	IdUsuario ");
                query.Append(") VALUES ( ");
                query.Append("	@IdTipo, ");
                query.Append("	@DataHoraPublicado, ");
                query.Append("	@IdUsuario); ");
                query.Append("SET @IdPublicacao = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO PublicacoesProjeto ( ");
                query.Append("	Id, ");
                query.Append("	Titulo, ");
                query.Append("	PathImagemCapa, ");
                query.Append("	Conteudo ");
                query.Append(") VALUES ( ");
                query.Append("	@IdPublicacao, ");
                query.Append("	@Titulo, ");
                query.Append("	@PathImagemCapa, ");
                query.Append("	@Conteudo); ");
                query.Append("COMMIT");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        command.Parameters.AddWithValue("@IdTipo", (int)novaPublicacaoProjeto.Tipo);
                        command.Parameters.AddWithValue("@DataHoraPublicado", dataHoraRegistrada);
                        command.Parameters.AddWithValue("@IdUsuario", novaPublicacaoProjeto.Autor.Id);
                        command.Parameters.AddWithValue("@Titulo", novaPublicacaoProjeto.Titulo);
                        command.Parameters.AddWithValue("@PathImagemCapa", novaPublicacaoProjeto.PathImagemCapa);
                        command.Parameters.AddWithValue("@Conteudo", JsonConvert.SerializeObject(novaPublicacaoProjeto.Conteudo));
                        command.Parameters.Add("@IdPublicacao", SqlDbType.Int).Direction = ParameterDirection.Output;

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            novaPublicacaoProjeto.Id = (int)command.Parameters["@IdPublicacao"].Value;
                            novaPublicacaoProjeto.DataHoraPublicado = dataHoraRegistrada;
                        }
                        else
                            return false;
                    }
                }

                daoEtiquetaPublicacao = new DAOEtiquetaPublicacao();
                foreach (EtiquetaPublicacao etiqueta in novaPublicacaoProjeto.Etiquetas)
                {
                    etiquetaPublicacao = daoEtiquetaPublicacao.ObterEtiqueta(etiqueta.Nome);
                    if (etiquetaPublicacao == null)
                    {
                        etiquetaPublicacao = new EtiquetaPublicacao();
                        etiquetaPublicacao.Nome = etiqueta.Nome;
                        daoEtiquetaPublicacao.IncluirEtiqueta(etiquetaPublicacao);
                    }
                    daoEtiquetaPublicacao.IncluirAssociacao(novaPublicacaoProjeto.Id, etiquetaPublicacao.Id);
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