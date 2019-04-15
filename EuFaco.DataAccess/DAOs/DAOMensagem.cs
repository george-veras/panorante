using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOMensagem
    {
        public bool IncluirMensagem(Mensagem novaMensagem)
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
                query.Append("INSERT INTO Mensagens ( ");
                query.Append("	IdDestinatario, ");
                query.Append("	IdRemetente, ");
                query.Append("	Conteudo, ");
                query.Append("	DataHoraEnvio ");
                query.Append(") VALUES ( ");
                query.Append("	@IdDestinatario, ");
                query.Append("	@IdRemetente, ");
                query.Append("	@Conteudo, ");
                query.Append("	@DataHoraEnvio); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdDestinatario", novaMensagem.Destinatario.Id);
                command.Parameters.AddWithValue("@IdRemetente", novaMensagem.Remetente.Id);
                command.Parameters.AddWithValue("@Conteudo", novaMensagem.Conteudo);
                command.Parameters.AddWithValue("@DataHoraEnvio", dataHoraRegistrada);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaMensagem.Id = (int)command.Parameters["@Id"].Value;
                    novaMensagem.DataHoraEnvio = dataHoraRegistrada;
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

        public List<Mensagem> ObterMensagens(Usuario usuarioDestinatario)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Mensagem mensagem = null;
            Usuario remetente = null;
            List<Mensagem> listaMensagens = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  M.Id, ");
                query.Append("	M.IdDestinatario, ");
                query.Append("	M.IdRemetente, ");
                query.Append("	M.Conteudo, ");
                query.Append("	M.DataHoraEnvio, ");
                query.Append("	U.Nome AS NomeRemetente ");
                query.Append("FROM Mensagens M INNER JOIN Usuarios U ON M.IdDestinatario = U.Id ");
                query.Append("  AND M.IdDestinatario = @IdDestinatario");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@IdDestinatario", usuarioDestinatario.Id);

                dataReader = command.ExecuteReader();

                listaMensagens = new List<Mensagem>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        mensagem = new Mensagem();

                        mensagem.Id = (int)dataReader["Id"];
                        mensagem.Conteudo = dataReader["Conteudo"].ToString();
                        mensagem.DataHoraEnvio = (DateTime)dataReader["DataHoraEnvio"];

                        remetente = new Usuario();
                        remetente.Id = (int)dataReader["IdRemetente"];
                        remetente.Nome = dataReader["NomeRemetente"].ToString();
                        mensagem.Remetente = remetente;

                        listaMensagens.Add(mensagem);
                    }
                }

                return listaMensagens;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}