using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CajeroautomaticoPOO
{
    public static class GestorTransacciones
    {
        // Archivo donde se guarda todo el historial de movimientos
        private static string archivo = "transacciones.txt";

        // Guarda una transacción en el archivo
        public static void Registrar(Transaccion t)
        {
            string linea =
                $"{t.Id};{t.NumeroCuenta};{t.TipoTransaccion};{t.Monto};" +
                $"{t.Fecha:yyyy-MM-dd HH:mm:ss};{t.SaldoAnterior};{t.SaldoNuevo}";

            File.AppendAllText(archivo, linea + Environment.NewLine);
        }

        // Muestra todas las transacciones de una cuenta específica
        public static void MostrarHistorial(string numeroCuenta)
        {
            Console.Clear();
            Console.WriteLine($"=== HISTORIAL DE {numeroCuenta} ===");
            Console.WriteLine("");

            if (!File.Exists(archivo))
            {
                Console.WriteLine("No hay transacciones registradas.");
                return;
            }

            string[] lineas = File.ReadAllLines(archivo);
            bool encontrado = false;

            // Busco solo las transacciones de esta cuenta
            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] datos = linea.Split(';');

                if (datos[1] == numeroCuenta)
                {
                    encontrado = true;

                    Console.WriteLine(
                        $"[{datos[4]}] {datos[2]} por {datos[3]} | Saldo: {datos[5]} -> {datos[6]}"
                    );
                }
            }

            if (!encontrado)
            {
                Console.WriteLine("No hay transacciones para esta cuenta.");
            }
        }
    }
}
