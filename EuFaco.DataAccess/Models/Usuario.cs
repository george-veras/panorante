using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Usuario
    {
        public enum TipoPerfil
        {
            PerfilIndefinido = 0,
            UsuarioParticular = 1,
            UsuarioProfissional = 2,
            Visitante = 3
        }

        #region Campos
        private int _Id;
        private string _Nome;
        private string _Sobrenome;
        private string _Sexo;
        private DateTime? _DataNascimento;
        private string _NumeroCelular;
        private DateTime _DataHoraCadastro;
        private string _Senha;
        private string _Email;
        private TipoPerfil _Perfil;
        private List<Imagem> _Imagens;
        private FacebookAccount _Facebook;
        #endregion

        #region Atributos
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Nome
        {
            get { return _Nome; }
            set { _Nome = value; }
        }

        public string Sobrenome
        {
            get { return _Sobrenome; }
            set { _Sobrenome = value; }
        }

        public string Sexo
        {
            get { return _Sexo; }
            set { _Sexo = value; }
        }

        public DateTime? DataNascimento
        {
            get { return _DataNascimento; }
            set { _DataNascimento = value; }
        }

        public string NumeroCelular
        {
            get { return _NumeroCelular; }
            set { _NumeroCelular = value; }
        }

        public DateTime DataHoraCadastro
        {
            get { return _DataHoraCadastro; }
            set { _DataHoraCadastro = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string Senha
        {
            get { return _Senha; }
            set { _Senha = value; }
        }

        public TipoPerfil Perfil
        {
            get { return _Perfil; }
            set { _Perfil = value; }
        }

        public List<Imagem> Imagens
        {
            get { return _Imagens; }
            set { _Imagens = value; }
        }

        public FacebookAccount Facebook
        {
            get
            {
                return _Facebook;
            }

            set
            {
                _Facebook = value;
            }
        }
        #endregion

        public Usuario()
        {
            this.Perfil = TipoPerfil.PerfilIndefinido;
            this.Imagens = new List<Imagem>();
        }

        public Usuario(FacebookAccount facebook)
        {
            this.Perfil = TipoPerfil.PerfilIndefinido;
            this.Imagens = new List<Imagem>();
            this.Facebook = facebook;
        }
    }
}
