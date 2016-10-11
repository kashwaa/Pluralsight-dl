using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PluralSight_dl.Core
{
    /// <summary>
    /// Base class for a web request
    /// </summary>
    public abstract class Request
    {
        public Request(CookieContainer container = null)
        {
            this.container = container ?? new CookieContainer();
        }
        public abstract string SendRequest(string url, string parameters = null, string contentType = null);
        public abstract Task<string> SendRequestAsync(string url, string parameters = null, string contentType = null);
        public CookieContainer container { get; set; }
    }
}
