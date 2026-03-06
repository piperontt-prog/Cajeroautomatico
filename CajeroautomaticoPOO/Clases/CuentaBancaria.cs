using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CajeroautomaticoPOO
{
    // Clase base de la que heredan ahorros y corriente
    public abstract class CuentaBancaria
    {
        public string NumeroCuenta { get; private set; }  // Número único
        public string Titular { get; private set; }       // Nombre del dueño
        public string Pin { get; private set; }           // PIN de seguridad
        public decimal Saldo { get; protected set; }      // Dinero en la cuenta

        public CuentaBancaria(string numero, string titular, string pin, decimal saldo)
        {
            NumeroCuenta = numero;
            Titular = titular;
            Pin = pin;
            Saldo = saldo;
        }

        public abstract bool Retirar(decimal monto); // Método que cada tipo implementa diferente

        public void Depositar(decimal monto)
        {
            Saldo += monto; // Depósito básico (ahorros lo sobreescribe)
        }

        public bool ValidarPin(string pin)
        {
            return Pin == pin; // Verifica si el PIN coincide
        }

        public decimal ConsultarSaldo()
        {
            return Saldo;
        }
    }
}
