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
    public class DAOAvaliacao
    {
        public void PopularAvaliacoes(UsuarioProfissional usuarioAvaliado)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Avaliacao avaliacao = null;
            Usuario avaliador = null;
            DAOComentarioAvaliacao daoComentarioAvaliacao = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	A.Id, ");
                query.Append("	A.IdAvaliador, ");
                query.Append("	A.IdAvaliado, ");
                query.Append("	A.Resumo, ");
                query.Append("	A.Texto, ");
                query.Append("	A.Nota, ");
                query.Append("	A.DataHoraAvaliacao, ");
                query.Append("	U.Nome AS NomeAvaliador, ");
                query.Append("	U.Sobrenome AS SobrenomeAvaliador ");
                query.Append("FROM Avaliacoes A INNER JOIN Usuarios U ON A.IdAvaliador = U.Id ");
                query.Append("WHERE IdAvaliado = @IdAvaliado");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdAvaliado", usuarioAvaliado.Id);

                dataReader = command.ExecuteReader();

                usuarioAvaliado.Avaliacoes = new List<Avaliacao>();
                daoComentarioAvaliacao = new DAOComentarioAvaliacao();
                if (dataReader.HasRows) 
                {
                    while (dataReader.Read())
                    {
                        avaliacao = new Avaliacao();
                        
                        avaliacao.Id = (int)dataReader["Id"];
                        avaliacao.Avaliado = usuarioAvaliado;
                        avaliacao.Resumo = dataReader["Resumo"].ToString();
                        avaliacao.Texto = dataReader["Texto"].ToString();
                        avaliacao.Nota = byte.Parse(dataReader["Nota"].ToString());
                        avaliacao.DataHoraAvaliacao = (DateTime)dataReader["DataHoraAvaliacao"];

                        avaliador = new Usuario();
                        avaliador.Id = (int)dataReader["IdAvaliador"];
                        avaliador.Nome = dataReader["NomeAvaliador"].ToString();
                        avaliador.Sobrenome = dataReader["SobrenomeAvaliador"].ToString();
                        avaliacao.Avaliador = avaliador;

                        daoComentarioAvaliacao.PopularComentarios(avaliacao);
                        usuarioAvaliado.Avaliacoes.Add(avaliacao);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IncluirAvaliacao(Avaliacao novaAvaliacao)
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
                query.Append("INSERT INTO Avaliacoes ( ");
                query.Append("	IdAvaliador, ");
                query.Append("	IdAvaliado, ");
                query.Append("	Resumo, ");
                query.Append("	Nota, ");
                query.Append("	DataHoraAvaliacao, ");
                query.Append("	Texto ");
                query.Append(") VALUES ( ");
                query.Append("	@IdAvaliador, ");
                query.Append("	@IdAvaliado, ");
                query.Append("	@Resumo, ");
                query.Append("	@Nota, ");
                query.Append("	@DataHoraAvaliacao, ");
                query.Append("	@Texto); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdAvaliador", novaAvaliacao.Avaliador.Id);
                command.Parameters.AddWithValue("@IdAvaliado", novaAvaliacao.Avaliado.Id);
                command.Parameters.AddWithValue("@Resumo", novaAvaliacao.Resumo);
                command.Parameters.AddWithValue("@Nota", novaAvaliacao.Nota);
                command.Parameters.AddWithValue("@DataHoraAvaliacao", dataHoraRegistrada);
                command.Parameters.AddWithValue("@Texto", novaAvaliacao.Texto);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaAvaliacao.Id = (int)command.Parameters["@Id"].Value;
                    novaAvaliacao.DataHoraAvaliacao = dataHoraRegistrada;
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
