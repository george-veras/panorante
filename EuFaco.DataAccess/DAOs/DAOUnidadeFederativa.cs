using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOUnidadeFederativa
    {
        public List<UnidadeFederativa> ListarUnidadesFederativas()
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            UnidadeFederativa uf = null;
            List<UnidadeFederativa> unidadesFederativas = null;
            try
            {
                unidadesFederativas = new List<UnidadeFederativa>();

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("    Id, ");
                query.Append("    Nome, ");
                query.Append("    Abreviacao ");
                query.Append("FROM UnidadesFederativas");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    uf = new UnidadeFederativa();
                                    uf.Id = (int)dataReader["Id"];
                                    uf.Nome = dataReader["Nome"].ToString();
                                    uf.Abreviacao = dataReader["Abreviacao"].ToString();

                                    unidadesFederativas.Add(uf);
                                }
                            }
                        }
                    }
                }

                return unidadesFederativas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
