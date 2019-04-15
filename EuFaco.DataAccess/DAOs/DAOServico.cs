using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOServico
    {
        public bool IncluirServico(Servico novoServico)
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
                query.Append("INSERT INTO Servicos ( ");
                query.Append("	IdSolicitante, ");
                query.Append("	Titulo, ");
                query.Append("	Descricao, ");
                query.Append("	DataHoraSolicitacao ");
                query.Append(") VALUES ( ");
                query.Append("	@IdSolicitante, ");
                query.Append("	@Titulo, ");
                query.Append("	@Descricao, ");
                query.Append("	@DataHoraSolicitacao); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdSolicitante", novoServico.Solicitante.Id);
                command.Parameters.AddWithValue("@Titulo", novoServico.Titulo);
                command.Parameters.AddWithValue("@Descricao", novoServico.Descricao);
                command.Parameters.AddWithValue("@DataHoraSolicitacao", dataHoraRegistrada);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novoServico.Id = (int)command.Parameters["@Id"].Value;
                    novoServico.DataHoraSolicitacao = dataHoraRegistrada;
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

        public Servico ObterServico(int idServico)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Servico servico = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  S.Id, ");
                query.Append("	S.Titulo, ");
                query.Append("	S.Descricao, ");
                query.Append("	S.DataHoraSolicitacao, ");
                query.Append("	S.IdSolicitante, ");
                query.Append("	U.Nome AS NomeSolicitante ");
                query.Append("FROM Servicos S INNER JOIN Usuarios U ON S.IdSolicitante = U.Id ");
                query.Append("WHERE S.Id = @Id");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@Id", idServico);

                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();

                    servico = new Servico();
                    servico.Id = (int)dataReader["Id"];
                    servico.Titulo = dataReader["Titulo"].ToString();
                    servico.Descricao = dataReader["Descricao"].ToString();
                    servico.DataHoraSolicitacao = (DateTime)dataReader["DataHoraSolicitacao"];

                    usuario = new Usuario();
                    usuario.Id = (int)dataReader["IdSolicitante"];
                    usuario.Nome = dataReader["NomeSolicitante"].ToString();
                    servico.Solicitante = usuario;
                }

                return servico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Servico> ObterServicos()
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Servico servico = null;
            Usuario usuario = null;
            List<Servico> listaServicos = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  S.Id, ");
                query.Append("	S.Titulo, ");
                query.Append("	S.Descricao, ");
                query.Append("	S.DataHoraSolicitacao, ");
                query.Append("	S.IdSolicitante, ");
                query.Append("	U.Nome AS NomeSolicitante ");
                query.Append("FROM Servicos S INNER JOIN Usuarios U ON S.IdSolicitante = U.Id ");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                dataReader = command.ExecuteReader();

                listaServicos = new List<Servico>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        servico = new Servico();
                        servico.Id = (int)dataReader["Id"];
                        servico.Titulo = dataReader["Titulo"].ToString();
                        servico.Descricao = dataReader["Descricao"].ToString();
                        servico.DataHoraSolicitacao = (DateTime)dataReader["DataHoraSolicitacao"];

                        usuario = new Usuario();
                        usuario.Id = (int)dataReader["IdSolicitante"];
                        usuario.Nome = dataReader["NomeSolicitante"].ToString();
                        servico.Solicitante = usuario;

                        listaServicos.Add(servico);
                    }
                }

                return listaServicos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtém uma lista de serviços publicados pelo usuário informado, identificado pela propriedade "Id". 
        /// As candidaturas de cada serviço solicitado também são obtidas.
        /// </summary>
        /// <param name="usuarioSolicitante">Instância do usuário que publicou os serviços buscados, 
        /// a propriedade Id desta instância será usada como filtro de pesquisa.</param>
        /// <returns>Lista de serviços publicados por este usuário (identificado pelo Id).</returns>
        /// <remarks>
        /// <para>Criado por: George Véras</para>
        /// <para>Data de Criação: 29/01/2016</para>
        /// <para>Alterado por: -x-</para>
        /// <para>Data da última alteração: -x-</para>
        /// </remarks>
        public List<Servico> ObterServicosSolicitados(Usuario usuarioSolicitante)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Servico servicoSolicitado = null;
            List<Servico> listaServicos = null;
            DAOCandidatura daoCandidatura = null;
            try
            {
                listaServicos = new List<Servico>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Id, ");
                query.Append("	IdSolicitante, ");
                query.Append("	Titulo, ");
                query.Append("	Descricao, ");
                query.Append("	DataHoraSolicitacao ");
                query.Append("FROM Servicos ");
                query.Append("WHERE IdSolicitante = @IdSolicitante");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdSolicitante", usuarioSolicitante.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            daoCandidatura = new DAOCandidatura();
                            while (dataReader.Read())
                            {
                                servicoSolicitado = new Servico();
                                servicoSolicitado.Id = (int)dataReader["Id"];
                                servicoSolicitado.Solicitante = usuarioSolicitante;
                                servicoSolicitado.Titulo = dataReader["Titulo"].ToString();
                                servicoSolicitado.Descricao = dataReader["Descricao"].ToString();
                                servicoSolicitado.DataHoraSolicitacao = (DateTime)dataReader["DataHoraSolicitacao"];

                                daoCandidatura.PopularCandidaturas(servicoSolicitado);

                                listaServicos.Add(servicoSolicitado);
                            }
                        }
                    }
                }

                return listaServicos;
            }
            finally
            {
                if (query != null)
                    query = null;

                if (servicoSolicitado != null)
                    servicoSolicitado = null;

                if (listaServicos != null)
                    listaServicos = null;

                if (daoCandidatura != null)
                    daoCandidatura = null;
            }
        }
    }
}
