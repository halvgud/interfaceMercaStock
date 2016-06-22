using System;
using System.Net;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    class Usuario
    {
#region "kek"
        public string IdUsuario { get; set; }
        public string Vsuario { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Sexo { get; set; }
        public string Contacto { get; set; }
        public string IdSucursal { get; set; }
        public string IdNivelAutorizacion { get; set; }
        public string IdEstado { get; set; }
        public DateTime FechaEstado { get; set; }
        public DateTime FechaSesion { get; set; }
        public string ClaveGcm { get; set; }
        #endregion
     public class Local
        {
           public static void Importar(string json,Action<string> callback)
           {
               try
               {
                   var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Usuario.UrlImportar, Method.POST);
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
                            Opcion.Log(Config.Log.Interno.Usuario, response.Content);
                            callback("CONTINUAR");
                        }
                    });
                }
                catch (Exception e)
               {
                    Opcion.Log(Config.Log.Interno.Usuario, e.Message);
                    callback("CONTINUAR");
                }
           }
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Usuario.UrlExportar, Method.POST);
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
                                Opcion.Log(Config.Log.Externo.Usuario, response.Content);
                                callback("CONTINUAR");
                                break;
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Usuario, e.Message);
                }
            }
        }

     public class Externa
        {
            public static void Importar(string json, Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Usuario.UrlImportar, Method.POST);
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
                            Opcion.Log(Config.Log.Interno.Usuario, response.Content);
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Interno.Usuario, e.Message);
                }
            }
            public static void Exportar(Action<string> callback)
            {
                try
                {
                    var rest = new Rest(Config.Externa.Api.UrlApi, Config.Externa.Usuario.UrlExportar, Method.POST);
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
                                Opcion.Log(Config.Log.Externo.Usuario, response.Content);
                                callback("CONTINUAR");
                                break;
                        }
                    });
                }
                catch (Exception e)
                {
                    Opcion.Log(Config.Log.Externo.Usuario, e.Message);
                    callback("CONTINUAR");
                }
            }
        }
    }
}


#region "basura"
/*  public static void ObtenerUsuario(Action<string> callback)
  {
      Config.Local.Local.UrlImportar = "usuario/obtener";
      var Cliente = new RestClient(Config.Local.Local.UrlApi);
      var Peticion = new RestRequest(Config.Local.Local.UrlImportar, Method.POST);
      Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido, Constantes.Http.TipoDeContenido.Json);
      Peticion.AddJsonBody(new { hora_inicio = "2016 - 05 - 01", hora_fin = "2016 - 05 - 08" });
      Cliente.ExecuteAsync(Peticion, response =>
      {
          callback(response.Content);
      });
  }
  */
/*
   // Peticion.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
                // Peticion.AddUrlSegment("id", "123"); // replaces matching token in Peticion.Resource

                // easily add HTTP Headers
                //  Peticion.AddHeader("header", "value");

                // add files to upload (works with compatible verbs)
                // Peticion.AddFile(path);

                // execute the Peticion
                //Cliente.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
                //IRestResponse response1 = Cliente.Execute(Peticion);
                //var content = response1.Content; // raw content as string
                // Debug.WriteLine(content);
                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                // RestResponse<Person> response2 = Cliente.Execute<Person>(Peticion);
                //  var name = response2.Data.Name;

                // easy async support

                  // Cliente.ExecuteAsync(Peticion, response => {
               //     Debug.WriteLine(response.Content);
              //                                               return response.Content;
              //  });
               // return response1.Content;
                // async with deserialization
                ///   var asyncHandle = Cliente.ExecuteAsync<Person>(Peticion, response => {
                //       Console.WriteLine(response.Data.Name);
                //   });

                // abort the Peticion on demand
                //  asyncHandle.Abort();

             
*/
#endregion