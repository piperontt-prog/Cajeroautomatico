using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CajeroautomaticoPOO
{
    public class CuentaCorriente : CuentaBancaria
    {
        public decimal Limite { get; private set; } // Límite permitido al sobregiro

        public CuentaCorriente(string numero, string titular, string pin, decimal saldo, decimal limite)
            : base(numero, titular, pin, saldo)
        {
            Limite = limite;
        }

        // Retiro especial: permite sobregirar hasta el límite
        public override bool Retirar(decimal monto)
        {
            if (monto > Saldo + Limite)
                return false;

            Saldo -= monto;
            return true;
        }
    }
}