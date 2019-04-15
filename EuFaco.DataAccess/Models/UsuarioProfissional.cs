using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class UsuarioProfissional : Usuario
    {
        #region Campos
        private string _Resumo;
        private string _PathFotoPerfil;
        private Estado _Estado;
        private Municipio _Municipio;
        private List<Avaliacao> _Avaliacoes;
        private List<Publicacao> _Publicacoes;
        #endregion

        #region Atributos
        public string Resumo
        {
            get { return _Resumo; }
            set { _Resumo = value; }
        }

        public string PathFotoPerfil
        {
            get { return _PathFotoPerfil; }
            set { _PathFotoPerfil = value; }
        }

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

        public List<Avaliacao> Avaliacoes
        {
            get { return _Avaliacoes; }
            set { _Avaliacoes = value; }
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

        public UsuarioProfissional()
        {
            this.Perfil = Usuario.TipoPerfil.UsuarioProfissional;
            this.Avaliacoes = new List<Avaliacao>();

            this.Estado = new Estado();
            this.Estado.Id = 0;

            this.Municipio = new Municipio();
            this.Municipio.Id = 0;
        }

        public UsuarioProfissional(Usuario usuario)
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
            this.Perfil = Usuario.TipoPerfil.UsuarioProfissional;
            this.Imagens = usuario.Imagens;
            this.Facebook = usuario.Facebook;
            this.Avaliacoes = new List<Avaliacao>();
            this.Estado = new Estado();
            this.Estado.Id = 0;
            this.Municipio = new Municipio();
            this.Municipio.Id = 0;
            this.Publicacoes = new List<Publicacao>();
        }
    }
}
