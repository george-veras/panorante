using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class PublicacaoDicas : Publicacao
    {
        #region Campos
        private string _Texto;
        #endregion

        #region Propriedades
        public string Texto
        {
            get
            {
                return _Texto;
            }

            set
            {
                _Texto = value;
            }
        }
        #endregion
    }
}
