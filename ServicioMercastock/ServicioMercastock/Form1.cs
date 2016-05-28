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
            ExternoUrlUsuario.Text = Config.Externa.Usuario.UrlExportar;
            LocalUrlUsuario.Text = Config.Local.Usuario.UrlImportar;
            LocalUrlArticulo.Text = Config.Local.Articulo.UrlExportar;
            LocalUrlCategoria.Text = Config.Local.Categoria.UrlExportar;
            LocalUrlInventario.Text = Config.Local.Inventario.UrlExportar;
            LocalUrlDetalleVenta.Text = Config.Local.DetalleVenta.UrlExportar;
            LocalUrlParametro.Text = Config.Local.Parametro.UrlImportar;
            LocalUrlVenta.Text = Config.Local.Venta.UrlExportar;
            ExternoUrlArticulo.Text = Config.Externa.Articulo.UrlImportar;
            ExternoUrlCategoria.Text = Config.Externa.Categoria.UrlImportar;
            ExternoUrlDetalleVenta.Text = Config.Externa.DetalleVenta.UrlImportar;
            ExternoUrlInventario.Text = Config.Externa.Inventario.UrlExportar;
            ExternoUrlParametro.Text = Config.Externa.Parametro.UrlExportar;
            ExternoUrlUsuario.Text = Config.Externa.Usuario.UrlExportar;
            ExternoUrlVenta.Text = Config.Externa.Venta.UrlImportar;
            if (Sucursal.Autenticar())
            {
                Console.WriteLine(@"Autenticado");
             /*   backgroundWorker1.RunWorkerAsync();
                bwUsuario.RunWorkerAsync();
                bwArticulo.RunWorkerAsync();
                bwParametro.RunWorkerAsync();
                bwDepartamento.RunWorkerAsync();
                bwCategoria.RunWorkerAsync();
                bwVenta.RunWorkerAsync();
                bwDetalleVenta.RunWorkerAsync();*/
                bwNotificacionGcm.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(@"Error de Autenticación, revisar credenciales");
            }

        }

        public static bool EnviarNotificacion = false;
        private void MetodoGenerico(BackgroundWorker bw,Label status, Action<Action<string>> exportar, Action<string, Action<string>> importar, int tiempo)
        {
            try
            {
                BeginInvoke((MethodInvoker)(() => status.Text = @"1.- Exportando Información al Servidor"));
                exportar(x =>
                {
                    BeginInvoke((MethodInvoker)(() => status.Text = @"2.- Enviando información"));
                    importar(Opcion.LimpiarJson(x), y =>
                    {
                        BeginInvoke((MethodInvoker)(() => status.Text = @"3.- Envío terminado"));
                        if (EnviarNotificacion)
                        {
                            bwNotificacionGcm.RunWorkerAsync();
                        }
                        bw.CancelAsync();
                        Thread.Sleep(1000 * 60 * tiempo);
                        MetodoGenerico(bw,status, exportar, importar, tiempo);
                    });
                });
            }
            catch (Exception e)
            {
                Opcion.Log("GENERIC_LOG.txt", e.Message);
            }
        }

        private void bwUsuario_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MetodoGenerico(bwUsuario,statusUsuario, Usuario.Externa.Exportar, Usuario.Local.Importar, Config.General.Tiempo.Usuario);
            }
            catch (Exception errorException)
            {
                Opcion.Log("log_usuario.txt", errorException.Message);
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
                _memoriaRam = (Process.GetCurrentProcess().WorkingSet64 / 1024).ToString();
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(1000*Config.General.Tiempo.Pantalla);
            }
        }



        private void bWParametro_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MetodoGenerico(bwParametro,statusParametro, Parametro.Externa.Exportar, Parametro.Local.Importar, Config.General.Tiempo.Parametro);          
            }
            catch (ArgumentOutOfRangeException errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        private void bwArticulo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MetodoGenerico(bwArticulo,statusArticulo, Articulo.Local.Exportar, Articulo.Externa.Importar, Config.General.Tiempo.Articulo);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        private void bwCategoria_DoWork(object sender, DoWorkEventArgs e)
        { 
            try
            {
                MetodoGenerico(bwCategoria,statusCategoria, Categoria.Local.Exportar, Categoria.Externa.Importar, Config.General.Tiempo.Categoria);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        private void bwDepartamento_DoWork(object sender, DoWorkEventArgs e)
        {            
            try
            {
                MetodoGenerico(bwDepartamento,statusCategoria, Departamento.Local.Exportar, Departamento.Externa.Importar, Config.General.Tiempo.Departamento);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
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
            try
            {
                MetodoGenerico(bwVenta,statusVenta, Venta.Local.Exportar, Venta.Externa.Importar, Config.General.Tiempo.Venta);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        private void bwDetalleVenta_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MetodoGenerico(bwDetalleVenta,statusDetalleVenta, DetalleVenta.Local.Exportar, DetalleVenta.Externa.Importar, Config.General.Tiempo.DetalleVenta);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        public void bwNotificacionGcm_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                MetodoGenerico(bwNotificacionGcm,statusDetalleVenta, GcmPushNotification.ObtenerListaGcm, GcmPushNotification.EnviarNotificacion, Config.General.Tiempo.DetalleVenta);
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
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
               Console.WriteLine(e.Message);
               return "";
           }
           catch (ArgumentOutOfRangeException e)
           {
               Console.WriteLine(e.Message);
               return "";
           }
           catch (FormatException e)
           {
               Console.WriteLine(e.Message);
               return "";
           }
       }
       */

