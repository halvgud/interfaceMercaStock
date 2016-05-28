using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Parametro
    {
        public class Local
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Parametro.UrlImportar,
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
                            Opcion.Log("log_parametro_local.txt",response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log("log_parametro_local.txt", e.Message);
                    throw;
                }
            }
        }

        public class Externa
        {
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Parametro.UrlExportar,
                        Method.POST);
                    rest.Peticion.AddHeader(Constantes.Http.Autenticacion, Config.Externa.Sucursal.ClaveApi);
                    rest.Peticion.AddJsonBody(new {idSucursal = Config.Externa.Sucursal.IdSucursal});
                    rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                        Constantes.Http.TipoDeContenido.Json);

                    

                    rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            callback(response.Content);
                        }
                        else
                        {
                            Opcion.Log("log_parametro_externa.txt", response.Content);
                        }
                        
                    });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Opcion.Log("log_parametro_externa.txt", e.Message);
                }
            }
        }
    }
}