using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class ComentarioResposta
    {
        #region Campos
        private int _Id;
        private string _Texto;
        private Usuario _Autor;
        private Resposta _RespostaComentada;
        private DateTime _DataHoraComentada;
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

        public Resposta RespostaComentada
        {
            get { return _RespostaComentada; }
            set { _RespostaComentada = value; }
        }

        public DateTime DataHoraComentada
        {
            get { return _DataHoraComentada; }
            set { _DataHoraComentada = value; }
        }
        #endregion

        public ComentarioResposta()
        {
            this.Autor = new Usuario();
            this.RespostaComentada = new Resposta();
        }
    }
}
