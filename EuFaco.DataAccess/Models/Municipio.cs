using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuFaco.DataAccess.Models
{
    public class Municipio
    {
        private int _Id;

        
        private Estado _Estado;

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public Estado Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
    }
}
