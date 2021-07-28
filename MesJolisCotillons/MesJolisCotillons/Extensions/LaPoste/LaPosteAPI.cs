using MesJolisCotillons.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Extensions.LaPoste
{
    public class LaPosteAPI
    {
        public TarifEnvoiResponse TarifEnvoi(int poids, TypeEnvoi? type = null)
        {
            var request = new LaPosteRestRequest(LaPosteConfig.GetTarifEnvoiUrl);
            if (type != null)
            {
                request.AddParameter("type", type.ToString(), ParameterType.QueryString);
            }
            request.AddParameter("poids", poids, ParameterType.QueryString);


            var response = request.ExecuteRequest();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var jSonresult = JsonConvert.DeserializeObject<List<TarifEnvoiResponseItem>>(response.Content, jsonSerializerSettings);

            return new TarifEnvoiResponse
            {
                response = (RestResponse)response,
                Tarifs_Set = jSonresult
            };
        }
    }

    #region LaPosteRestRequest
    public class LaPosteRestRequest : RestRequest
    {
        private RestClient client;

        public LaPosteRestRequest(string Resource)
            : base(Resource)
        {
            client = new RestClient(LaPosteConfig.BaseUrl);
            this.AddHeader("X-Okapi-Key", LaPosteConfig.PermanentAccessToken);
            this.AddHeader("Content-Type", "application/json; charset=utf-8");
        }

        public LaPosteRestRequest(string Resource, Method method)
            : base(Resource, method)
        {
            client = new RestClient(LaPosteConfig.BaseUrl);
            //client.AddDefaultHeader("Content-Type", "application/xml");
            this.AddHeader("X-Okapi-Key", LaPosteConfig.PermanentAccessToken);
            this.AddHeader("Content-Type", "application/json; charset=utf-8");
        }

        public IRestResponse ExecuteRequest()
        {
            IRestResponse response = client.Execute(this);
            return response;
        }

        public T ExecuteRequest<T>() where T : LaPosteBaseResponse, new()
        {
            IRestResponse response = client.Execute(this);
            T result = null;

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            result = JsonConvert.DeserializeObject<T>(response.Content, jsonSerializerSettings);
            result.response = (RestResponse)response;

            return result;
        }

        public new IRestRequest AddJsonBody(object obj)
        {

            var json = JsonConvert.SerializeObject(obj);
            this.AddParameter("application/json", json, ParameterType.RequestBody);

            return this;
        }

        public IRestRequest AddFile(string Name, byte[] fileStream, string FileName)
        {
            base.AddFile(Name, fileStream, FileName);
            this.AlwaysMultipartFormData = true;

            return this;
        }
    }
    #endregion

    #region ResponseClass

    #region LaPosteBaseResponse
    public class LaPosteBaseResponse
    {
        [JsonExtensionData]
        public Dictionary<string, object> others { get; set; }
        [JsonIgnore]
        public RestResponse response { get; set; }

        public virtual bool isSuccess()
        {

            if (this.response != null && this.response.ResponseStatus == ResponseStatus.Completed)
                return true;
            else
                return false;
        }
    }
    #endregion


    public class TarifEnvoiResponse : LaPosteBaseResponse
    {
        public List<TarifEnvoiResponseItem> Tarifs_Set { get; set; }
    }

    public class TarifEnvoiResponseItem
    {
        public string channel { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        public string product { get; set; }
    }
    #endregion

    #region Enum Type
    public enum TypeEnvoi : int
    {
        colis = 1,
        chronopost = 2,
        lettre = 3
    }
    #endregion

    #region LaPosteConfig
    public static class LaPosteConfig
    {
        private static NameValueCollection laPosteWebConfig = (NameValueCollection)ConfigurationManager.GetSection("LaPosteApi");

        public static string PermanentAccessToken
        {
            get
            {
                return laPosteWebConfig["PermanentAccessToken"];
            }

        }
        public static string BaseUrl
        {
            get
            {
                return laPosteWebConfig["BaseUrl"];
            }

        }
        public static string GetTarifEnvoiUrl
        {
            get
            {
                return laPosteWebConfig["GetTarifEnvoiUrl"];
            }

        }
    }
    #endregion
}