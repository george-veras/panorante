using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EuFaco.ViewModel.Home
{
    public class PublicacaoImagem
    {
        private int _Id;
        private string _Legenda;
        private int _QuantidadeVisualizacoes;

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

        public string Legenda
        {
            get
            {
                return _Legenda;
            }

            set
            {
                _Legenda = value;
            }
        }

        public int QuantidadeVisualizacoes
        {
            get
            {
                return _QuantidadeVisualizacoes;
            }

            set
            {
                _QuantidadeVisualizacoes = value;
            }
        }
    }
}