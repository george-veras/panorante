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
    public class DAOImagem
    {
        public void PopularImagens(Usuario usuarioDono)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Imagem imagem = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("	Id, ");
                query.Append("	PathImagem, ");
                query.Append("	Legenda, ");
                query.Append("	IdUsuario, ");
                query.Append("	DataHoraInclusao ");
                query.Append("FROM Imagens ");
                query.Append("WHERE IdUsuario = @IdUsuario");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdUsuario", usuarioDono.Id);

                dataReader = command.ExecuteReader();
                
                if (dataReader.HasRows)
                {
                    usuarioDono.Imagens = new List<Imagem>();
                    while (dataReader.Read())
                    {
                        imagem = new Imagem();

                        imagem.Id = (int)dataReader["Id"];
                        imagem.PathImagem = dataReader["PathImagem"].ToString();
                        imagem.Legenda = dataReader["Legenda"].ToString();
                        imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusao"];
                        imagem.Dono = usuarioDono;

                        usuarioDono.Imagens.Add(imagem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IncluirImagem(Imagem novaImagem)
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
                query.Append("INSERT INTO Imagens ( ");
                query.Append("	Legenda, ");
                query.Append("	IdUsuario, ");
                query.Append("	DataHoraInclusao, ");
                query.Append("	PathImagem ");
                query.Append(") VALUES ( ");
                query.Append("	@Legenda, ");
                query.Append("	@IdUsuario, ");
                query.Append("	@DataHoraInclusao, ");
                query.Append("	@PathImagem); ");
                query.Append("SET @Id = SCOPE_IDENTITY(); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                
                command.Parameters.AddWithValue("@IdUsuario", novaImagem.Dono.Id);
                command.Parameters.AddWithValue("@DataHoraInclusao", dataHoraRegistrada);
                command.Parameters.AddWithValue("@PathImagem", novaImagem.PathImagem);
                command.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (String.IsNullOrEmpty(novaImagem.Legenda))
                    command.Parameters.AddWithValue("@Legenda", String.Empty);
                else
                    command.Parameters.AddWithValue("@Legenda", novaImagem.Legenda);

                if (command.ExecuteNonQuery() > 0)
                {
                    novaImagem.Id = (int)command.Parameters["@Id"].Value;
                    novaImagem.DataHoraInclusao = dataHoraRegistrada;
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

        public bool EditarLegenda(int idImagem, string novoTexto)
        {
            SqlConnection connection = null;
            StringBuilder query = null;
            SqlCommand command = null;
            try
            {
                query = new StringBuilder();
                query.Append("UPDATE Imagens SET ");
                query.Append("	Legenda = @Legenda ");
                query.Append("WHERE Id = @Id");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@Legenda", novoTexto);
                command.Parameters.AddWithValue("@Id", idImagem);

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

        public Imagem ObterImagem(int idImagem)
        {
            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader dataReader = null;
            StringBuilder query = null;
            Imagem imagem = null;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("  Id, ");
                query.Append("	Legenda, ");
                query.Append("	IdUsuario, ");
                query.Append("	DataHoraInclusao, ");
                query.Append("	PathImagem ");
                query.Append("FROM Imagens ");
                query.Append("WHERE Id = @Id");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();
                command.Parameters.AddWithValue("@Id", idImagem);

                dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    imagem = new Imagem();

                    imagem.Id = (int)dataReader["Id"];
                    imagem.Legenda = dataReader["Legenda"].ToString();
                    imagem.DataHoraInclusao = (DateTime)dataReader["DataHoraInclusao"];
                    imagem.PathImagem = dataReader["PathImagem"].ToString();
                }

                return imagem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
