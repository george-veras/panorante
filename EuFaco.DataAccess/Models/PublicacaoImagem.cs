using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class PublicacaoImagem : Publicacao
    {
        #region Campos
        private Imagem _Imagem;
        private List<ComentarioPublicacao> _Comentarios;
        private List<EtiquetaPublicacao> _Etiquetas;
        private int _QuantidadeVisualizacoes;
        #endregion

        #region Propriedades
        public Imagem Imagem
        {
            get
            {
                return _Imagem;
            }

            set
            {
                _Imagem = value;
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

        public int QuantidadeVisualizacoes
        {
            get
            {
                return _QuantidadeVisualizacoes;
            }
            set
            {
                _QuantidadeVisualizacoes = value;
            }
        }
        #endregion

        public PublicacaoImagem()
        {
            this.Tipo = TipoPublicacao.PublicacaoImagem;
            this.Comentarios = new List<ComentarioPublicacao>();
            this.Etiquetas = new List<EtiquetaPublicacao>();
        }
    }
}
