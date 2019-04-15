using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Publicacao
    {
        public enum TipoPublicacao
        {
            PublicacaoImagem = 1,
            PublicacaoProjeto = 2,
            PublicacaoAntesDepois = 3
        }

        #region Campos
        private int _Id;
        protected TipoPublicacao _Tipo;
        private DateTime _DataHoraPublicado;
        private Usuario _Autor;
        #endregion

        #region Atributos
        public int Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        public TipoPublicacao Tipo
        {
            get
            {
                return _Tipo;
            }

            protected set
            {
                _Tipo = value;
            }
        }

        public DateTime DataHoraPublicado
        {
            get
            {
                return _DataHoraPublicado;
            }

            set
            {
                _DataHoraPublicado = value;
            }
        }

        public Usuario Autor
        {
            get
            {
                return _Autor;
            }

            set
            {
                _Autor = value;
            }
        }
        #endregion
    }
}
