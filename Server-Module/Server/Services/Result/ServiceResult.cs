using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Result
{
    public class ServiceResult : IServiceResult
    {
        public HttpStatusCode StatusCode { get; }
        public object Result { get; }

        public ServiceResult(HttpStatusCode code, object result)
        {
            StatusCode = code;
            Result = result;
        }
        public ServiceResult(HttpStatusCode code)
        {
            StatusCode = code;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.Run(async () =>
                {
                    context.HttpContext.Response.StatusCode = (int)StatusCode;
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    string json = JsonConvert.SerializeObject(Result, serializerSettings);

                    byte[] data = Encoding.UTF8.GetBytes(json);
                    await context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
                    await context.HttpContext.Response.Body.FlushAsync();
                });
        }
    }
}
