using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CajeroautomaticoPOO
{
    // Esta clase representa una operación realizada por el usuario
    public class Transaccion
    {
        public int Id { get; set; }                  // ID único de transacción
        public string NumeroCuenta { get; set; }     // A qué cuenta pertenece
        public string TipoTransaccion { get; set; }  // Depósito, retiro, interés...
        public decimal Monto { get; set; }           // Cuánto dinero fue movido
        public DateTime Fecha { get; set; }          // Cuándo se realizó
        public decimal SaldoAnterior { get; set; }   // Saldo antes del movimiento
        public decimal SaldoNuevo { get; set; }      // Saldo después del movimiento
    }
}
