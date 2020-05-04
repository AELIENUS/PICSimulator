using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicator.Services
{
    public static class Constants
    {
        #region program memory
        public static readonly short PROGRAM_MEMORY_SIZE = 1024;
        public static readonly short PERIPHERAL_INTERRUPT_VECTOR_ADDRESS = 0x04;
        #endregion
        #region Bank 1 data memory SFR
        public static readonly short INDF_B1 = 0x00;
        public static readonly short TMR0 = 0x01;
        public static readonly short PCL_B1 = 0x02;
        public static readonly short STATUS_B1 = 0x03;
        public static readonly short FSR_B1 = 0x04;
        public static readonly short PORTA = 0x05;
        public static readonly short PORTB = 0x06;
        public static readonly short EEDATA = 0x08;
        public static readonly short EEADR = 0x09;
        public static readonly short PCLATH_B1 = 0x0A;
        public static readonly short INTCON_B1 = 0x0B;
        #endregion
        #region Bank 2 data memory SFR
        public static readonly short INDF_B2 = 0x80;
        public static readonly short OPTION_REG = 0x81;
        public static readonly short PCL_B2 = 0x82;
        public static readonly short STATUS_B3 = 0x83;
        public static readonly short FSR_B2 = 0x84;
        public static readonly short TRISA = 0x85;
        public static readonly short TRISB = 0x86;
        public static readonly short EECON1 = 0x88;
        public static readonly short EECON2 = 0x89; // nicht zugreifen, sollte man nie brauchen
        public static readonly short PCLATH_B2 = 0x8A;
        public static readonly short INTCON_B2 = 0x8B;
        #endregion 
        #region STATUS BITS
        public static readonly short IRP = 0x07;
        public static readonly short RP1 = 0x06;
        public static readonly short RP0 = 0x05;
        public static readonly short INV_TO = 0x04;
        public static readonly short INV_PD = 0x05;
        public static readonly short Z = 0x02;
        public static readonly short DC = 0x01;
        public static readonly short C = 0x00;
        #endregion
        #region PORT A Bits
        public static readonly short RA4_T0CKI = 0x04;
        public static readonly short RA3 = 0x03;
        public static readonly short RA2 = 0x02;
        public static readonly short RA1 = 0x01;
        public static readonly short RA0 = 0x00;
        #endregion
        #region PORT B Bits
        public static readonly short RB7 = 0x07;
        public static readonly short RB6 = 0x06;
        public static readonly short RB5 = 0x05;
        public static readonly short RB4 = 0x04;
        public static readonly short RB3 = 0x03;
        public static readonly short RB2 = 0x02;
        public static readonly short RB1 = 0x01;
        public static readonly short RB0 = 0x00;
        #endregion
        #region OPTION_REG BITS
        public static readonly short INV_RBPU = 0x07;
        public static readonly short INTEDG = 0x06;
        public static readonly short T0CS = 0x05;
        public static readonly short T0SE = 0x04;
        public static readonly short PSA = 0x03;
        public static readonly short PS2 = 0x02;
        public static readonly short PS1 = 0x01;
        public static readonly short PS0 = 0x00;
        #endregion
        #region EECON1 Bits
        public static readonly short EEIF = 0x04;
        public static readonly short WRERR = 0x03;
        public static readonly short WREN = 0x02;
        public static readonly short WR = 0x01;
        public static readonly short RD = 0x00;
        #endregion
        #region INTCON Bits
        public static readonly short GIE = 0x07;
        public static readonly short EEIE = 0x06;
        public static readonly short T0IE = 0x05;
        public static readonly short INTE = 0x04;
        public static readonly short RBIE = 0x03;
        public static readonly short T0IF = 0x02;
        public static readonly short INTF = 0x01;
        public static readonly short RBIF = 0x00;
        #endregion
    }
}
