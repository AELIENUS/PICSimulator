using Application.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class PortADummy : AbstractByte
    {
        private PortA _PortA;

        public override byte Value
        {
            get
            {
                return _PortA.Value;
            }
            set
            {
                _PortA.Value = value;
            }
        }

        public PortADummy(PortA port)
        {
            _PortA = port;
        }
    }
}
