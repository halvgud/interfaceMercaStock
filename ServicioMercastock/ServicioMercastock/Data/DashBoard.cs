using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class DashBoard
    {
        public class Externa
        {
            public static void Actualizar(string json,int tiempo,int tiempoDeEspera)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.DashBoard.UrlActualizar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddJsonBody(new
                    { idSucursal=Config.Externa.Sucursal.IdSucursal,
                        accion="CONFIG_DASHBOARD",
                        valor=tiempo,
                        comentario = tiempoDeEspera,
                        parametro = json });
                    var response = rest.Cliente.Execute(rest.Peticion);
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                
                                break;
                            case HttpStatusCode.Accepted:
                                break;
                            default:
                                Opcion.Log(Config.Log.Interno.DashBoard, response.Content);
                                break;
                        }
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.DashBoard,  "EXCEPCION: "+ e.Message);
                }
            }
        }

     
    }
}
