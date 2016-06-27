using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using ServicioMercastock.Data;
using ServicioMercastock.Prop;

namespace ServicioMercastock
{
    public partial class Form1 : Form
    {
        private string _memoriaRam = "";
        public Form1()
        {
            InitializeComponent();
            Opcion.InicializarVariables();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            ApiUrlLocal.Text = Config.Local.Api.UrlApi;
            ApiUrlWeb.Text = Config.Externa.Api.UrlApi;
            TiempoPantalla.Text = Config.General.Tiempo.Pantalla.ToString();
            TiempoArticulo.Text = Config.General.Tiempo.Articulo.ToString();
            TiempoCategoria.Text = Config.General.Tiempo.Categoria.ToString();
            TiempoDepartamento.Text = Config.General.Tiempo.Departamento.ToString();
            TiempoUsuario.Text = Config.General.Tiempo.Usuario.ToString();
            TiempoParametro.Text = Config.General.Tiempo.Parametro.ToString();
            TiempoDetalleVenta.Text = Config.General.Tiempo.DetalleVenta.ToString();
            TiempoVenta.Text = Config.General.Tiempo.Venta.ToString();
            TiempoInventario1.Text = Config.General.Tiempo.Inventario1.ToString();
            TiempoInventario2.Text = Config.General.Tiempo.Inventario2.ToString();
            TiempoVentaTipoPago.Text = Config.General.Tiempo.VentaTipoPago.ToString();
            InicializarTareasAsyncronas();
            MostrarConfiguracionesLocales();
            MostrarConfiguracionesExternas();
        }

        private void MostrarConfiguracionesLocales()
        {
            LocalUrlUsuario.Text = Config.Local.Usuario.UrlImportar;
            LocalUrlArticulo.Text = Config.Local.Articulo.UrlExportar;
            LocalUrlCategoria.Text = Config.Local.Categoria.UrlExportar;
            LocalUrlInventario.Text = Config.Local.Inventario.UrlExportar;
            LocalUrlDetalleVenta.Text = Config.Local.DetalleVenta.UrlExportar;
            LocalUrlParametro.Text = Config.Local.Parametro.UrlImportar;
            LocalUrlVenta.Text = Config.Local.Venta.UrlExportar;
            LocalUrlInventario.Text = Config.Local.Inventario.UrlImportar;
            LocalUrlInventario2.Text = Config.Local.Inventario.UrlExportar;
            

        }
        private void MostrarConfiguracionesExternas()
        {
            ExternoUrlUsuario.Text = Config.Externa.Usuario.UrlExportar;
            ExternoUrlArticulo.Text = Config.Externa.Articulo.UrlImportar;
            ExternoUrlCategoria.Text = Config.Externa.Categoria.UrlImportar;
            ExternoUrlDetalleVenta.Text = Config.Externa.DetalleVenta.UrlImportar;
            ExternoUrlInventario.Text = Config.Externa.Inventario.UrlExportar;
            ExternoUrlParametro.Text = Config.Externa.Parametro.UrlExportar;
            ExternoUrlUsuario.Text = Config.Externa.Usuario.UrlExportar;
            ExternoUrlVenta.Text = Config.Externa.Venta.UrlImportar;
            
            ExternoUrlInventario.Text = Config.Externa.Inventario.UrlImportar;
            ExternoUrlInventario2.Text = Config.Externa.Inventario.UrlExportar;


        }

        private void InicializarTareasAsyncronas()
        {
            if (Sucursal.Autenticar())
            {
                Console.WriteLine(@"Autenticado");
                backgroundWorker1.RunWorkerAsync();
                InicializarEstado(estadoUsuario, bwUsuario, Config.General.Activacion.Usuario);
                InicializarEstado(estadoUsuario2,bwUsuario2,Config.General.Activacion.Usuario2);
                InicializarEstado(estadoArticulo, bwArticulo, Config.General.Activacion.Articulo);
                InicializarEstado(estadoParametro, bwParametro, Config.General.Activacion.Parametro);
                InicializarEstado(estadoDepartamento, bwDepartamento, Config.General.Activacion.Departamento);
                InicializarEstado(estadoCategoria, bwCategoria, Config.General.Activacion.Categoria);
                InicializarEstado(estadoVenta, bwVenta, Config.General.Activacion.Venta);
                InicializarEstado(estadoDetalleVenta, bwDetalleVenta, Config.General.Activacion.DetalleVenta);
                InicializarEstado(estadoInventario1, bwInventario1, Config.General.Activacion.Inventario1);
                InicializarEstado(estadoInventario2, bwInventario2, Config.General.Activacion.Inventario2);
                InicializarEstado(estadoVentaTipoPago,bwVentaTipoPago,Config.General.Activacion.VentaTipoPago);
                bwFormulario.RunWorkerAsync();

            }
            else
            {
                MessageBox.Show(@"Error de Autenticación, revisar credenciales");
            }
        }

