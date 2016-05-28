﻿using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Inventario
    {
        public class Local
        {
            public static void Exportar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Local.Inventario.UrlExportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log("log_inventario_local.txt",response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_inventario_local.txt", e.Message);
                }
            }
            public static void Importar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Local.Inventario.UrlImportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                            Form1 f1 = new Form1();
                            f1.bwNotificacionGcm.RunWorkerAsync();
                        }
                        else
                        {
                            Opcion.Log("log_inventario_externa.txt", response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_inventario_externa.txt", e.Message);
                }
            }
        }

        public class Externa
        {
            public static void Exportar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Inventario.UrlExportar,
     Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, json, ParameterType.RequestBody);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log("log_inventario_local.txt", response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_inventario_local.txt", e.Message);
                }
            }
            public static void Importar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Inventario.UrlImportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log("log_inventario_externa.txt", response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_inventario_externa.txt", e.Message);
                }
            }
        }
    }
}
