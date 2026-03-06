using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CajeroautomaticoPOO
{
    public class CuentaAhorros : CuentaBancaria
    {
        private int transaccionesRealizadas = 0;     // Contador de movimientos del mes
        private const int transaccionesGratis = 5;   // Movimientos gratis permitidos
        private const decimal costoTransaccion = 100;// Cobro si se pasa del límite
        private decimal tasaInteres;                 // % de interés que genera

        public CuentaAhorros(string numero, string titular, string pin, decimal saldo, decimal interes)
            : base(numero, titular, pin, saldo)
        {
            tasaInteres = interes;
        }

        // Genera intereses y los registra como transacción
        public void GenerarIntereses()
        {
            decimal interesGenerado = Saldo * tasaInteres;
            Saldo += interesGenerado;

            GestorTransacciones.Registrar(new Transaccion
            {
                Id = new Random().Next(1, 999999),
                NumeroCuenta = NumeroCuenta,
                TipoTransaccion = "Interés generado",
                Monto = interesGenerado,
                Fecha = DateTime.Now,
                SaldoAnterior = Saldo - interesGenerado,
                SaldoNuevo = Saldo
            });
        }

        // Depósito con cobro si pasa de las transacciones gratis
        public void Depositar(decimal monto)
        {
            if (monto <= 0)
                throw new Exception("No se puede depositar un monto menor o igual a 0.");

            decimal saldoAnterior = Saldo;

            Saldo += monto;
            transaccionesRealizadas++;

            if (transaccionesRealizadas > transaccionesGratis)
                Saldo -= costoTransaccion;

            // Registro de la operación
            GestorTransacciones.Registrar(new Transaccion
            {
                Id = new Random().Next(1, 999999),
                NumeroCuenta = NumeroCuenta,
                TipoTransaccion = "Depósito",
                Monto = monto,
                Fecha = DateTime.Now,
                SaldoAnterior = saldoAnterior,
                SaldoNuevo = Saldo
            });
        }

        // Retiro con cobro si pasa del límite de transacciones gratis
        public override bool Retirar(decimal monto)
        {
            if (monto <= 0)
                throw new Exception("El monto debe ser mayor a 0.");

            decimal saldoAnterior = Saldo;

            if (Saldo < monto)
                return false;

            Saldo -= monto;
            transaccionesRealizadas++;

            if (transaccionesRealizadas > transaccionesGratis)
                Saldo -= costoTransaccion;

            // Registro del retiro
            GestorTransacciones.Registrar(new Transaccion
            {
                Id = new Random().Next(1, 999999),
                NumeroCuenta = NumeroCuenta,
                TipoTransaccion = "Retiro",
                Monto = monto,
                Fecha = DateTime.Now,
                SaldoAnterior = saldoAnterior,
                SaldoNuevo = Saldo
            });

            return true;
        }
    }
}