        public void InicializarEstado(Label estado,BackgroundWorker bw,bool bandera)
        {
            if (!bandera) return;
            estado.Text = @"ACTIVO";
            bw.RunWorkerAsync();
        }

        public static bool EnviarNotificacion = false;
        private bool _ejecucionEnProgreso;
        private void MetodoGenerico(Label status,Label estadoTiempo, Action<Action<string>> exportar, Action<string, Action<string>> importar,ref int tiempo,int tiempo2)
        {
            var delegateTiempo = tiempo;
            if (_flagFinalizarEjecucion) return;
            try
            {
                _ejecucionEnProgreso = true;
                BeginInvoke((MethodInvoker)(() => status.Text = @"1.- Exportando Información al Servidor"));
                exportar(x =>
                {
                    if (x != "CONTINUAR")
                    {
                        delegateTiempo = tiempo2;
                        BeginInvoke((MethodInvoker) (() => status.Text = @"2.- Enviando información"));
                        importar(Opcion.LimpiarJson(x), y =>
                        {
                            BeginInvoke((MethodInvoker) (() => status.Text = @"3.- Envío terminado"));
                            TiempoDeEspera(estadoTiempo, ref delegateTiempo);
                            _ejecucionEnProgreso = false;
                            MetodoGenerico(status,estadoTiempo, exportar, importar, ref delegateTiempo,tiempo2);
                            
                        });
                    }
                    else
                    {
                        delegateTiempo += 1;
                        BeginInvoke((MethodInvoker)(() => status.Text = @"1.- Reiniciando Petición"));
                        _ejecucionEnProgreso = false;
                        TiempoDeEspera(estadoTiempo,ref delegateTiempo);
                        MetodoGenerico(status,estadoTiempo, exportar, importar,ref delegateTiempo,tiempo2);
                    }

                });
            }
            catch (Exception e)
            {
                Opcion.Log("GENERIC_LOG.txt",  "EXCEPCION: "+ e.Message);
            }
        }

        private static void TiempoDeEspera(ToolStripLabel estadoTiempo,ref int tiempoDeEspera)
        {

            for (var i = tiempoDeEspera; i >= 0; --i)
            {
               // DashBoard.Externa.Actualizar(estadoTiempo.Tag.ToString(),i,tiempoDeEspera);
                estadoTiempo.Text = i + @" Minutos";
                Thread.Sleep(1000 * 60);
            }
        }

        private static void TiempoDeEspera(Control estadoTiempo, ref int tiempoDeEspera)
        {
            for (var i = tiempoDeEspera; i >= 0; --i)
            {
                DashBoard.Externa.Actualizar(estadoTiempo.Tag.ToString(), i, tiempoDeEspera);
                estadoTiempo.Text = i + @" Minutos";
                Thread.Sleep(1000 * 60);
            }
        }

