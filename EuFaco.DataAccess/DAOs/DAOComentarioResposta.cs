using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOComentarioResposta
    {
        public void PopularComentarios(Resposta resposta)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            ComentarioResposta comentario = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	C.Id, ");
                query.Append("	C.Texto, ");
                query.Append("	C.IdRespostaComentada, ");
                query.Append("	C.DataHoraComentada, ");
                query.Append("	C.IdAutor, ");
                query.Append("	U.Nome AS NomeAutor ");
                query.Append("FROM ComentariosRespostas C INNER JOIN Usuarios U ON C.IdAutor = U.Id ");
                query.Append("WHERE C.IdRespostaComentada = @IdResposta");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@IdResposta", resposta.Id);

                dataReader = command.ExecuteReader();

                resposta.Comentarios = new List<ComentarioResposta>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        comentario = new ComentarioResposta();
                        comentario.Id = (int)dataReader["Id"];
                        comentario.Texto = dataReader["Texto"].ToString();
                        comentario.DataHoraComentada = (DateTime)dataReader["DataHoraComentada"];
                        comentario.RespostaComentada = resposta;

                        usuario = new Usuario();
                        usuario.Id = (int)dataReader["IdAutor"];
                        usuario.Nome = dataReader["NomeAutor"].ToString();
                        comentario.Autor = usuario;

                        resposta.Comentarios.Add(comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IncluirComentario(ComentarioResposta novoComentarioResposta)
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
                query.Append("INSERT INTO ComentariosRespostas ( ");
                query.Append("	Texto, ");
                query.Append("	IdAutor, ");
                query.Append("	DataHoraComentada, ");
                query.Append("	IdRespostaComentada ");
                query.Append(") VALUES ( ");
                query.Append("	@Texto, ");
                query.Append("	@IdAutor, ");
                query.Append("	@DataHoraComentada, ");
                query.Append("	@IdRespostaComentada); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Texto", novoComentarioResposta.Texto);
                command.Parameters.AddWithValue("@IdAutor", novoComentarioResposta.Autor.Id);
                command.Parameters.AddWithValue("@DataHoraComentada", dataHoraRegistrada);
                command.Parameters.AddWithValue("@IdRespostaComentada", novoComentarioResposta.RespostaComentada.Id);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novoComentarioResposta.Id = (int)command.Parameters["@Id"].Value;
                    novoComentarioResposta.DataHoraComentada = dataHoraRegistrada;
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
