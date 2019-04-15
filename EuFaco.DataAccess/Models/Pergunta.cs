using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Pergunta
    {
        #region Campos
        private int _Id;
        private Usuario _Autor;
        private string _Titulo;
        private string _Texto;
        private DateTime _DataHoraPerguntada;
        private List<Resposta> _Respostas;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Usuario Autor
        {
            get { return _Autor; }
            set { _Autor = value; }
        }

        public string Titulo
        {
            get { return _Titulo; }
            set { _Titulo = value; }
        }

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public DateTime DataHoraPerguntada
        {
            get { return _DataHoraPerguntada; }
            set { _DataHoraPerguntada = value; }
        }

        public List<Resposta> Respostas
        {
            get { return _Respostas; }
            set { _Respostas = value; }
        }
        #endregion
    }
}
