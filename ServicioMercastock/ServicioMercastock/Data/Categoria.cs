﻿using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Categoria
    {
        public class Local
        {
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Categoria.UrlExportar,
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
                                Opcion.Log(Config.Log.Interno.Categoria, response.Content);
                                callback("CONTINUAR");
                                break;
                        }

                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Categoria, e.Message);
                    callback("CONTINUAR");
                }
            }
        }

        public class Externa
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Categoria.UrlImportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
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
                            Opcion.Log(Config.Log.Externo.Categoria, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Categoria, e.Message);
                    callback("CONTINUAR");
                }
            }
        }
    }
}
