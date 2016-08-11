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
            //_flagFinalizarEjecucion = true;
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
              TiempoCancelacion.Text = Config.General.Tiempo.VentaCancelacion.ToString();
              TiempoAjuste.Text = Config.General.Tiempo.Ajuste.ToString();
              InicializarTareasAsyncronas();
              MostrarConfiguracionesLocales();
              MostrarConfiguracionesExternas();
            //ObtenerColorPorPorcentaje(10);
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
                if(!backgroundWorker1.IsBusy)
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
                InicializarEstado(estadoCancelacion,bwVentaCancelacion,Config.General.Activacion.VentaCancelacion);
                InicializarEstado(estadoAjuste,bwAjuste,Config.General.Activacion.Ajuste);
                InicializarEstado(estadoAjuste2,bwAjuste2,Config.General.Activacion.Ajuste2);
                InicializarEstado(estadoProveedor,bwProveedor,Config.General.Activacion.Proveedor);
                InicializarEstado(estadoProveedorArticulo,bwProveedorArticulo,Config.General.Activacion.ProveedorArticulo);
                if(!bwFormulario.IsBusy)
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
            if (_flagFinalizarEjecucion)
            {
                BeginInvoke((MethodInvoker)(() => status.Text = status.Text = @"Finalizando..."));
                var bw = (BackgroundWorker) status.Tag;
                bw.CancelAsync();
                return;
            }
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
                       // delegateTiempo += 1;
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
            statusUsuario.Tag = bwUsuario;
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
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
        }



        private void bWParametro_DoWork(object sender, DoWorkEventArgs e)
        {

            var tiempo = Config.General.Tiempo.Parametro;
            TiempoParametro.Tag = "PARAMETRO";
            statusParametro.Tag = bwParametro;
            MetodoGenerico(statusParametro,TiempoParametro, Parametro.Externa.Exportar, Parametro.Local.Importar, ref tiempo,tiempo);
        }

        private void bwArticulo_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Articulo;
            TiempoArticulo.Tag = "ARTICULO";
            statusArticulo.Tag = bwArticulo;
            MetodoGenerico(statusArticulo,TiempoArticulo, Articulo.Local.Exportar, Articulo.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwCategoria_DoWork(object sender, DoWorkEventArgs e)
        {
                var tiempo = Config.General.Tiempo.Categoria;
            TiempoCategoria.Tag = "CATEGORIA";
            statusCategoria.Tag = bwCategoria;
            MetodoGenerico(statusCategoria,TiempoCategoria, Categoria.Local.Exportar, Categoria.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwDepartamento_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Departamento;
            TiempoDepartamento.Tag = "DEPARTAMENTO";
            statusDepartamento.Tag = bwDepartamento;
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
            statusVenta.Tag = bwVenta;
            MetodoGenerico(statusVenta,TiempoVenta, Venta.Local.Exportar, Venta.Externa.Importar, ref tiempo,tiempo);

        }

        private void bwDetalleVenta_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.DetalleVenta;
            TiempoDetalleVenta.Tag = "DETALLEVENTA";
            statusDetalleVenta.Tag = bwDetalleVenta;
            MetodoGenerico(statusDetalleVenta,TiempoDetalleVenta, DetalleVenta.Local.Exportar, DetalleVenta.Externa.Importar, ref tiempo,tiempo);

        }

        public void bwInventario1_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Inventario1;
            TiempoInventario1.Tag = "INVENTARIO1";
            statusInventario.Tag = bwInventario1;
            MetodoGenerico(statusInventario,TiempoInventario1, Inventario.Externa.Exportar, Inventario.Local.Importar,ref tiempo,tiempo);

        }

        private void bwInventario2_DoWork(object sender, DoWorkEventArgs e)
        {

                var tiempo = Config.General.Tiempo.Inventario2;
            TiempoInventario2.Tag = "INVENTARIO2";
            statusInventario2.Tag = bwInventario2;
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
            TiempoUsuario.Tag = "USUARIO2";
            statusUsuario.Tag = bwUsuario2;
            MetodoGenerico(statusUsuario2, TiempoUsuario, Usuario.Local.Exportar, Usuario.Externa.Importar, ref tiempo, tiempo);
        }

        private void bwVentaTipoPago_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.VentaTipoPago;
            TiempoVentaTipoPago.Tag = "VENTA TIPO PAGO";
            statusVentaTipoPago.Tag = bwVentaTipoPago;
            MetodoGenerico(statusVentaTipoPago, TiempoVentaTipoPago, VentaTipoPago.Local.Exportar, VentaTipoPago.Externa.Importar, ref tiempo, tiempo);
        }

        private void bwVentaCancelacion_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.VentaCancelacion;
            TiempoCancelacion.Tag = "VENTA CANCELACION";
            statusCancelacion.Tag = bwVentaCancelacion;
            MetodoGenerico(statusCancelacion, TiempoCancelacion, Venta.Local.ExportarListaCancelacion, VentaTipoPago.Externa.ImportarCancelacion, ref tiempo, tiempo);
            
        }

        private void bwAjuste_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Ajuste;
            TiempoAjuste.Tag = "AJUSTE";
            statusAjuste.Tag = bwAjuste;
            MetodoGenerico(statusAjuste,TiempoAjuste,Ajuste.Externa.Exportar,Ajuste.Local.Importar,ref tiempo,tiempo);
        }

        private bool _banderaDetener;
        private void detenerMenuItem_Click(object sender, EventArgs e)
        {
            detenerMenuItem.Enabled = false;
            if (!_banderaDetener)
            {
                while (true)
                {
                    _flagFinalizarEjecucion = true;
                    if (!_ejecucionEnProgreso)
                    {
                        detenerMenuItem.Text = @"Iniciar";
                        detenerMenuItem.Enabled = true;
                        _banderaDetener = true;
                    }
                    else
                    {
                        detenerMenuItem.Enabled = false;
                        detenerMenuItem.Text = @"Deteniendo...";
                        Thread.Sleep(1000*60);
                        continue;
                    }
                    break;
                }
            }
            else
            {
                detenerMenuItem.Enabled = false;
                Form1_Load(sender, e);
                detenerMenuItem.Enabled = true;
                _banderaDetener = false;
            }
        }

        private void bwAjuste2_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Ajuste2;
            TiempoAjuste.Tag = "AJUSTE";
            statusAjuste.Tag = bwAjuste;
            MetodoGenerico(statusAjuste2, TiempoAjuste2, Ajuste.Local.Exportar, Ajuste.Externa.Importar, ref tiempo, tiempo);


        }

        private void bwProveedor_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.Proveedor;
            TiempoProveedor.Tag = "PROVEEDOR";
            statusProveedor.Tag = bwDepartamento;
            MetodoGenerico(statusDepartamento, TiempoProveedor, Departamento.Local.Exportar, Departamento.Externa.Importar, ref tiempo, tiempo);

        }

        private void bwProveedorArticulo_DoWork(object sender, DoWorkEventArgs e)
        {
            var tiempo = Config.General.Tiempo.ProveedorArticulo;
            TiempoProveedorArticulo.Tag = "PROVEEDOR ARTICULO";
            statusProveedorArticulo.Tag = bwDepartamento;
            MetodoGenerico(statusDepartamento, TiempoProveedorArticulo, Departamento.Local.Exportar, Departamento.Externa.Importar, ref tiempo, tiempo);

        }
    }

}


