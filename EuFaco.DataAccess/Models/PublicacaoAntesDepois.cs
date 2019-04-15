using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace EuFaco.DataAccess.Models
{
    public class PublicacaoAntesDepois : Publicacao
    {
        #region Campos
        private string _Titulo;
        private string _Texto;
        private string _PathImagemAntes;
        private string _PathImagemDepois;
        private List<string> _Etiquetas;
        #endregion

        #region Propriedades
        public string Titulo
        {
            get
            {
                return _Titulo;
            }

            set
            {
                _Titulo = value;
            }
        }

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

        public string PathImagemAntes
        {
            get
            {
                return _PathImagemAntes;
            }

            set
            {
                _PathImagemAntes = value;
            }
        }

        public string PathImagemDepois
        {
            get
            {
                return _PathImagemDepois;
            }

            set
            {
                _PathImagemDepois = value;
            }
        }

        public List<string> Etiquetas
        {
            get
            {
                return _Etiquetas;
            }

            set
            {
                _Etiquetas = value;
            }
        }
        #endregion

        public PublicacaoAntesDepois()
        {
            this.Tipo = TipoPublicacao.PublicacaoAntesDepois;
            this.Etiquetas = new List<string>();
        }
    }
}
