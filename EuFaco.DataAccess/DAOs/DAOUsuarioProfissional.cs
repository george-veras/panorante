using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using EuFaco.DataAccess.Models;
using System.Web.Configuration;

namespace EuFaco.DataAccess.DAOs
{
    public class DAOUsuarioProfissional
    {
        public bool IncluirUsuarioProfissional(UsuarioProfissional novoUsuarioProfissional)
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
                query.Append("INSERT INTO UsuariosProfissionais ( Id ) VALUES ( @Id ) ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Nome", novoUsuarioProfissional.Nome);
                command.Parameters.AddWithValue("@DataHoraCadastro", dataHoraCadastro);
                command.Parameters.AddWithValue("@Email", novoUsuarioProfissional.Email);
                command.Parameters.AddWithValue("@IdTipoPerfil", (int)Usuario.TipoPerfil.UsuarioProfissional);
                command.Parameters.AddWithValue("@Senha", novoUsuarioProfissional.Senha);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novoUsuarioProfissional.Id = (int)command.Parameters["@Id"].Value;
                    novoUsuarioProfissional.DataHoraCadastro = dataHoraCadastro;
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

        public bool IncluirUsuarioPeloFacebook(UsuarioProfissional novoUsuario)
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
                query.Append("  IdTipoPerfil ");
                query.Append(") VALUES ( ");
                query.Append("	@Nome, ");
                query.Append("  @DataHoraCadastro, ");
                query.Append("	@Email, ");
                query.Append("	@IdTipoPerfil); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO UsuariosProfissionais ( Id ) VALUES ( @Id ); ");
                query.Append("INSERT INTO FacebookAccounts ( ");
                query.Append("    IdUsuario, ");
                query.Append("    FacebookId, ");
                query.Append("    Nome, ");
                query.Append("    Email, ");
                query.Append("    DataHoraRegistro ");
                query.Append(") VALUES( ");
                query.Append("    @Id, ");
                query.Append("    @FacebookId, ");
                query.Append("    @Nome, ");
                query.Append("    @Email, ");
                query.Append("    @DataHoraCadastro); ");
                query.Append("SET @IdFacebookAccount = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Nome", novoUsuario.Facebook.Nome);
                command.Parameters.AddWithValue("@Email", novoUsuario.Facebook.Email);
                command.Parameters.AddWithValue("@DataHoraCadastro", dataHoraCadastro);
                command.Parameters.AddWithValue("@IdTipoPerfil", Usuario.TipoPerfil.UsuarioProfissional);
                command.Parameters.AddWithValue("@FacebookId", novoUsuario.Facebook.FacebookId);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@IdFacebookAccount", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novoUsuario.Id = (int)command.Parameters["@Id"].Value;
                    novoUsuario.DataHoraCadastro = dataHoraCadastro;
                    novoUsuario.Facebook.Id = (int)command.Parameters["@IdFacebookAccount"].Value;
                    novoUsuario.Facebook.DataHoraRegistro = dataHoraCadastro;
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

