using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOComentarioPublicacao
    {
        public bool IncluirComentarioPublicacao(ComentarioPublicacao novoComentario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraComentada;
            try
            {
                dataHoraComentada = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO ComentariosPublicacao ( ");
                query.Append("	Texto, ");
                query.Append("  IdAutor, ");
                query.Append("	DataHoraComentada ");
                query.Append(") VALUES ( ");
                query.Append("	@Texto, ");
                query.Append("  @IdAutor, ");
                query.Append("	@DataHoraComentada); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Texto", novoComentario.Texto);
                        command.Parameters.AddWithValue("@IdAutor", novoComentario.Autor.Id);
                        command.Parameters.AddWithValue("@DataHoraComentada", dataHoraComentada);
                        command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        connection.Open();
                        if (command.ExecuteNonQuery() == 0)
                            return false;

                        novoComentario.Id = (int)command.Parameters["@Id"].Value;
                        novoComentario.DataHoraComentada = dataHoraComentada;
                    }
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
