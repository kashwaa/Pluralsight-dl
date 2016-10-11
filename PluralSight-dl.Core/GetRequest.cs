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
    public class GetRequest : Request
    {
        public GetRequest(CookieContainer container = null) : base(container)
        {
        }
        public override string SendRequest(string url, string parameters = null, string contentType = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = container;
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
