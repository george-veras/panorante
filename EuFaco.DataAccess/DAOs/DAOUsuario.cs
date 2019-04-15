using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOUsuario
    {
        public Usuario ObterUsuario(string email, string senha)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Id, ");
                query.Append("	Nome, ");
                query.Append("	Email, ");
                query.Append("	Senha, ");
                query.Append("	DataHoraCadastro, ");
                query.Append("	IdTipoPerfil ");
                query.Append("FROM Usuarios ");
                query.Append("WHERE Email = @Email ");
                query.Append("  AND Senha = @Senha");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Senha", senha);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                usuario = new Usuario();
                                usuario.Id = (int)dataReader["Id"];
                                usuario.Nome = dataReader["Nome"].ToString();
                                usuario.Email = dataReader["Email"].ToString();
                                usuario.Senha = dataReader["Senha"].ToString();
                                usuario.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuario.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                            }
                        }
                    }
                }
                return usuario;
            }
            finally
            {
                if (usuario != null)
                    usuario = null;
            }
        }

        public bool EditarNomeEmail(string novoNome, string novoEmail, int idUsuario)
        {
            string connectionString;
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            try
            {
                query = new StringBuilder();
                query.Append("UPDATE Usuarios SET ");
                query.Append("	Nome = @Nome, ");
                query.Append("	Email = @Email ");
                query.Append("WHERE Id = @Id");
                
                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command = connection.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Nome", novoNome);
                        command.Parameters.AddWithValue("@Email", novoEmail);
                        command.Parameters.AddWithValue("@Id", idUsuario);

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtém um usuário específico, buscado pelo e-mail informado.
        /// </summary>
        /// <param name="email">E-mail do usuário buscado</param>
        /// <returns>Instância do usuário com o e-mail informado.</returns>
        public Usuario ObterUsuarioPorEmail(string email)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            Usuario usuario = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Id, ");
                query.Append("	Nome, ");
                query.Append("	Email, ");
                query.Append("	Senha, ");
                query.Append("	DataHoraCadastro, ");
                query.Append("	IdTipoPerfil ");
                query.Append("FROM Usuarios ");
                query.Append("WHERE Email = @Email");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Email", email);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                usuario = new Usuario();
                                usuario.Id = (int)dataReader["Id"];
                                usuario.Nome = dataReader["Nome"].ToString();
                                usuario.Email = dataReader["Email"].ToString();
                                usuario.Senha = dataReader["Senha"].ToString();
                                usuario.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuario.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                            }
                        }
                    }
                }
                return usuario;
            }
            finally
            {
                if (usuario != null)
                    usuario = null;
            }
        }

        /// <summary>
        /// Inclui no banco de dados o registro do usuário informado e sua conta no Facebook 
        /// (encontrada na propriedade "Facebook").
        /// </summary>
        /// <param name="usuario">A instância de usuário a ser incluída junto com a propriedade Facebook.</param>
        /// <returns>Informa se a inclusão foi feita com sucesso.</returns>
        public bool IncluirUsuarioComFacebook(Usuario usuario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraRegistro;
            try
            {
                dataHoraRegistro = DateTime.Now;

                query = new StringBuilder();
                query.Append("BEGIN TRANSACTION ");
                query.Append("INSERT INTO Usuarios ( ");
                query.Append("	Nome, ");
                query.Append("  DataHoraCadastro, ");
                query.Append("	Email, ");
                query.Append("  IdTipoPerfil, ");
                query.Append("	Senha ");
                query.Append(") VALUES ( ");
                query.Append("	@Nome, ");
                query.Append("  @DataHoraCadastro, ");
                query.Append("	@Email, ");
                query.Append("	@IdTipoPerfil, ");
                query.Append("	@Senha); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO FacebookAccounts ( ");
                query.Append("	IdUsuario, ");
                query.Append("  Nome, ");
                query.Append("	Email, ");
                query.Append("  DataHoraRegistro, ");
                query.Append("	FacebookId ");
                query.Append(") VALUES ( ");
                query.Append("	@Id, ");
                query.Append("  @NomeFacebook, ");
                query.Append("	@EmailFacebook, ");
                query.Append("	@DataHoraCadastro, ");
                query.Append("	@FacebookId); ");
                query.Append("COMMIT");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Nome", usuario.Nome);
                        command.Parameters.AddWithValue("@DataHoraCadastro", dataHoraRegistro);
                        command.Parameters.AddWithValue("@Email", usuario.Email);
                        command.Parameters.AddWithValue("@IdTipoPerfil", (int)usuario.Perfil);
                        command.Parameters.AddWithValue("@NomeFacebook", usuario.Facebook.Nome);
                        command.Parameters.AddWithValue("@EmailFacebook", usuario.Facebook.Email);
                        command.Parameters.AddWithValue("@FacebookId", usuario.Facebook.FacebookId);
                        if (String.IsNullOrEmpty(usuario.Senha))
                            command.Parameters.AddWithValue("@Senha", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Senha", usuario.Senha);
                        command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            usuario.Id = (int)command.Parameters["@Id"].Value;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
