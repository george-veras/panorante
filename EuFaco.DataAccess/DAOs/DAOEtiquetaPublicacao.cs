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
    public class DAOEtiquetaPublicacao
    {
        public void PopularEtiquetas(PublicacaoImagem publicacaoImagem)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            EtiquetaPublicacao etiquetaPublicacao = null;
            try
            {
                publicacaoImagem.Etiquetas = new List<EtiquetaPublicacao>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    PE.IdPublicacao, ");
                query.Append("    PE.IdEtiquetaPublicacao, ");
                query.Append("    E.Nome, ");
                query.Append("    E.Quantidade ");
                query.Append("FROM Publicacoes_EtiquetasPublicacao PE ");
                query.Append("    INNER JOIN EtiquetasPublicacao E ON PE.IdEtiquetaPublicacao = E.Id ");
                query.Append("WHERE PE.IdPublicacao = @IdPublicacao ");

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
                                    etiquetaPublicacao = new EtiquetaPublicacao();
                                    etiquetaPublicacao.Id = (int)dataReader["IdEtiquetaPublicacao"];
                                    etiquetaPublicacao.Nome = dataReader["Nome"].ToString();
                                    etiquetaPublicacao.Quantidade = (int)dataReader["Quantidade"];

                                    publicacaoImagem.Etiquetas.Add(etiquetaPublicacao);
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

        public bool IncluirAssociacao(int idPublicacao, int idEtiqueta)
        {
            string connectionString = null;
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraRegistrada;
            try
            {
                dataHoraRegistrada = DateTime.Now;

                query = new StringBuilder();
                query.Append("INSERT INTO Publicacoes_EtiquetasPublicacao ( ");
                query.Append("	IdPublicacao, ");
                query.Append("	IdEtiquetaPublicacao ");
                query.Append(") VALUES ( ");
                query.Append("	@IdPublicacao, ");
                query.Append("	@IdEtiquetaPublicacao)");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        command.Parameters.AddWithValue("@IdPublicacao", idPublicacao);
                        command.Parameters.AddWithValue("@IdEtiquetaPublicacao", idEtiqueta);

                        connection.Open();
                        if (command.ExecuteNonQuery() == 0)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IncluirEtiqueta(EtiquetaPublicacao novaEtiquetaPublicacao)
        {
            string connectionString = null;
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraRegistrada;
            try
            {
                dataHoraRegistrada = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO EtiquetasPublicacao ( ");
                query.Append("	Nome, ");
                query.Append("	Quantidade, ");
                query.Append("	DataHoraInclusao ");
                query.Append(") VALUES ( ");
                query.Append("	@Nome, ");
                query.Append("	0, ");
                query.Append("	@DataHoraInclusao); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        command.Parameters.AddWithValue("@Nome", novaEtiquetaPublicacao.Nome);
                        command.Parameters.AddWithValue("@DataHoraInclusao", dataHoraRegistrada);
                        command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            novaEtiquetaPublicacao.Id = (int)command.Parameters["@Id"].Value;
                            novaEtiquetaPublicacao.DataHoraInclusao = (DateTime)command.Parameters["@DataHoraInclusao"].Value;
                        }
                        else
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EtiquetaPublicacao ObterEtiqueta(string nomeEtiqueta)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            EtiquetaPublicacao etiquetaPublicacao = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    Id, ");
                query.Append("    Nome, ");
                query.Append("    Quantidade, ");
                query.Append("    DataHoraInclusao ");
                query.Append("FROM EtiquetasPublicacao ");
                query.Append("WHERE Nome = @Nome");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Nome", nomeEtiqueta);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                etiquetaPublicacao = new EtiquetaPublicacao();
                                etiquetaPublicacao.Id = (int)dataReader["Id"];
                                etiquetaPublicacao.Nome = dataReader["Nome"].ToString();
                                etiquetaPublicacao.Quantidade = (int)dataReader["Quantidade"];
                                etiquetaPublicacao.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusao"];
                            }
                        }
                    }
                }

                return etiquetaPublicacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
