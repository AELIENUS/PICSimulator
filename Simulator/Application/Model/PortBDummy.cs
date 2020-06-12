using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Model
{
    public class PortBDummy : AbstractByte
    {
        private PortB _PortB;

        public override byte Value
        {
            get
            {
                return _PortB.Value;
            }
            set
            {
                _PortB.Value = value;
            }
        }

        public PortBDummy(PortB port)
        {
            _PortB = port;
        }
    }
}
