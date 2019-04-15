using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Candidatura
    {
        #region Campos
        private int _Id;
        private Servico _ServicoCandidatado;
        private Usuario _Candidato;
        private string _Mensagem;
        private DateTime _DataHoraCandidatura;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Servico ServicoCandidatado
        {
            get { return _ServicoCandidatado; }
            set { _ServicoCandidatado = value; }
        }

        public Usuario Candidato
        {
            get { return _Candidato; }
            set { _Candidato = value; }
        }

        public string Mensagem
        {
            get { return _Mensagem; }
            set { _Mensagem = value; }
        }

        public DateTime DataHoraCandidatura
        {
            get { return _DataHoraCandidatura; }
            set { _DataHoraCandidatura = value; }
        }
        #endregion

        public Candidatura()
        {
            this.ServicoCandidatado = new Servico();
            this.Candidato = new Usuario();
        }
    }
}
