using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CajeroautomaticoPOO
{
    internal class Program
    {
        // Ruta donde se guarda el archivo con los usuarios registrados
        static string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usuarios.txt");

        static void Main(string[] args)
        {
            // Cargo todas las cuentas que ya existan en el archivo
            Dictionary<string, CuentaBancaria> cuentas = CargarCuentas();

            // Menú principal del cajero
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== CAJERO AUTOMÁTICO =====");
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("2. Registrar usuario");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");

                string op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        IniciarSesion(cuentas); // Va al inicio de sesión
                        break;
                    case "2":
                        RegistrarUsuario(cuentas); // Crea una nueva cuenta
                        break;
                    case "3":
                        GuardarCuentas(cuentas); // Guarda todo antes de salir
                        Console.WriteLine("Datos guardados.");
                        Console.ReadKey();
                        return;

                    default:
                        Console.WriteLine("Opción inválida.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Carga todas las cuentas desde el archivo usuarios.txt
        static Dictionary<string, CuentaBancaria> CargarCuentas()
        {
            Dictionary<string, CuentaBancaria> cuentas = new Dictionary<string, CuentaBancaria>();

            if (!File.Exists(ruta))
                return cuentas;

            string[] lineas = File.ReadAllLines(ruta);

            // Recorro cada línea y convierto la info a objetos
            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] datos = linea.Split(';');
                if (datos.Length != 5)
                    continue;

                string numero = datos[0];
                string titular = datos[1];
                decimal saldo = decimal.Parse(datos[2]);
                string pin = datos[3];
                string tipo = datos[4];

                CuentaBancaria cuenta;

                // Según el tipo creo la clase correspondiente
                if (tipo == "Ahorros")
                    cuenta = new CuentaAhorros(numero, titular, pin, saldo, 0.01m);
                else
                    cuenta = new CuentaCorriente(numero, titular, pin, saldo, 500000);

                cuentas[numero] = cuenta;
            }

            return cuentas;
        }

        // Guarda todas las cuentas en usuarios.txt
        static void GuardarCuentas(Dictionary<string, CuentaBancaria> cuentas)
        {
            List<string> lineas = new List<string>();

            // Convertimos cada cuenta a una línea de texto
            foreach (CuentaBancaria c in cuentas.Values)
            {
                string tipo = (c is CuentaAhorros) ? "Ahorros" : "Corriente";
                lineas.Add($"{c.NumeroCuenta};{c.Titular};{c.Saldo};{c.Pin};{tipo}");
            }

            File.WriteAllLines(ruta, lineas);
        }

        // Registrar un nuevo usuario
        static void RegistrarUsuario(Dictionary<string, CuentaBancaria> cuentas)
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRAR USUARIO ===");

            Console.Write("Número de cuenta: ");
            string numero = Console.ReadLine();

            if (cuentas.ContainsKey(numero))
            {
                Console.WriteLine("La cuenta ya existe.");
                Console.ReadKey();
                return;
            }

            Console.Write("Titular: ");
            string titular = Console.ReadLine();

            Console.Write("PIN: ");
            string pin = Console.ReadLine();

            Console.Write("Tipo (1=Ahorros, 2=Corriente): ");
            string tipo = Console.ReadLine();

            CuentaBancaria nueva;

            // Según la opción crea la clase correcta
            if (tipo == "1")
                nueva = new CuentaAhorros(numero, titular, pin, 0, 0.01m);
            else
                nueva = new CuentaCorriente(numero, titular, pin, 0, 500000);

            cuentas[numero] = nueva;
            GuardarCuentas(cuentas);

            Console.WriteLine("Cuenta creada con éxito.");
            Console.ReadKey();
        }

        // Proceso para iniciar sesión
        static void IniciarSesion(Dictionary<string, CuentaBancaria> cuentas)
        {
            Console.Clear();
            Console.WriteLine("=== INICIO DE SESIÓN ===");

            Console.Write("Número de cuenta: ");
            string numero = Console.ReadLine();

            if (!cuentas.ContainsKey(numero))
            {
                Console.WriteLine("La cuenta no existe.");
                Console.ReadKey();
                return;
            }

            Console.Write("PIN: ");
            string pin = Console.ReadLine();

            CuentaBancaria cuenta = cuentas[numero];

            // Validamos el PIN
            if (!cuenta.ValidarPin(pin))
            {
                Console.WriteLine("PIN incorrecto.");
                Console.ReadKey();
                return;
            }

            MenuCuenta(cuenta);
        }

        // Menú interno luego de iniciar sesión
        static void MenuCuenta(CuentaBancaria cuenta)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== BIENVENIDO " + cuenta.Titular + " ===");
                Console.WriteLine("1. Consultar saldo");
                Console.WriteLine("2. Depositar");
                Console.WriteLine("3. Ver historial de transacciones");
                Console.WriteLine("4. Retirar");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione: ");

                string op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        // Solo muestra el saldo actual
                        Console.WriteLine("Saldo actual: " + cuenta.Saldo);
                        Console.ReadKey();
                        break;

                    case "2":
                        // Proceso de depósito
                        Console.Write("Monto a depositar: ");
                        decimal deposito = decimal.Parse(Console.ReadLine());

                        decimal saldoAnteriorD = cuenta.Saldo;

                        cuenta.Depositar(deposito); // Llama al método de depósito

                        // Guarda un registro en transacciones.txt
                        GestorTransacciones.Registrar(new Transaccion
                        {
                            Id = new Random().Next(1, 999999),
                            NumeroCuenta = cuenta.NumeroCuenta,
                            TipoTransaccion = "Depósito",
                            Monto = deposito,
                            Fecha = DateTime.Now,
                            SaldoAnterior = saldoAnteriorD,
                            SaldoNuevo = cuenta.Saldo
                        });

                        Console.WriteLine("Depósito exitoso.");
                        Console.ReadKey();
                        break;

                    case "4":
                        // Proceso de retiro
                        Console.Write("Monto a retirar: ");
                        decimal retiro = decimal.Parse(Console.ReadLine());

                        decimal saldoAnteriorR = cuenta.Saldo;

                        if (cuenta.Retirar(retiro))
                        {
                            GestorTransacciones.Registrar(new Transaccion
                            {
                                Id = new Random().Next(1, 999999),
                                NumeroCuenta = cuenta.NumeroCuenta,
                                TipoTransaccion = "Retiro",
                                Monto = retiro,
                                Fecha = DateTime.Now,
                                SaldoAnterior = saldoAnteriorR,
                                SaldoNuevo = cuenta.Saldo
                            });

                            Console.WriteLine("Retiro exitoso.");
                        }
                        else
                        {
                            Console.WriteLine("Fondos insuficientes.");
                        }

                        Console.ReadKey();
                        break;

                    case "3":
                        // Muestra historial desde el archivo
                        GestorTransacciones.MostrarHistorial(cuenta.NumeroCuenta);
                        Console.ReadKey();
                        break;

                    case "5":
                        // Salir del menú
                        return;

                    default:
                        Console.WriteLine("Opción inválida.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
