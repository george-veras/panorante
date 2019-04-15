using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOUsuarioParticular
    {
        public bool IncluirUsuarioParticular(UsuarioParticular novoUsuarioParticular)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            DateTime dataHoraCadastro;
            try
            {
                dataHoraCadastro = DateTime.Now;

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
                query.Append("INSERT INTO UsuariosParticulares (Id) VALUES (@Id) ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Nome", novoUsuarioParticular.Nome);
                command.Parameters.AddWithValue("@DataHoraCadastro", dataHoraCadastro);
                command.Parameters.AddWithValue("@Email", novoUsuarioParticular.Email);
                command.Parameters.AddWithValue("@IdTipoPerfil", (int)Usuario.TipoPerfil.UsuarioParticular);
                command.Parameters.AddWithValue("@Senha", novoUsuarioParticular.Senha);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novoUsuarioParticular.Id = (int)command.Parameters["@Id"].Value;
                    novoUsuarioParticular.DataHoraCadastro = dataHoraCadastro;
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

        public UsuarioParticular IncluirComplementoUsuarioParticular(Usuario usuario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraRegistro;
            UsuarioParticular usuarioParticular = null;
            try
            {
                dataHoraRegistro = DateTime.Now;

                query = new StringBuilder();
                query.Append("INSERT INTO UsuariosParticulares (Id) VALUES (@Id) ");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", usuario.Id);

                        connection.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            usuarioParticular = new UsuarioParticular(usuario);
                        }
                    }
                }
                return usuarioParticular;
            }
            finally
            {
            }
        }

        public UsuarioParticular CompletarPerfilUsuarioParticular(Usuario usuario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraRegistro;
            UsuarioParticular usuarioParticular = null;
            SqlDataReader dataReader = null;
            try
            {
                dataHoraRegistro = DateTime.Now;

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Id, ");
                query.Append("	IdEstado, ");
                query.Append("	IdCidade, ");
                query.Append("	PathFotoPerfil ");
                query.Append("FROM UsuariosParticulares ");
                query.Append("WHERE Id = @Id ");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", usuario.Id);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                usuarioParticular = new UsuarioParticular(usuario);
                                usuarioParticular.Id = (int)dataReader["Id"];
                            }
                        }
                    }
                }
                return usuarioParticular;
            }
            finally
            {
            }
        }

        public UsuarioParticular ObterUsuarioParticular(string email, string senha)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            UsuarioParticular usuarioParticular = null;
            SqlDataReader dataReader = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	U.Id, ");
                query.Append("	U.Nome, ");
                query.Append("	U.Sobrenome, ");
                query.Append("	U.Sexo, ");
                query.Append("	U.DataNascimento, ");
                query.Append("	U.NumeroCelular, ");
                query.Append("	U.DataHoraCadastro, ");
                query.Append("	U.Senha, ");
                query.Append("	U.Email, ");
                query.Append("	U.IdTipoPerfil, ");
                query.Append("	UP.IdEstado, ");
                query.Append("	UP.IdCidade, ");
                query.Append("	UP.PathFotoPerfil ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosParticulares UP ON U.Id = U.Id ");
                query.Append("WHERE U.Email = @Email ");
                query.Append("	AND U.Senha = @Senha");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Senha", senha);

                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    usuarioParticular = new UsuarioParticular();
                    usuarioParticular.Id = (int)dataReader["Id"];
                    usuarioParticular.Nome = dataReader["Nome"].ToString();
                    usuarioParticular.Sobrenome = dataReader["Sobrenome"].ToString();
                    usuarioParticular.Sexo = dataReader["Sexo"].ToString();                    
                    usuarioParticular.NumeroCelular = dataReader["NumeroCelular"].ToString();
                    usuarioParticular.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                    usuarioParticular.Senha = dataReader["Senha"].ToString();
                    usuarioParticular.Email = dataReader["Email"].ToString();
                    usuarioParticular.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                    if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                        usuarioParticular.DataNascimento = null;
                    else
                        usuarioParticular.DataNascimento = (DateTime)dataReader["DataNascimento"];
                    if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                        usuarioParticular.Estado.Id = 0;
                    else
                        usuarioParticular.Estado.Id = (int)dataReader["IdEstado"];
                    if (String.IsNullOrEmpty(dataReader["IdCidade"].ToString()))
                        usuarioParticular.Municipio.Id = 0;
                    else
                        usuarioParticular.Municipio.Id = (int)dataReader["IdCidade"];
                }

                return usuarioParticular;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioParticular ObterUsuarioParticular(int idUsuarioParticular)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            UsuarioParticular usuarioParticular = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	U.Id, ");
                query.Append("	U.Nome, ");
                query.Append("	U.Sobrenome, ");
                query.Append("	U.Sexo, ");
                query.Append("	U.DataNascimento, ");
                query.Append("	U.NumeroCelular, ");
                query.Append("	U.DataHoraCadastro, ");
                query.Append("	U.Senha, ");
                query.Append("	U.Email, ");
                query.Append("	U.IdTipoPerfil, ");
                query.Append("	UP.IdEstado, ");
                query.Append("	UP.IdCidade, ");
                query.Append("	UP.PathFotoPerfil ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosParticulares UP ON U.Id = UP.Id ");
                query.Append("WHERE U.Id = @Id ");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", idUsuarioParticular);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                usuarioParticular = new UsuarioParticular();
                                usuarioParticular.Id = (int)dataReader["Id"];
                                usuarioParticular.Nome = dataReader["Nome"].ToString();
                                usuarioParticular.Sobrenome = dataReader["Sobrenome"].ToString();
                                usuarioParticular.Sexo = dataReader["Sexo"].ToString();
                                usuarioParticular.NumeroCelular = dataReader["NumeroCelular"].ToString();
                                usuarioParticular.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuarioParticular.Senha = dataReader["Senha"].ToString();
                                usuarioParticular.Email = dataReader["Email"].ToString();
                                usuarioParticular.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                                    usuarioParticular.DataNascimento = null;
                                else
                                    usuarioParticular.DataNascimento = (DateTime)dataReader["DataNascimento"];
                                if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                                    usuarioParticular.Estado.Id = 0;
                                else
                                    usuarioParticular.Estado.Id = (int)dataReader["IdEstado"];
                                if (String.IsNullOrEmpty(dataReader["IdCidade"].ToString()))
                                    usuarioParticular.Municipio.Id = 0;
                                else
                                    usuarioParticular.Municipio.Id = (int)dataReader["IdCidade"];
                            }
                        }
                    }
                }
                return usuarioParticular;
            }
            finally
            {
            }
        }
    }
}
