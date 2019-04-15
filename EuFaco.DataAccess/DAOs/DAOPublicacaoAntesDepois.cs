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
    public class DAOPublicacaoAntesDepois
    {
        public bool IncluirPublicacaoAntesDepois(PublicacaoAntesDepois novaPublicacaoAntesDepois)
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
                query.Append("INSERT INTO Publicacoes ( ");
                query.Append("	IdTipo, ");
                query.Append("	DataHoraPublicado, ");
                query.Append("	IdUsuario ");
                query.Append(") VALUES ( ");
                query.Append("	@IdTipo, ");
                query.Append("	@DataHoraPublicado, ");
                query.Append("	@IdUsuario); ");
                query.Append("SET @IdPublicacao = SCOPE_IDENTITY(); ");
                query.Append("INSERT INTO PublicacoesAntesDepois ( ");
                query.Append("	Id, ");
                query.Append("	PathImagemAntes, ");
                query.Append("	PathImagemDepois, ");
                query.Append("	Titulo, ");
                query.Append("	Etiquetas, ");
                query.Append("	Texto ");
                query.Append(") VALUES ( ");
                query.Append("	@IdPublicacao, ");
                query.Append("	@PathImagemAntes, ");
                query.Append("	@PathImagemDepois, ");
                query.Append("	@Titulo, ");
                query.Append("	@Etiquetas, ");
                query.Append("	@Texto); ");
                query.Append("COMMIT");

                connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionEstudo"].ConnectionString);
                connection.Open();

                command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query.ToString();

                command.Parameters.AddWithValue("@IdTipo", (int)novaPublicacaoAntesDepois.Tipo);
                command.Parameters.AddWithValue("@DataHoraPublicado", dataHoraRegistrada);
                command.Parameters.AddWithValue("@IdUsuario", novaPublicacaoAntesDepois.Autor.Id);
                command.Parameters.AddWithValue("@PathImagemAntes", novaPublicacaoAntesDepois.PathImagemAntes);
                command.Parameters.AddWithValue("@PathImagemDepois", novaPublicacaoAntesDepois.PathImagemDepois);
                command.Parameters.AddWithValue("@Titulo", novaPublicacaoAntesDepois.Titulo);
                command.Parameters.AddWithValue("@Texto", novaPublicacaoAntesDepois.Texto);
                command.Parameters.AddWithValue("@Etiquetas", String.Join(",", novaPublicacaoAntesDepois.Etiquetas));
                command.Parameters.Add("@IdPublicacao", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (command.ExecuteNonQuery() > 0)
                {
                    novaPublicacaoAntesDepois.Id = (int)command.Parameters["@IdPublicacao"].Value;
                    novaPublicacaoAntesDepois.DataHoraPublicado = dataHoraRegistrada;
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
