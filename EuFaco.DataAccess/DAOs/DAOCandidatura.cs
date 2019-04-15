using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOCandidatura
    {
        public bool IncluirCandidatura(Candidatura novaCandidatura)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraRegistrada;
            try
            {
                dataHoraRegistrada = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO Candidaturas ( ");
                query.Append("	IdCandidato, ");
                query.Append("	Mensagem, ");
                query.Append("	DataHoraCandidatura, ");
                query.Append("	IdServicoCandidatado ");
                query.Append(") VALUES ( ");
                query.Append("	@IdCandidato, ");
                query.Append("	@Mensagem, ");
                query.Append("	@DataHoraCandidatura, ");
                query.Append("	@IdServicoCandidatado); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdCandidato", novaCandidatura.Candidato.Id);
                command.Parameters.AddWithValue("@Mensagem", novaCandidatura.Mensagem);
                command.Parameters.AddWithValue("@DataHoraCandidatura", dataHoraRegistrada);
                command.Parameters.AddWithValue("@IdServicoCandidatado", novaCandidatura.ServicoCandidatado.Id);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaCandidatura.Id = (int)command.Parameters["@Id"].Value;
                    novaCandidatura.DataHoraCandidatura = dataHoraRegistrada;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PopularCandidaturas(Servico servicoSolicitado)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Candidatura candidatura = null;
            UsuarioProfissional candidato = null;
            try
            {
                servicoSolicitado.Candidaturas = new List<Candidatura>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	C.Id, ");
                query.Append("	C.IdCandidato, ");
                query.Append("	C.Mensagem, ");
                query.Append("	C.DataHoraCandidatura, ");
                query.Append("	C.IdServicoCandidatado, ");
                query.Append("	U.Nome AS NomeCandidato, ");
                query.Append("	U.NumeroCelular AS CelularCandidato, ");
                query.Append("	U.DataHoraCadastro AS DataHoraCadastroCandidato, ");
                query.Append("	U.Email AS EmailCandidato, ");
                query.Append("	UPR.Resumo AS ResumoCandidato, ");
                query.Append("	UPR.IdEstado AS IdEstadoCandidato, ");
                query.Append("	UPR.IdMunicipio AS IdMunicipioCandidato ");
                query.Append("FROM Candidaturas C ");
                query.Append("	INNER JOIN Usuarios U ON C.IdCandidato = U.Id ");
                query.Append("	INNER JOIN UsuariosProfissionais UPR ON C.IdCandidato = UPR.Id ");
                query.Append("WHERE C.IdServicoCandidatado = @IdServicoCandidatado");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdServicoCandidatado", servicoSolicitado.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                candidatura = new Candidatura();
                                candidatura.Id = (int)dataReader["Id"];
                                candidatura.Mensagem = dataReader["Mensagem"].ToString();
                                candidatura.DataHoraCandidatura = (DateTime)dataReader["DataHoraCandidatura"];

                                candidato = new UsuarioProfissional();
                                candidato.Id = (int)dataReader["IdCandidato"];
                                candidato.Nome = dataReader["NomeCandidato"].ToString();
                                candidato.NumeroCelular = dataReader["CelularCandidato"].ToString();
                                candidato.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastroCandidato"];
                                candidato.Email = dataReader["EmailCandidato"].ToString();
                                candidato.Resumo = dataReader["ResumoCandidato"].ToString();

                                candidatura.ServicoCandidatado = servicoSolicitado;
                                candidatura.Candidato = candidato;
                                servicoSolicitado.Candidaturas.Add(candidatura);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (query != null)
                    query = null;

                if (candidatura != null)
                    candidatura = null;

                if (candidato != null)
                    candidato = null;
            }
        }
    }
}
