using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Imagem
    {
        private int _Id;
        private string _PathImagem;
        private string _Legenda;
        private Usuario _Dono;
        private DateTime _DataHoraInclusao;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string PathImagem
        {
            get { return _PathImagem; }
            set { _PathImagem = value; }
        }

        public string Legenda
        {
            get { return _Legenda; }
            set { _Legenda = value; }
        }

        public Usuario Dono
        {
            get { return _Dono; }
            set { _Dono = value; }
        }

        public DateTime DataHoraInclusao
        {
            get { return _DataHoraInclusao; }
            set { _DataHoraInclusao = value; }
        }
    }
}
