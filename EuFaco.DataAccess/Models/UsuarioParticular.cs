using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class UsuarioParticular : Usuario
    {
        #region Campos
        private Estado _Estado;
        private Municipio _Municipio;
        private string _PathFotoPerfil;
        private List<Publicacao> _Publicacoes;
        #endregion

        #region Atributos
        public Estado Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        public Municipio Municipio
        {
            get { return _Municipio; }
            set { _Municipio = value; }
        }

        public string PathFotoPerfil
        {
            get { return _PathFotoPerfil; }
            set { _PathFotoPerfil = value; }
        }

        public List<Publicacao> Publicacoes
        {
            get
            {
                return _Publicacoes;
            }

            set
            {
                _Publicacoes = value;
            }
        }
        #endregion

        public UsuarioParticular()
        {
            this.Estado = new Estado();
            this.Municipio = new Municipio();
        }

        public UsuarioParticular(Usuario usuario)
        {
            this.Id = usuario.Id;
            this.Nome = usuario.Nome;
            this.Sobrenome = usuario.Sobrenome;
            this.Sexo = usuario.Sexo;
            this.DataNascimento = usuario.DataNascimento;
            this.NumeroCelular = usuario.NumeroCelular;
            this.DataHoraCadastro = usuario.DataHoraCadastro;
            this.Senha = usuario.Senha;
            this.Email = usuario.Email;
            this.Perfil = usuario.Perfil;
            this.Imagens = usuario.Imagens;
            this.Facebook = usuario.Facebook;
            this.Estado = new Estado();
            this.Municipio = new Municipio();
            this.Publicacoes = new List<Publicacao>();
        }
    }
}
