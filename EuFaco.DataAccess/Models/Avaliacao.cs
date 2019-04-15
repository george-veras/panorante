using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Avaliacao
    {
        #region Campos
        private int _Id;
        private Usuario _Avaliador;
        private Usuario _Avaliado;
        private string _Resumo;
        private string _Texto;
        private DateTime _DataHoraAvaliacao;
        private byte _Nota;
        private List<ComentarioAvaliacao> _Comentarios;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Usuario Avaliador
        {
            get { return _Avaliador; }
            set { _Avaliador = value; }
        }

        public Usuario Avaliado
        {
            get { return _Avaliado; }
            set { _Avaliado = value; }
        }

        public string Resumo
        {
            get { return _Resumo; }
            set { _Resumo = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public DateTime DataHoraAvaliacao
        {
            get { return _DataHoraAvaliacao; }
            set { _DataHoraAvaliacao = value; }
        }

        public byte Nota
        {
            get { return _Nota; }
            set { _Nota = value; }
        }

        public List<ComentarioAvaliacao> Comentarios
        {
            get { return _Comentarios; }
            set { _Comentarios = value; }
        }
        #endregion

        public Avaliacao()
        {
            this.Avaliador = new Usuario();
            this.Avaliado = new Usuario();
            this.Comentarios = new List<ComentarioAvaliacao>();
        }
    }
}
