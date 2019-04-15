using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Servico
    {
        #region Campos
        private int _Id;
        private Usuario _Solicitante;
        private string _Titulo;
        private string _Descricao;
        private DateTime _DataHoraSolicitacao;
        private List<Candidatura> _Candidaturas;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Usuario Solicitante
        {
            get { return _Solicitante; }
            set { _Solicitante = value; }
        }

        public string Titulo
        {
            get { return _Titulo; }
            set { _Titulo = value; }
        }

        public string Descricao
        {
            get { return _Descricao; }
            set { _Descricao = value; }
        }

        public DateTime DataHoraSolicitacao
        {
            get { return _DataHoraSolicitacao; }
            set { _DataHoraSolicitacao = value; }
        }

        public List<Candidatura> Candidaturas
        {
            get { return _Candidaturas; }
            set { _Candidaturas = value; }
        }
        #endregion

        public Servico()
        {
            this.Solicitante = new Usuario();
            this.Candidaturas = new List<Candidatura>();
        }
    }
}
