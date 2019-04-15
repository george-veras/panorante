using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace EuFaco.DataAccess.Models
{
    public class PublicacaoProjeto : Publicacao
    {
        #region Campos
        private string _Titulo;
        private string _PathImagemCapa;
        private List<ItemPublicacao> _Conteudo;
        private List<EtiquetaPublicacao> _Etiquetas;
        private List<ComentarioPublicacao> _Comentarios;
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

        public string PathImagemCapa
        {
            get
            {
                return _PathImagemCapa;
            }

            set
            {
                _PathImagemCapa = value;
            }
        }

        public List<ItemPublicacao> Conteudo
        {
            get
            {
                return _Conteudo;
            }

            set
            {
                _Conteudo = value;
            }
        }

        public List<EtiquetaPublicacao> Etiquetas
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

        public List<ComentarioPublicacao> Comentarios
        {
            get
            {
                return _Comentarios;
            }

            set
            {
                _Comentarios = value;
            }
        }
        #endregion

        public PublicacaoProjeto()
        {
            this.Tipo = TipoPublicacao.PublicacaoProjeto;
            this.Conteudo = new List<ItemPublicacao>();
            this.Etiquetas = new List<EtiquetaPublicacao>();
            this.Comentarios = new List<ComentarioPublicacao>();
        }
    }
}
