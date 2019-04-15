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
    public class DAOComentarioAvaliacao
    {
        public void PopularComentarios(Avaliacao avaliacao)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            ComentarioAvaliacao comentario = null;
            Usuario autor = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	C.Id, ");
                query.Append("	C.Texto, ");
                query.Append("	C.DataHora, ");
                query.Append("	C.IdAutor, ");
                query.Append("	U.Nome AS NomeAutor, ");
                query.Append("	U.Sobrenome AS SobrenomeAutor ");
                query.Append("FROM ComentariosAvaliacoes C INNER JOIN Usuarios U ON C.IdAutor = U.Id ");
                query.Append("WHERE C.IdAvaliacao = @IdAvaliacao");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@IdAvaliacao", avaliacao.Id);

                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        comentario = new ComentarioAvaliacao();

                        comentario.Id = (int)dataReader["Id"];
                        comentario.Texto = dataReader["Texto"].ToString();
                        comentario.DataHora = (DateTime)dataReader["DataHora"];

                        autor = new Usuario();
                        autor.Id = (int)dataReader["IdAutor"];
                        autor.Nome = dataReader["NomeAutor"].ToString();
                        autor.Sobrenome = dataReader["SobrenomeAutor"].ToString();
                        comentario.Autor = autor;

                        avaliacao.Comentarios.Add(comentario);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
