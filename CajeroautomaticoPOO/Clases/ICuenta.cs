using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.cs
{
    public interface ICuenta
    {
        decimal ConsultarSaldo();
        bool Retirar(decimal monto);
        void Depositar(decimal monto);
    }
}
