using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class ItemPublicacao
    {
        private string _Nome;
        private string _Valor;

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

        public string Valor
        {
            get
            {
                return _Valor;
            }

            set
            {
                _Valor = value;
            }
        }

        public ItemPublicacao() { }

        public ItemPublicacao(string nome, string valor)
        {
            this.Nome = nome;
            this.Valor = valor;
        }
    }
}
