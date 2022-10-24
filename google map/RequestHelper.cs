using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Abc_mart
{
    class RequestHelper
    {
        public CookieCollection sitecookie { get; set; } = new CookieCollection();
        public string responseurl { get; set; }
        public WebHeaderCollection wr { get; set; }
        public string Locationurl { get; set; }
        public string httpRequest(string url, string info, WebHeaderCollection wc, Encoding encoding, Encoding streamEncoding, bool isusecookie,
                    string host, string contentype, string method, string userAgent, bool autoRedirect, bool abcRaffle = false, int timeut = 5000)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            byte[] contentByte = streamEncoding.GetBytes(info);
            req.Timeout = timeut;
            req.Method = method;
            req.ContentLength = contentByte.Length;
            req.Accept = "application/json";
            req.Headers.Add(wc);
            req.UserAgent = userAgent;
            req.AllowAutoRedirect = autoRedirect;
            if (method != "GET")
            {
                req.ContentType = contentype;
                Stream webstream = req.GetRequestStream();
                webstream.Write(contentByte, 0, contentByte.Length);
                webstream.Close();
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    responseurl = response.ResponseUri.ToString();
                    wr = response.Headers;
                    StreamReader readhtmlStream = null;
                    string readtoend = null;
                    if (response.ContentEncoding == "gzip")
                    {
                        Stream tokenStream = response.GetResponseStream();
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        readhtmlStream = new StreamReader(new GZipStream(tokenStream, CompressionMode.Decompress), encoding);
                        readtoend = readhtmlStream.ReadToEnd();
                    }
                    else if (response.ContentEncoding == "br")
                    {
                        Stream tokenStream = response.GetResponseStream();
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        readhtmlStream = new StreamReader(new BrotliStream(tokenStream, CompressionMode.Decompress), encoding);
                        readtoend = readhtmlStream.ReadToEnd();
                    }
                    else
                    {
                        Stream tokenStream = response.GetResponseStream();
                        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                        readhtmlStream = new StreamReader(tokenStream, encoding);
                        readtoend = readhtmlStream.ReadToEnd();
                    }
                    return readtoend;
                }

            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("503"))
                {
                    Console.WriteLine("503 Service Unavailable");
                    return "";
                }
                else if (ex.Message.Contains("403"))
                {
                    Console.WriteLine("403 Forbidden");
                    return "";
                }
                else if (ex.Message.Contains("500"))
                {
                    Console.WriteLine("500 Server Error");
                    return "";
                }
                else if (ex.Message.Contains("504"))
                {
                    Console.WriteLine("504 Gateway timeout");
                    return "";
                }
                else if (ex.Message.Contains("502"))
                {
                    Console.WriteLine("502 Bad Gateway");
                    return "";

                }
                else if (ex.Message.Contains("407"))
                {
                    Console.WriteLine("407 Proxy Authentication Required");
                    return "";
                }
                else if (ex.Message.Contains("400"))
                {
                    Console.WriteLine("400 Bad Request");
                    return "";

                }
                else if (ex.Message.Contains("401"))
                {
                    Console.WriteLine("401 Unauthorized");
                    return "";
                }
                else if (ex.Message.Contains("429"))
                {
                    Console.WriteLine("429 Too Many Requests");
                    return "";
                }
                else
                {
                    Console.WriteLine(ex.Message.ToString());
                    return "";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return "";
            }
        }
    }
}
