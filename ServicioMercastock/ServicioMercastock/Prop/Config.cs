namespace ServicioMercastock.Prop
{
   public class Config
    {
       public class General
       {
           public class Tiempo
           {
                public static int Pantalla { get; set; }
                public static int Usuario { get; set; }
                public static int Articulo { get; set; }
                public static int Categoria { get; set; }
                public static int Departamento { get; set; }
                public static int Parametro { get; set; }
                public static int Venta { get; set; }
                public static int DetalleVenta { get; set; }
                public static int Inventario1 { get; set; }
                public static int Inventario2 { get; set; }
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
            }

            public class Inventario
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
            }
        }

       public class Externa
       {
           public class Api
           {
               public static string UrlApi { get; set; }
           }
           public class Usuario
           {
                    public static string UrlExportar { get; set; }
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
               public static string IdSucursal { get; set; }
                public static string User { get; set; }
                public static string Password { get; set; }
                public static string ClaveApi { get; set; }
           } 
           public class DetalleVenta
           {
               public static string UrlImportar { get; set; }
           }
            public class Venta
            {
                public static string UrlImportar { get; set; }
                public static string UrlMaximaIdVenta { get; set; }
            }
            public class Inventario
            {
                public static string UrlImportar { get; set; }
                public static string UrlExportar { get; set; }
            }
        }
    }
}
