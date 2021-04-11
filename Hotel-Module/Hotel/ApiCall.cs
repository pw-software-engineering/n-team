using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    public class ApiCall
    {
        private string ip;
        public string Token { get; set; }

        public ApiCall(string ip, string token="")
        {
            Token = token;
            this.ip = ip;
        }

        //enpint - define in raml in specyfication
        //type - GET POST DELETE PUT etc.
        //content - for example application/json
        //dataString - info witch we want to send to the server 
        public string Api(string endpoint,string type, string content,string dataString)
        {
            string responseString;
            var data = Encoding.UTF8.GetBytes(dataString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ip + endpoint);
            request.Method = type;
            request.ContentType = content;
            //adding info tu body
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //auth
            request.Headers.Add(Token);

            //geting info from response
            using (var responsel = request.GetResponse())
            {
                using(var reader = new StreamReader(responsel.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }
    }
}
