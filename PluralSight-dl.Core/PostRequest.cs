using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
    /// <summary>
    /// A facade that encapsulates WebRequest class
    /// </summary>
    public class PostRequest : Request
    {
        public PostRequest(CookieContainer container = null) : base(container)
        {
        }
        public override string SendRequest(string url, string parameters, string contentType)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.CookieContainer = container;
            request.ContentType = contentType;// "application/x-www-form-urlencoded";
            var buffer = Encoding.UTF8.GetBytes(parameters);
            request.ContentLength = buffer.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();
            }
            var res = request.GetResponse();
            using (var st = res.GetResponseStream())
            {
                StreamReader r = new StreamReader(st);
                string data = r.ReadToEnd();
                this.container.Add(((HttpWebResponse)res).Cookies);
                return data;
            }
        }
        public override Task<string> SendRequestAsync(string url, string parameters = null, string contentType = null)
        {
            return Task.Run(() => {
                return SendRequest(url, parameters, contentType);
            });
        }
    }
}
