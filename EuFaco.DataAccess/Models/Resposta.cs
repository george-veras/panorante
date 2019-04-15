using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Resposta
    {
        #region Campos
        private int _Id;
        private string _Texto;
        private Usuario _Autor;
        private Pergunta _Pergunta;
        private DateTime _DataHoraRespondida;
        private List<ComentarioResposta> _Comentarios;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
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

        public Pergunta Pergunta
        {
            get { return _Pergunta; }
            set { _Pergunta = value; }
        }

        public DateTime DataHoraRespondida
        {
            get { return _DataHoraRespondida; }
            set { _DataHoraRespondida = value; }
        }

        public List<ComentarioResposta> Comentarios
        {
            get { return _Comentarios; }
            set { _Comentarios = value; }
        }
        #endregion

        public Resposta()
        {
            this.Autor = new Usuario();
            this.Pergunta = new Pergunta();
            this.Comentarios = new List<ComentarioResposta>();
        }
    }
}
