namespace ServicioMercastock.Prop
{
   public class Config
    {
       public class Log
       {
           public class Interno
           {
                public static string Articulo { get; set; }
                public static string Categoria { get; set; }
                public static string Departamento { get; set; }
                public static string DetalleVenta { get; set; }
                public static string Gcm { get; set; }
                public static string Inventario1 { get; set; }
                public static string Parametro { get; set; }
                public static string Sucursal { get; set; }
                public static string Usuario { get; set; }
                public static string Venta { get; set; }
                public static string VentaTipoPago { get; set; }
                public static string DashBoard { get; set; }
                public static string Ajuste { get; set; }
                public static string Proveedor { get; set; }
                public static string ProveedorArticulo { get; set; }
            }

           public class Externo
           {
                public static string Articulo { get; set; }
                public static string Categoria { get; set; }
                public static string Departamento { get; set; }
                public static string DetalleVenta { get; set; }
                public static string Inventario2 { get; set; }
                public static string Parametro { get; set; }
                public static string Sucursal { get; set; }
                public static string Usuario { get; set; }
                public static string Venta { get; set; }
                public static string VentaTipoPago { get; set; }
                public static string VentaCancelacion { get; set; }
                public static string Ajuste { get; set; }
                public static string Proveedor { get; set; }
                public static string ProveedorArticulo { get; set; }
            }
       }
       public class General
       {
           public class Tiempo
           {
                public static int Pantalla { get; set; }
                public static int Usuario { get; set; }
                public static int Usuario2 { get; set; }
                public static int Articulo { get; set; }
                public static int Categoria { get; set; }
                public static int Departamento { get; set; }
                public static int Parametro { get; set; }
                public static int Venta { get; set; }
                public static int DetalleVenta { get; set; }
                public static int Inventario1 { get; set; }
                public static int Inventario2 { get; set; }
                public static int VentaTipoPago { get; set; }
                public static int VentaCancelacion { get; set; }
                public static int Ajuste { get; set; }
                public static int Ajuste2 { get; set; }
                public static int Proveedor { get; set; }
                public static int ProveedorArticulo { get; set; }
            }

           public class Activacion
           {
                public static bool Usuario { get; set; }
                public static bool Usuario2 { get; set; }
                public static bool Articulo { get; set; }
                public static bool Categoria { get; set; }
                public static bool Departamento { get; set; }
                public static bool Parametro { get; set; }
                public static bool Venta { get; set; }
                public static bool DetalleVenta { get; set; }
                public static bool Inventario1 { get; set; }
                public static bool Inventario2 { get; set; }
                public static bool VentaTipoPago { get; set; }
                public static bool VentaCancelacion { get; set; }
                public static bool Ajuste { get; set; }
                public static bool Ajuste2 { get; set; }
                public static bool Proveedor { get; set; }
                public static bool ProveedorArticulo { get; set; }
            }

           public class Gcm
           {
               public static string ServerApiKey { get; set; }
               public static string SenderId { get; set; }
               public static string UrlRequest { get; set; }    
           }

           public class GcmParametro
           {
              
               public static string LlaveDeColapso { get; set; }
               public static int TiempoDeVida { get; set; }
                public static bool RetardoMientrasInactivo { get; set; }
               public static string Mensaje { get; set; }
           }
        }
        public class Local
        {
            public class Api
            {
                public static string UrlApi { get; set; }
            }

            public class Gcm
            {
                public static string UrlLista { get; set; }
            }
            public class Usuario
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
            }

            public class Parametro
            {
                public static string UrlImportar { get; set; }
            }
            public class Articulo
            {
                public static string UrlExportar { get; set; }
            }

            public class Categoria
            {
                public static string UrlExportar { get; set; }
            }

            public class Sucursal
            {
                public static string UrlUsuario { get; set; }
                public static string JsonCredencial { get; set; }
            }
            public class Departamento
            {
                public static string UrlExportar { get; set; }
            }

            public class DetalleVenta
            {
                public static string UrlExportar { get; set; }
            }

            public class Venta
            {
                public static string UrlExportar { get; set; }
                public static string UrlExportarCancelacion { get; set; }
            }

            public class Inventario
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
                public static string UrlActualizar { get; set; }
            }

            public class VentaTipoPago
            {
                public static string UrlExportar { get; set; }
            }

            public class Ajuste
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
                public static string UrlActualizar { get; set; }
            }

            public class Proveedor
            {
                public static string UrlExportar { get; set; }
            }

            public class ProveedorArticulo
            {
                public static string UrlExportar { get; set; }
            }
        }

       public class Externa
       {
           public class Api
           {
               public static string UrlApi { get; set; }
           }

           public class DashBoard
           {
               public static string UrlActualizar { get; set; }
           }
           public class Usuario
           {
                public static string UrlExportar { get; set; }
                public static string UrlImportar { get; set; }
            }
            public class Parametro
            {
                public static string UrlExportar { get; set; }
            }

           public class Articulo
           {
               public static string UrlImportar { get; set; }
           }

           public class Categoria
           {
               public static string UrlImportar { get; set; }
           }

           public class Departamento
           {
               public static string UrlImportar { get; set; }
           }

           public class Sucursal
           {
                public static string UrlAutenticar { get; set; }
                public static string UrlDb { get; set; }
                public static string IdSucursal { get; set; }
                public static string User { get; set; }
                public static string Password { get; set; }
                public static string ClaveApi { get; set; }
                public static string Nombre { get; set; }
                public static string Host { get; set; }
           } 
           public class DetalleVenta
           {
               public static string UrlImportar { get; set; }
                public static string UrlMaximaIdVenta { get; set; }
           }
            public class Venta
            {
                public static string UrlImportar { get; set; }
                public static string UrlMaximaIdVenta { get; set; }
                public static string UrlImportarCancelacion { get; set; }
            }
            public class Inventario
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
                public static string UrlActualizar { get; set; }
            }
            public class VentaTipoPago
            {
                public static string UrlImportar { get; set; }
                public static string UrlMaximaIdVenta { get; set; }
            }
            public class Ajuste
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
                public static string UrlActualizar { get; set; }
            }
            public class Proveedor
            {
                public static string UrlImportar { get; set; }
            }
            public class ProveedorArticulo
            {
                public static string UrlImportar { get; set; }
            }
        }
    }
}
