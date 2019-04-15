using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOFacebookAccount
    {
        public FacebookAccount ObterFacebookAccount(string facebookId)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            FacebookAccount facebookAccount = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  F.Id, ");
                query.Append("  F.Nome AS NomeFacebook, ");
                query.Append("  F.Email AS EmailFacebook, ");
                query.Append("  F.DataHoraRegistro AS DataHoraRegistroLoginFacebook, ");
                query.Append("  F.FacebookId, ");
                query.Append("	U.Id AS IdUsuario, ");
                query.Append("	U.Nome AS NomeUsuario, ");
                query.Append("	U.Email AS EmailUsuario, ");
                query.Append("	U.Senha, ");
                query.Append("	U.Email AS EmailUsuario, ");
                query.Append("	U.IdTipoPerfil ");
                query.Append("FROM FacebookAccounts F ");
                query.Append("  INNER JOIN Usuarios U ON F.IdUsuario = U.Id ");
                query.Append("WHERE F.FacebookId = @FacebookId");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@FacebookId", facebookId);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                facebookAccount = new FacebookAccount();
                                facebookAccount.Id = (int)dataReader["Id"];
                                facebookAccount.Nome = dataReader["NomeFacebook"].ToString();
                                facebookAccount.Email = dataReader["EmailFacebook"].ToString();
                                facebookAccount.DataHoraRegistro = (DateTime)dataReader["DataHoraRegistroLoginFacebook"];
                                facebookAccount.FacebookId = dataReader["FacebookId"].ToString();

                                usuario = new Usuario();
                                usuario.Id = (int)dataReader["IdUsuario"];
                                usuario.Nome = dataReader["NomeUsuario"].ToString();
                                usuario.Email = dataReader["EmailUsuario"].ToString();
                                usuario.Senha = dataReader["Senha"].ToString();
                                usuario.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                usuario.Facebook = facebookAccount;
                                facebookAccount.Usuario = usuario;
                            }
                        }
                    }
                }

                return facebookAccount;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }
    }
}
