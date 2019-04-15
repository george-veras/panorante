using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class ComentarioPublicacao
    {
        private int _Id;
        private Usuario _Autor;
        private string _Texto;
        private DateTime _DataHoraComentada;

        public int Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        public Usuario Autor
        {
            get
            {
                return _Autor;
            }

            set
            {
                _Autor = value;
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

        public DateTime DataHoraComentada
        {
            get
            {
                return _DataHoraComentada;
            }

            set
            {
                _DataHoraComentada = value;
            }
        }
    }
}
