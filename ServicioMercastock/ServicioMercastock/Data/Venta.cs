
using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Venta
    {
        public class Local
        {
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Venta.UrlExportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Externa.ObtenerIdVenta(), ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content); 
                        }
                        else
                        {
                            Opcion.Log("log_venta_local.txt", response.Content);
                        }

                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_venta_local.txt", e.Message);
                }
            }
        }

        public class Externa
        {
            public static string ObtenerIdVenta()
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Venta.UrlMaximaIdVenta, Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddJsonBody(new { idSucursal = Config.Externa.Sucursal.IdSucursal });
                    var response = rest.Cliente.Execute(rest.Peticion);
                    return response.Content;
                }
                catch (Exception e)
                {
                    Opcion.Log("log_venta_externa.txt", e.Message);
                    throw;
                }
            }
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Venta.UrlImportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Opcion.LimpiarJson(json), ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log("log_venta_externa.txt", response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_venta_externa.txt", e.Message);
                }
            }
        }
    }
}
