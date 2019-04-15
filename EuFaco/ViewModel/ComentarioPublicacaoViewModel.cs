using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EuFaco.ViewModel
{
    public class ComentarioPublicacaoViewModel
    {
        private string _Texto;

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
    }
}