        public UsuarioProfissional IncluirComplementoUsuarioProfissional(Usuario usuario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraRegistro;
            UsuarioProfissional usuarioProfissional = null;
            try
            {
                dataHoraRegistro = DateTime.Now;

                query = new StringBuilder();
                query.Append("INSERT INTO UsuariosProfissionais (Id) VALUES (@Id) ");

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
                            usuarioProfissional = new UsuarioProfissional(usuario);
                        }
                    }
                }
                return usuarioProfissional;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Busca um usuário profissional específico através de seu e-mail registrado e senha, 
        /// retornando uma instância do mesmo.
        /// </summary>
        /// <param name="email">E-mail registrado do usuário buscado.</param>
        /// <param name="senha">Senha do usuário buscado.</param>
        /// <returns>Instância do usuário profissional buscado.</returns>
        /// <remarks>
        /// <para>Criado por: George Véras</para>
        /// <para>Data de Criação: 01/2016</para>
        /// <para>Alterado por: -x-</para>
        /// <para>Data da última alteração: -x-</para>
        /// </remarks>
        public UsuarioProfissional ObterUsuarioProfissional(string email, string senha)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            UsuarioProfissional usuarioProfissional = null;
            DAOAvaliacao daoAvaliacao = null;
            DAOImagem daoImagem = null;
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
                query.Append("	UPR.Resumo, ");
                query.Append("	UPR.PathFotoPerfil, ");
                query.Append("	UPR.IdEstado, ");
                query.Append("	UPR.IdMunicipio ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosProfissionais UPR ON U.Id = UPR.Id ");
                query.Append("WHERE U.Email = @Email ");
                query.Append("	AND U.Senha = @Senha");

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
                                usuarioProfissional = new UsuarioProfissional();
                                usuarioProfissional.Id = (int)dataReader["Id"];
                                usuarioProfissional.Nome = dataReader["Nome"].ToString();
                                usuarioProfissional.Sobrenome = dataReader["Sobrenome"].ToString();
                                usuarioProfissional.Sexo = dataReader["Sexo"].ToString();
                                usuarioProfissional.NumeroCelular = dataReader["NumeroCelular"].ToString();
                                usuarioProfissional.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuarioProfissional.Senha = dataReader["Senha"].ToString();
                                usuarioProfissional.Email = dataReader["Email"].ToString();
                                usuarioProfissional.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                                usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                                if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                                    usuarioProfissional.DataNascimento = null;
                                else
                                    usuarioProfissional.DataNascimento = (DateTime)dataReader["DataNascimento"];
                                if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                                    usuarioProfissional.Estado.Id = 0;
                                else
                                    usuarioProfissional.Estado.Id = (int)dataReader["IdEstado"];
                                if (String.IsNullOrEmpty(dataReader["IdMunicipio"].ToString()))
                                    usuarioProfissional.Municipio.Id = 0;
                                else
                                    usuarioProfissional.Municipio.Id = (int)dataReader["IdMunicipio"];
                            }
                        }
                    }
                }
                daoAvaliacao = new DAOAvaliacao();
                daoAvaliacao.PopularAvaliacoes(usuarioProfissional);

                daoImagem = new DAOImagem();
                daoImagem.PopularImagens(usuarioProfissional);

                return usuarioProfissional;
            }
            finally
            {
                if (daoAvaliacao != null)
                    daoAvaliacao = null;

                if (daoImagem != null)
                    daoImagem = null;
            }
        }

        /// <summary>
        /// Busca um usuário profissional específico através de seu e-mail registrado e senha, 
        /// retornando uma instância do mesmo.
        /// </summary>
        /// <param name="idUsuarioProfissional">Id do registro do profissional.</param>
        /// <returns>Instância do usuário profissional buscado.</returns>
        /// <remarks>
        /// <para>Criado por: George Véras</para>
        /// <para>Data de Criação: 01/2016</para>
        /// <para>Alterado por: -x-</para>
        /// <para>Data da última alteração: -x-</para>
        /// </remarks>
        public UsuarioProfissional ObterUsuarioProfissional(int idUsuarioProfissional)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            UsuarioProfissional usuarioProfissional = null;
            DAOAvaliacao daoAvaliacao = null;
            DAOImagem daoImagem = null;
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
                query.Append("	UPR.Resumo, ");
                query.Append("	UPR.PathFotoPerfil, ");
                query.Append("	UPR.IdEstado, ");
                query.Append("	UPR.IdMunicipio ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosProfissionais UPR ON U.Id = UPR.Id ");
                query.Append("WHERE U.Id = @Id");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@Id", idUsuarioProfissional);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                usuarioProfissional = new UsuarioProfissional();
                                usuarioProfissional.Id = (int)dataReader["Id"];
                                usuarioProfissional.Nome = dataReader["Nome"].ToString();
                                usuarioProfissional.Sobrenome = dataReader["Sobrenome"].ToString();
                                usuarioProfissional.Sexo = dataReader["Sexo"].ToString();
                                usuarioProfissional.NumeroCelular = dataReader["NumeroCelular"].ToString();
                                usuarioProfissional.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuarioProfissional.Senha = dataReader["Senha"].ToString();
                                usuarioProfissional.Email = dataReader["Email"].ToString();
                                usuarioProfissional.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                                usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                                if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                                    usuarioProfissional.DataNascimento = null;
                                else
                                    usuarioProfissional.DataNascimento = (DateTime)dataReader["DataNascimento"];
                                if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                                    usuarioProfissional.Estado.Id = 0;
                                else
                                    usuarioProfissional.Estado.Id = (int)dataReader["IdEstado"];
                                if (String.IsNullOrEmpty(dataReader["IdMunicipio"].ToString()))
                                    usuarioProfissional.Municipio.Id = 0;
                                else
                                    usuarioProfissional.Municipio.Id = (int)dataReader["IdMunicipio"];
                            }
                        }
                    }
                }
                daoAvaliacao = new DAOAvaliacao();
                daoAvaliacao.PopularAvaliacoes(usuarioProfissional);

                daoImagem = new DAOImagem();
                daoImagem.PopularImagens(usuarioProfissional);

                return usuarioProfissional;
            }
            finally
            {
                if (daoAvaliacao != null)
                    daoAvaliacao = null;

                if (daoImagem != null)
                    daoImagem = null;
            }
        }

        /// <summary>
        /// Obtém uma lista de todos os perfis profissionais no banco de dados.
        /// </summary>
        /// <returns>Uma lista com todos os registros de perfis profissionais no banco dados.</returns>
        /// <remarks>
        /// <para>Criado por: George Véras</para>
        /// <para>Data de Criação: 01/2016</para>
        /// <para>Alterado por: -x-</para>
        /// <para>Data da última alteração: -x-</para>
        /// </remarks>
        public List<UsuarioProfissional> ObterProfissionais()
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            UsuarioProfissional usuarioProfissional = null;
            List<UsuarioProfissional> listaProfissionais = null;
            try
            {
                listaProfissionais = new List<UsuarioProfissional>();

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
                query.Append("	UPR.Resumo, ");
                query.Append("	UPR.PathFotoPerfil, ");
                query.Append("	UPR.IdEstado, ");
                query.Append("	UPR.IdMunicipio ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosProfissionais UPR ON U.Id = UPR.Id ");
                query.Append("WHERE U.IdTipoPerfil = @IdTipoPerfil ");

                connectionString = WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = query.ToString();
                        command.Parameters.AddWithValue("@IdTipoPerfil", 2);

                        connection.Open();
                        using (dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                usuarioProfissional = new UsuarioProfissional();
                                usuarioProfissional.Id = (int)dataReader["Id"];
                                usuarioProfissional.Nome = dataReader["Nome"].ToString();
                                usuarioProfissional.Sobrenome = dataReader["Sobrenome"].ToString();
                                usuarioProfissional.Sexo = dataReader["Sexo"].ToString();
                                usuarioProfissional.NumeroCelular = dataReader["NumeroCelular"].ToString();
                                usuarioProfissional.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuarioProfissional.Email = dataReader["Email"].ToString();
                                usuarioProfissional.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                                usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                                if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                                    usuarioProfissional.DataNascimento = null;
                                else
                                    usuarioProfissional.DataNascimento = (DateTime)dataReader["DataNascimento"];
                                if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                                    usuarioProfissional.Estado.Id = 0;
                                else
                                    usuarioProfissional.Estado.Id = (int)dataReader["IdEstado"];
                                if (String.IsNullOrEmpty(dataReader["IdMunicipio"].ToString()))
                                    usuarioProfissional.Municipio.Id = 0;
                                else
                                    usuarioProfissional.Municipio.Id = (int)dataReader["IdMunicipio"];

                                listaProfissionais.Add(usuarioProfissional);
                            }
                        }
                    }
                }
                return listaProfissionais;
            }
            finally
            {
                if (query != null)
                    query = null;

                if (usuarioProfissional != null)
                    usuarioProfissional = null;

                if (listaProfissionais != null)
                    listaProfissionais = null;
            }
        }

        public List<UsuarioProfissional> ObterProfissionaisPorNome(string nome)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            UsuarioProfissional usuarioProfissional = null;
            SqlDataReader dataReader = null;
            List<UsuarioProfissional> listaProfissionais = null;
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
                query.Append("	UPR.Resumo, ");
                query.Append("	UPR.PathFotoPerfil, ");
                query.Append("	UPR.IdEstado, ");
                query.Append("	UPR.IdMunicipio ");
                query.Append("FROM Usuarios U INNER JOIN UsuariosProfissionais UPR ON U.Id = UPR.Id ");
                query.Append("WHERE U.IdTipoPerfil = @IdTipoPerfil ");
                query.Append("  AND U.Nome LIKE @Nome");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@IdTipoPerfil", 2);
                command.Parameters.AddWithValue("@Nome", '%' + nome + '%');

                dataReader = command.ExecuteReader();

                listaProfissionais = new List<UsuarioProfissional>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        usuarioProfissional = new UsuarioProfissional();
                        usuarioProfissional.Id = (int)dataReader["Id"];
                        usuarioProfissional.Nome = dataReader["Nome"].ToString();
                        usuarioProfissional.Sobrenome = dataReader["Sobrenome"].ToString();
                        usuarioProfissional.Sexo = dataReader["Sexo"].ToString();
                        usuarioProfissional.NumeroCelular = dataReader["NumeroCelular"].ToString();
                        usuarioProfissional.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                        usuarioProfissional.Email = dataReader["Email"].ToString();
                        usuarioProfissional.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                        usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                        usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                        if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                            usuarioProfissional.DataNascimento = null;
                        else
                            usuarioProfissional.DataNascimento = (DateTime)dataReader["DataNascimento"];
                        if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                            usuarioProfissional.Estado.Id = 0;
                        else
                            usuarioProfissional.Estado.Id = (int)dataReader["IdEstado"];
                        if (String.IsNullOrEmpty(dataReader["IdMunicipio"].ToString()))
                            usuarioProfissional.Municipio.Id = 0;
                        else
                            usuarioProfissional.Municipio.Id = (int)dataReader["IdMunicipio"];

                        listaProfissionais.Add(usuarioProfissional);
                    }
                }

                return listaProfissionais;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EditarResumo(string novoResumo, int idUsuario)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            try
            {
                query = new StringBuilder();
                query.Append("UPDATE UsuariosProfissionais SET ");
                query.Append("	Resumo = @Resumo ");
                query.Append("WHERE Id = @Id");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Resumo", novoResumo);
                command.Parameters.AddWithValue("@Id", idUsuario);

                if (command.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioProfissional ObterUsuarioProfissionalPorFacebookAccount(string facebookId)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            DAOAvaliacao daoAvaliacao = null;
            DAOImagem daoImagem = null;
            UsuarioProfissional usuarioProfissional = null;
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
                query.Append("	UPR.Resumo, ");
                query.Append("	UPR.PathFotoPerfil, ");
                query.Append("	UPR.IdEstado, ");
                query.Append("	UPR.IdMunicipio, ");
                query.Append("  F.Nome AS NomeFacebook, ");
                query.Append("  F.Email AS EmailFacebook, ");
                query.Append("  F.DataHoraRegistro AS DataHoraRegistroFacebookLogin, ");
                query.Append("  F.FacebookId ");
                query.Append("FROM Usuarios U ");
                query.Append("  INNER JOIN UsuariosProfissionais UPR ON U.Id = UPR.Id ");
                query.Append("  INNER JOIN FacebookAccounts F ON U.Id = F.IdUsuario ");
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
                                usuarioProfissional = new UsuarioProfissional();
                                usuarioProfissional.Id = (int)dataReader["Id"];
                                usuarioProfissional.Nome = dataReader["Nome"].ToString();
                                usuarioProfissional.Sobrenome = dataReader["Sobrenome"].ToString();
                                usuarioProfissional.Sexo = dataReader["Sexo"].ToString();
                                usuarioProfissional.NumeroCelular = dataReader["NumeroCelular"].ToString();
                                usuarioProfissional.DataHoraCadastro = (DateTime)dataReader["DataHoraCadastro"];
                                usuarioProfissional.Senha = dataReader["Senha"].ToString();
                                usuarioProfissional.Email = dataReader["Email"].ToString();
                                usuarioProfissional.Perfil = (Usuario.TipoPerfil)dataReader["IdTipoPerfil"];
                                usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                                usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                                usuarioProfissional.Facebook.FacebookId = dataReader["FacebookId"].ToString();
                                usuarioProfissional.Facebook.Nome = dataReader["NomeFacebook"].ToString();
                                usuarioProfissional.Facebook.Email = dataReader["EmailFacebook"].ToString();
                                usuarioProfissional.Facebook.DataHoraRegistro = (DateTime)dataReader["DataHoraRegistroFacebookLogin"];
                                if (String.IsNullOrEmpty(dataReader["DataNascimento"].ToString()))
                                    usuarioProfissional.DataNascimento = null;
                                else
                                    usuarioProfissional.DataNascimento = (DateTime)dataReader["DataNascimento"];
                                if (String.IsNullOrEmpty(dataReader["IdEstado"].ToString()))
                                    usuarioProfissional.Estado.Id = 0;
                                else
                                    usuarioProfissional.Estado.Id = (int)dataReader["IdEstado"];
                                if (String.IsNullOrEmpty(dataReader["IdMunicipio"].ToString()))
                                    usuarioProfissional.Municipio.Id = 0;
                                else
                                    usuarioProfissional.Municipio.Id = (int)dataReader["IdMunicipio"];
                            }
                        }
                    }
                }
                daoAvaliacao = new DAOAvaliacao();
                daoAvaliacao.PopularAvaliacoes(usuarioProfissional);

                daoImagem = new DAOImagem();
                daoImagem.PopularImagens(usuarioProfissional);

                return usuarioProfissional;
            }
            finally
            {
                if (daoAvaliacao != null)
                    daoAvaliacao = null;

                if (daoImagem != null)
                    daoImagem = null;
            }
        }

        public UsuarioProfissional CompletarPerfilUsuarioProfissional(Usuario usuario)
        {
            string connectionString;
            StringBuilder query = null;
            SqlConnection connection = null;
            SqlCommand command = null;
            DateTime dataHoraRegistro;
            UsuarioProfissional usuarioProfissional = null;
            SqlDataReader dataReader = null;
            try
            {
                dataHoraRegistro = DateTime.Now;

                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Resumo, ");
                query.Append("	PathFotoPerfil ");
                query.Append("FROM UsuariosProfissionais ");
                query.Append("WHERE Id = @Id");

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
                                usuarioProfissional = new UsuarioProfissional(usuario);
                                usuarioProfissional.Resumo = dataReader["Resumo"].ToString();
                                usuarioProfissional.PathFotoPerfil = dataReader["PathFotoPerfil"].ToString();
                            }
                        }
                    }
                }
                return usuarioProfissional;
            }
            finally
            {
            }
        }
    }
}
