﻿using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Sucursal
    {
        
        public static bool Autenticar()
        {
            try {
            
             var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Sucursal.UrlAutenticar,
     Method.POST);
            rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
            rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Opcion.LimpiarJson(ObtenerCredencialesLocal()), ParameterType.RequestBody);
                var respuesta = rest.Cliente.Execute(rest.Peticion);
                if (respuesta.StatusCode == HttpStatusCode.OK)
                {
                    Config.Externa.Sucursal.ClaveApi =
                        JObject.Parse(respuesta.Content).Property("claveAPI").Value.ToString();
                    Config.Externa.Sucursal.IdSucursal =
                        JObject.Parse(respuesta.Content).Property("idSucursal").Value.ToString();
                    Config.Externa.Sucursal.Nombre =
                        JObject.Parse(respuesta.Content).Property("nombre").Value.ToString();


                }
                else
                {
                    throw new Exception("Error de autenticación: "+respuesta.Content);
                }
                return respuesta.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Sucursal, "EXCEPCION: "+ e.Message);
                }
            return false;
        }

        private static string ObtenerCredencialesLocal()
        {
            var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Sucursal.UrlUsuario,
                        Method.POST);

            rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                Constantes.Http.TipoDeContenido.Json);
            rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Config.Local.Sucursal.JsonCredencial, ParameterType.RequestBody);
            var respuesta = rest.Cliente.Execute(rest.Peticion);
            return respuesta.Content;
        }
    }
}
