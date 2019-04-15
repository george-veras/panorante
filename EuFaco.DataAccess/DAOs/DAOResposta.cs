using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOResposta
    {
        public void PopularRespostas(Pergunta pergunta)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Resposta resposta = null;
            Usuario usuario = null;
            DAOComentarioResposta daoComentarioResposta = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	R.Id, ");
                query.Append("	R.Texto, ");
                query.Append("	R.IdPerguntaRespondida, ");
                query.Append("	R.IdAutor, ");
                query.Append("	R.DataHoraRespondida, ");
                query.Append("	U.Nome AS NomeAutor ");
                query.Append("FROM Respostas R INNER JOIN Usuarios U ON R.IdAutor = U.Id ");
                query.Append("WHERE IdPerguntaRespondida = @IdPerguntaRespondida");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@IdPerguntaRespondida", pergunta.Id);

                dataReader = command.ExecuteReader();

                pergunta.Respostas = new List<Resposta>();
                daoComentarioResposta = new DAOComentarioResposta();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        resposta = new Resposta();
                        resposta.Id = (int)dataReader["Id"];
                        resposta.Texto = dataReader["Texto"].ToString();
                        resposta.DataHoraRespondida = (DateTime)dataReader["DataHoraRespondida"];
                        resposta.Pergunta = pergunta;

                        usuario = new Usuario();
                        usuario.Id = (int)dataReader["IdAutor"];
                        usuario.Nome = dataReader["NomeAutor"].ToString();
                        resposta.Autor = usuario;

                        daoComentarioResposta.PopularComentarios(resposta);

                        pergunta.Respostas.Add(resposta);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IncluirResposta(Resposta novaResposta)
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
                query.Append("INSERT INTO Respostas ( ");
                query.Append("	Texto, ");
                query.Append("	IdPerguntaRespondida, ");
                query.Append("	IdAutor, ");
                query.Append("	DataHoraRespondida ");
                query.Append(") VALUES ( ");
                query.Append("	@Texto, ");
                query.Append("	@IdPerguntaRespondida, ");
                query.Append("	@IdAutor, ");
                query.Append("	@DataHoraRespondida); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Texto", novaResposta.Texto);
                command.Parameters.AddWithValue("@IdPerguntaRespondida", novaResposta.Pergunta.Id);
                command.Parameters.AddWithValue("@IdAutor", novaResposta.Autor.Id);
                command.Parameters.AddWithValue("@DataHoraRespondida", dataHoraRegistrada);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaResposta.Id = (int)command.Parameters["@Id"].Value;
                    novaResposta.DataHoraRespondida = dataHoraRegistrada;
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
    }
}
