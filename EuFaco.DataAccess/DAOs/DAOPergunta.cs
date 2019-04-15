using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOPergunta
    {
        public bool IncluirPergunta(Pergunta novaPergunta)
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
                query.Append("INSERT INTO Perguntas ( ");
                query.Append("	IdAutor, ");
                query.Append("	Titulo, ");
                query.Append("	Texto, ");
                query.Append("	DataHora ");
                query.Append(") VALUES ( ");
                query.Append("	@IdAutor, ");
                query.Append("	@Titulo, ");
                query.Append("	@Texto, ");
                query.Append("	@DataHora); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdAutor", novaPergunta.Autor.Id);
                command.Parameters.AddWithValue("@Titulo", novaPergunta.Titulo);
                command.Parameters.AddWithValue("@Texto", novaPergunta.Texto);
                command.Parameters.AddWithValue("@DataHora", dataHoraRegistrada);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaPergunta.Id = (int)command.Parameters["@Id"].Value;
                    novaPergunta.DataHoraPerguntada = dataHoraRegistrada;
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

        public List<Pergunta> ObterPerguntas()
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Pergunta pergunta = null;
            Usuario usuario = null;
            List<Pergunta> perguntas = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  P.Id, ");
                query.Append("	P.IdAutor, ");
                query.Append("	P.Titulo, ");
                query.Append("	P.Texto, ");
                query.Append("	P.DataHora, ");
                query.Append("	U.Nome AS NomeAutor ");
                query.Append("FROM Perguntas P INNER JOIN Usuarios U ON P.IdAutor = U.Id ");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                dataReader = command.ExecuteReader();

                perguntas = new List<Pergunta>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        pergunta = new Pergunta();

                        pergunta.Id = (int)dataReader["Id"];
                        pergunta.Titulo = dataReader["Titulo"].ToString();
                        pergunta.Texto = dataReader["Texto"].ToString();
                        pergunta.DataHoraPerguntada = (DateTime)dataReader["DataHora"];

                        usuario = new Usuario();
                        usuario.Id = (int)dataReader["Id"];
                        usuario.Nome = dataReader["NomeAutor"].ToString();
                        pergunta.Autor = usuario;

                        perguntas.Add(pergunta);
                    }
                }

                return perguntas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Pergunta ObterPergunta(int idPergunta)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Pergunta pergunta = null;
            Usuario usuario = null;
            DAOResposta daoResposta = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  P.Id, ");
                query.Append("	P.IdAutor, ");
                query.Append("	P.Titulo, ");
                query.Append("	P.Texto, ");
                query.Append("	P.DataHora, ");
                query.Append("	U.Nome AS NomeAutor ");
                query.Append("FROM Perguntas P INNER JOIN Usuarios U ON P.IdAutor = U.Id ");
                query.Append("WHERE P.Id = @Id");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@Id", idPergunta);

                dataReader = command.ExecuteReader();

                daoResposta = new DAOResposta();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    pergunta = new Pergunta();

                    pergunta.Id = (int)dataReader["Id"];
                    pergunta.Titulo = dataReader["Titulo"].ToString();
                    pergunta.Texto = dataReader["Texto"].ToString();
                    pergunta.DataHoraPerguntada = (DateTime)dataReader["DataHora"];

                    usuario = new Usuario();
                    usuario.Id = (int)dataReader["IdAutor"];
                    usuario.Nome = dataReader["NomeAutor"].ToString();
                    pergunta.Autor = usuario;
                }

                daoResposta.PopularRespostas(pergunta);

                return pergunta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
