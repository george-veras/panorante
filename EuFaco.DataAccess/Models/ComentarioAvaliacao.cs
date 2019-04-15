using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class ComentarioAvaliacao
    {
        #region Campos
        private int _Id;
        private Avaliacao _Avaliacao;
        private string _Texto;
        private Usuario _Autor;
        private DateTime _DataHora;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Avaliacao Avaliacao
        {
            get { return _Avaliacao; }
            set { _Avaliacao = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public Usuario Autor
        {
            get { return _Autor; }
            set { _Autor = value; }
        }

        public DateTime DataHora
        {
            get { return _DataHora; }
            set { _DataHora = value; }
        }
        #endregion
    }
}
