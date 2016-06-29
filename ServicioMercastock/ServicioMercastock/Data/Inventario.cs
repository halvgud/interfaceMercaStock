using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Inventario
    {
        public class Local
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Inventario.UrlImportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Console.WriteLine(response.Content);
                            GcmPushNotification.ObtenerListaGcm(response.Content,x =>
                            {
                                GcmPushNotification.EnviarNotificacion(json,Opcion.LimpiarJson(x), y =>
                                {
                                    Externa.Actualizar(Console.WriteLine);
                                });
                            });
                            callback(response.Content);

                        }
                        else
                        {
                            Opcion.Log(Config.Log.Interno.Inventario1,response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Inventario1,  "EXCEPCION: "+ e.Message);
                }
            }
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Inventario.UrlExportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);

                    //rest1.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                callback(response.Content);
                                break;
                            case HttpStatusCode.Accepted:
                                callback("CONTINUAR");
                                break;
                            default:
                                Opcion.Log(Config.Log.Interno.Inventario1, response.Content+"::"+response.Content);
                                callback("CONTINUAR");
                                break;
                        }
                    }); 
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Inventario1,  "EXCEPCION: "+ e.Message);
                }
            }
            public static void Actualizar(string json,Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Inventario.UrlActualizar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Inventario2, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Inventario2,  "EXCEPCION: "+ e.Message);
                }
            }
        }

        public class Externa
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Inventario.UrlImportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Local.Actualizar(json, x =>
                            {
                                Console.WriteLine(x);
                            });
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Inventario2, response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Inventario2,  "EXCEPCION: "+ e.Message);
                }
            }
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Inventario.UrlExportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddJsonBody(new { idSucursal = Config.Externa.Sucursal.IdSucursal });
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                callback(response.Content);
                                break;
                            case HttpStatusCode.Accepted:
                                callback("CONTINUAR");
                                break;
                            default:
                                Opcion.Log(Config.Log.Externo.Inventario2, response.Content);
                                callback("CONTINUAR");
                                break;
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Inventario2,  "EXCEPCION: "+ e.Message);
                }
            }

            public static void Actualizar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Inventario.UrlActualizar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddJsonBody(new { idSucursal = Config.Externa.Sucursal.IdSucursal });
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Inventario2, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Inventario2,  "EXCEPCION: "+ e.Message);
                }
            }
        }
    }
}
