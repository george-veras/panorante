using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Mensagem
    {
        #region Campos
        private int _Id;
        private Usuario _Destinatario;
        private Usuario _Remetente;
        private string _Conteudo;
        private DateTime _DataHoraEnvio;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Usuario Destinatario
        {
            get { return _Destinatario; }
            set { _Destinatario = value; }
        }

        public Usuario Remetente
        {
            get { return _Remetente; }
            set { _Remetente = value; }
        }

        public string Conteudo
        {
            get { return _Conteudo; }
            set { _Conteudo = value; }
        }

        public DateTime DataHoraEnvio
        {
            get { return _DataHoraEnvio; }
            set { _DataHoraEnvio = value; }
        }
        #endregion

        public Mensagem()
        {
            this.Destinatario = new Usuario();
            this.Remetente = new Usuario();
        }
    }
}