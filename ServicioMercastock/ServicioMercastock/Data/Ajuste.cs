/*
                   +----------------+
                   |                |
                   |   MERCASTOCK   |
                   |                |
                   +----^-----------+
                        |   |
  Externo.Importar();+-^+   +^-------+Externo.Exportar();
                        |   |
                   +--------v-------+     +----------------+  +---------------+
                   |                |     |  WEB SERVICES  |  |  PROCESO DE   |
                   |    INTERFAZ    +--^--+    LOCALES     +--+ ACTUALIZACION |
                   |                |  |  |                |  |               |
                   +----------------+  |  +---------^------+  +---------------+
                          ^            |        |   |                 |
                          |            +        |   |   Local.        |
                          |   Local.Importar(); |   +-> Transaccion()<-+
                          |                     |
                          |                     |
                          +---------------------+

*/

using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Ajuste
    {
        public class Local
        { 
            /*2do paso*/
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Ajuste.UrlImportar,Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Console.WriteLine(response.Content);
                            Externa.Actualizar(json,Console.WriteLine,1);
                            callback(response.Content); 
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Interno.Inventario1, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Inventario1, "EXCEPCION: " + e.Message);
                }
            }
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Ajuste.UrlExportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
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
                                Opcion.Log(Config.Log.Interno.Inventario1, response.Content + "::" + response.Content);
                                callback("CONTINUAR");
                                break;  
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Ajuste, "EXCEPCION: eXportar" + e.Message);
                }
            }

            public static void Actualizar(string json,Action<string> callback,int tipo)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi,Config.Local.Ajuste.UrlActualizar+"/"+tipo,Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    var response = rest.Cliente.Execute(rest.Peticion);               
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK: 
                            Console.WriteLine(response.Content);
                            callback(response.Content);
                            break;              
                        default:
                            Opcion.Log(Config.Log.Interno.Inventario1, response.StatusCode + "::" + response.Content);
                            callback("CONTINUAR");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Ajuste, "EXCEPCION:Actualizar " + e.Message);
                }
            }
            /*3er paso-> aqui se inicializa el ajuste*/
            public static void Transaccion(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Local.Ajuste.UrlLista, Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            IniciaTransaccion(response.Content, callback);
                        }
                        else
                        {
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Ajuste,"EXCEPCION Transaccion:"+e.Message);
                }
            }

            private static void IniciaTransaccion(string json,Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Local.Ajuste.UrlTransaccion, Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(json);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Ajuste, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Ajuste, "EXCEPCION: IniciarTransaccion " + e.Message);
                }
            }
        }

        public class Externa
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Ajuste.UrlImportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {//que pedo?
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Local.Actualizar(json, callback, 1);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Ajuste, response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Ajuste, "EXCEPCION: Importar:" + e.Message);
                }
            }

            /*primer paso*/
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Ajuste.UrlExportar,
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
                                Opcion.Log(Config.Log.Externo.Ajuste, response.Content);
                                callback("CONTINUAR");
                                break;
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Inventario2, "EXCEPCION-Exportar: " + e.Message);
                }
            }

            /*3er paso?  cambia de estados*/
            public static void Actualizar(string json,Action<string> callback,int tipo)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Ajuste.UrlActualizar+"/"+tipo,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Opcion.LimpiarJson(json), ParameterType.RequestBody);
                    rest.Peticion.AddJsonBody(new { idSucursal = Config.Externa.Sucursal.IdSucursal });
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log(Config.Log.Externo.Ajuste, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Ajuste, "EXCEPCION: " + e.Message);
                }
            }
          
        }
    }
}