        private void bwUsuario_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Usuario;
            TiempoUsuario.Tag = "USUARIO";
            MetodoGenerico(statusUsuario, TiempoUsuario, Usuario.Externa.Exportar, Usuario.Local.Importar, ref tiempo, tiempo);
            
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                _memoriaRam = (Process.GetCurrentProcess().WorkingSet64 / 1024).ToString();
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(1000 * Config.General.Tiempo.Pantalla);
            }
        }



        private void bWParametro_DoWork(object sender, DoWorkEventArgs e)
        {

            var tiempo = Config.General.Tiempo.Parametro;
            TiempoParametro.Tag = "PARAMETRO";
            MetodoGenerico(statusParametro,TiempoParametro, Parametro.Externa.Exportar, Parametro.Local.Importar, ref tiempo,tiempo);
        }

        private void bwArticulo_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Articulo;
            TiempoArticulo.Tag = "ARTICULO";
            MetodoGenerico(statusArticulo,TiempoArticulo, Articulo.Local.Exportar, Articulo.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwCategoria_DoWork(object sender, DoWorkEventArgs e)
        {
                var tiempo = Config.General.Tiempo.Categoria;
            TiempoCategoria.Tag = "CATEGORIA";
            MetodoGenerico(statusCategoria,TiempoCategoria, Categoria.Local.Exportar, Categoria.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwDepartamento_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Departamento;
            TiempoDepartamento.Tag = "DEPARTAMENTO";
            MetodoGenerico(statusDepartamento,TiempoDepartamento, Departamento.Local.Exportar, Departamento.Externa.Importar,ref tiempo,tiempo);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  String resultado = e.Result as String;
            //  label1.Text = resultado;

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            memoriaRAM.Text = _memoriaRam + @" KB."; //e.ProgressPercentage.ToString();
        }

        private void bwVenta_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Venta;
            TiempoVenta.Tag = "VENTA";
            MetodoGenerico(statusVenta,TiempoVenta, Venta.Local.Exportar, Venta.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwDetalleVenta_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.DetalleVenta;
            TiempoDetalleVenta.Tag = "DETALLEVENTA";
            MetodoGenerico(statusDetalleVenta,TiempoDetalleVenta, DetalleVenta.Local.Exportar, DetalleVenta.Externa.Importar, ref tiempo,tiempo);

        }

        public void bwInventario1_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Inventario1;
            TiempoInventario1.Tag = "INVENTARIO1";
            MetodoGenerico(statusInventario,TiempoInventario1, Inventario.Externa.Exportar, Inventario.Local.Importar,ref tiempo,tiempo);

        }

        private void bwInventario2_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Inventario2;
            TiempoInventario1.Tag = "INVENTARIO2";
            MetodoGenerico(statusInventario2,TiempoInventario2, Inventario.Local.Exportar, Inventario.Externa.Importar,ref tiempo,tiempo);
   
        }

        private bool _flagFinalizarEjecucion;
        private void bwFormulario_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo =(1000*60*6);
            TiempoDeEspera(statusTiempoDeReinicio, ref tiempo);
            FinalizarEjecuciones();
        }

        private void FinalizarEjecuciones()
        {
            while (true)
            {
                _flagFinalizarEjecucion = true;
                if (!_ejecucionEnProgreso)
                {
                    Process.Start(Application.ExecutablePath); // to start new instance of application
                    Close();
                }
                else
                {
                    Thread.Sleep(1000*60);
                    continue;
                }
                break;
            }
        }

        private void bwUsuario2_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Usuario2;
            TiempoUsuario.Tag = "USUARIO1";
            MetodoGenerico(statusUsuario2, TiempoUsuario, Usuario.Local.Exportar, Usuario.Externa.Importar, ref tiempo, tiempo);
        }

        private void bwVentaTipoPago_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.VentaTipoPago;
            TiempoVentaTipoPago.Tag = "VENTA TIPO PAGO";
            MetodoGenerico(statusVentaTipoPago, TiempoVentaTipoPago, VentaTipoPago.Local.Exportar, VentaTipoPago.Externa.Importar, ref tiempo, tiempo);
        }
    }

}
/*
       private static string GetJsonDiff(string existing, string modified)
       {
           try
           {
               var webJson = JObject.Parse(modified).Property("data").Value;

               var data1 = JObject.Parse(existing.Substring(existing.IndexOf('{'),
                       existing.LastIndexOf('}') + 1 - existing.IndexOf('{'))).Property("data").Value;

               DataTable tablaLocal = (DataTable)JsonConvert.DeserializeObject(data1.ToString(), (typeof(DataTable)));
               DataTable tablaExterna =
                   (DataTable) JsonConvert.DeserializeObject(webJson.ToString(), typeof (DataTable));
               var matched =
              tablaLocal.AsEnumerable() //se numeraliza el datatable para heredar ciertas propiedades
                  .Join(tablaExterna.AsEnumerable(), table1 => table1.Field<string>("usuario"), //se hace join con la otra tabla para el campo id empleado
                      table2 => table2.Field<string>("usuario"),
                      (table1, table2) => new { table1, table2 }) //para cada tabla se crea una nueva tabla
                  .Where( // donde
                      @t => //para cada valor perteneciente a la expresion lambda se comparan campos estrategicos de la tabla 1 y tabla 2
                          @t.table1.Field<string>("nombre").Trim() == @t.table2.Field<string>("nombre") &&
                          @t.table1.Field<string>("apellido").Trim() == @t.table2.Field<string>("apellido") &&
                          //@t.table1.Field<string>("contrasena").Trim() == @t.table2.Field<string>("password") &&
                          @t.table1.Field<string>("idEstado").Trim() == @t.table2.Field<string>("idEstado") &&
                          @t.table1.Field<string>("idNivelAutorizacion").Trim() == @t.table2.Field<string>("idNivelAutorizacion"))
                  .Select(@t => @t.table1);//para al final seleccionar el resultado
                                           //otra expresion linq para sacar las diferencias que no estan en la tabla del dbf
               var missing = from table1 in tablaLocal.AsEnumerable()
                             where !matched.Contains(table1)
                             select table1;
               var dt1 = missing.CopyToDataTable();
               var diferencia = JsonConvert.SerializeObject(dt1);

               return diferencia;
               //  return JsonConvert.SerializeObject(auditLog);
           }
           catch (JsonException e)
           {
               Console.WriteLine( "EXCEPCION: "+ e.Message);
               return "";
           }
           catch (ArgumentOutOfRangeException e)
           {
               Console.WriteLine( "EXCEPCION: "+ e.Message);
               return "";
           }
           catch (FormatException e)
           {
               Console.WriteLine( "EXCEPCION: "+ e.Message);
               return "";
           }
       }
       */

