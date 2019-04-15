using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class FacebookAccount
    {
        #region Campos
        private int _Id;
        private Usuario _Usuario;
        private string _FacebookId;
        private string _Nome;
        private string _Email;
        private string _AccessToken;
        private DateTime _DataHoraRegistro;
        #endregion

        #region Propriedades
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

        public Usuario Usuario
        {
            get
            {
                return _Usuario;
            }

            set
            {
                _Usuario = value;
            }
        }

        public string FacebookId
        {
            get
            {
                return _FacebookId;
            }

            set
            {
                _FacebookId = value;
            }
        }

        public string Nome
        {
            get
            {
                return _Nome;
            }

            set
            {
                _Nome = value;
            }
        }

        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }

        public string AccessToken
        {
            get
            {
                return _AccessToken;
            }

            set
            {
                _AccessToken = value;
            }
        }

        public DateTime DataHoraRegistro
        {
            get
            {
                return _DataHoraRegistro;
            }

            set
            {
                _DataHoraRegistro = value;
            }
        }
        #endregion

        public FacebookAccount() { }

        public FacebookAccount(Usuario usuario)
        {
            this.Usuario = usuario;
        }
    }
}
