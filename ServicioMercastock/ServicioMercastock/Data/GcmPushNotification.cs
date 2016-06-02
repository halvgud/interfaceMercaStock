using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using ServicioMercastock.Prop;

namespace ServicioMercastock.Data
{
    public class GcmPushNotification
    {
        public static void ObtenerListaGcm(string json,Action<string> callback)
        {
            try
            {
                var rest = new Rest(Config.Local.Api.UrlApi, Config.Local.Gcm.UrlLista,
                    Method.POST);
                rest.Peticion.AddHeader(Constantes.Http.ObtenerTipoDeContenido,
                    Constantes.Http.TipoDeContenido.Json);
                rest.Peticion.AddParameter(Constantes.Http.RequestHeaders.Json, Venta.Externa.ObtenerIdVenta(), ParameterType.RequestBody);
                rest.Cliente.ExecuteAsync(rest.Peticion, response =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        callback(response.Content);
                    }
                    else
                    {
                        Opcion.Log("log_GCM.txt", response.Content);
                    }

                });
            }
            catch (Exception e)
            {
                Opcion.Log("log_GCM.txt", e.Message);
            }
        }
        public static void EnviarNotificacion(string categorias,string deviceRegIds,Action<string> callback)
        {
            //TODO Considerar separar el json object
            var a = JArray.Parse(JObject.Parse(deviceRegIds).Property("data").Value.ToString());
            var b = JArray.Parse(JObject.Parse(categorias).Property("data").Value.ToString());
            var tRequest = WebRequest.Create(Config.General.Gcm.UrlRequest);
            tRequest.Method = Constantes.Http.MetodoHttp.Post;
            tRequest.ContentType =Constantes.Http.TipoDeContenido.Json;
            tRequest.Headers.Add(string.Format("Authorization: key={0}", Config.General.Gcm.ServerApiKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", Config.General.Gcm.SenderId));
            var postdata = new JObject();
            var data = new JObject();
            var arreglo = new JArray();
            var arreglo2 = new JArray();
            foreach (var t in a)
            {
                arreglo.Add(t);
            }
            foreach (var x in b)
            {
                arreglo2.Add(x["cat_id"]);
            }
            postdata.Add(Constantes.Gcm.Parametro.CollapseKey, Config.General.GcmParametro.LlaveDeColapso);
            postdata.Add(Constantes.Gcm.Parametro.TimeToLive, Config.General.GcmParametro.TiempoDeVida);
            postdata.Add(Constantes.Gcm.Parametro.DelayWhileIdle, Config.General.GcmParametro.RetardoMientrasInactivo);
            data.Add(Constantes.Gcm.Parametro.Message, Config.General.GcmParametro.Mensaje);
            data.Add(Constantes.Gcm.Parametro.Data, arreglo2);
            data.Add(Constantes.Gcm.Parametro.Time, DateTime.Now);
            postdata.Add(Constantes.Gcm.Parametro.Data, data);
            postdata.Add(Constantes.Gcm.Parametro.RegistrationIds,arreglo);
            Console.WriteLine(postdata);
            var bytes = Encoding.UTF8.GetBytes(postdata.ToString());
            tRequest.ContentLength = bytes.Length;

            var stream = tRequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            var wResponse = tRequest.GetResponse();

            stream = wResponse.GetResponseStream();

            if (stream == null) return;
            var reader = new StreamReader(stream);

            var response = reader.ReadToEnd();

            var httpResponse = (HttpWebResponse)wResponse;
            var status = httpResponse.StatusCode.ToString();

            reader.Close();
            stream.Close();
            wResponse.Close();
            Form1.EnviarNotificacion = true;
            callback(status == "" ? response : "");

        }
    }
}
