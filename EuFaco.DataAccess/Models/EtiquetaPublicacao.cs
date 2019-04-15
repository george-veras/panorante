using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class EtiquetaPublicacao
    {
        #region Campos
        private int _Id;
        private string _Nome;
        private int _Quantidade;
        private DateTime _DataHoraInclusao;
        #endregion

        #region Propriedades
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

        public string Nome
        {
            get
            {
                return _Nome;
            }

            set
            {
                _Nome = value;
            }
        }

        public int Quantidade
        {
            get
            {
                return _Quantidade;
            }

            set
            {
                _Quantidade = value;
            }
        }

        public DateTime DataHoraInclusao
        {
            get
            {
                return _DataHoraInclusao;
            }

            set
            {
                _DataHoraInclusao = value;
            }
        }
        #endregion

        public EtiquetaPublicacao() { }
    }
}
