using Abc_mart;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace google_map
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string address = "";
            int n = 0;
            RequestHelper reqhelper = new RequestHelper();
            WebHeaderCollection req = new WebHeaderCollection();
            Program pg = new Program();
            var locationtxt = File.ReadAllLines(Environment.CurrentDirectory + "\\task.txt");
            foreach (var i in locationtxt)
            {
                for (int j = 0; j < 20; j++)
                {
                    double[] postion = pg.postion(i);
                    string url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + postion[0] + ", " + postion[1] + "&key=yourownkey&language=ja";
                    string readtoend = reqhelper.httpRequest(url, "", req, Encoding.UTF8, Encoding.UTF8, isusecookie: false, "", "application/json", "POST", "Mobile/7381 CFNetwork/1335.0.3 Darwin/21.6.0", autoRedirect: false, abcRaffle: false, 9000);
                    if (readtoend.Contains("premise"))
                    {
                        Console.WriteLine("GEO Decode Address From Google Map API "+n+"");
                        n++;
                        JObject jo = JObject.Parse(readtoend);
                        address += jo["results"][0]["formatted_address"].ToString()+"\n";
                    }
                }
            }
            File.WriteAllText(Environment.CurrentDirectory + "\\address.txt", address);
        }
        public double[] postion(string value)
        {
            string[] positionstring = value.Split(",");
            double longitude_double = double.Parse(positionstring[0]);
            double latitude_double = double.Parse(positionstring[1]);
            Random ran = new Random();
            double movevalue = ran.NextDouble(0.00202516381, 0.00405032762);
            return new double[2] { longitude_double + movevalue, latitude_double + movevalue };
        }
    }
    public static class MehtondExtension
    {
        public static double NextDouble(this Random random, double miniDouble, double maxiDouble)
        {
            if (random != null)
            {
                return random.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
            }
            else
            {
                return 0.0d;
            }
        }
    }
}
