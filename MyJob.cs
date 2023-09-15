using cronjob.Authentication;
using cronjob.Controllers;
using cronjob.Models;
using Newtonsoft.Json;
using Quartz;
using System.Net.Http.Headers;

namespace cronjob
{
    public class MyJob : IJob
    {
        string Baseurl = "https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_z06Lk6x4m9UIgIPvxeX0rXpZyZzad7D3PmxG7y8f&currencies=EUR%2CUSD%2CCAD%2CINR%2CNZD";

        public readonly Appdbcontext _db;
        public MyJob(Appdbcontext db)
        {
            _db = db;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //CurrencydataController permissionService = new CurrencydataController();
            // Your job logic here
            Console.WriteLine("Job executed at: " + DateTime.UtcNow);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync(Baseurl);
                if (Res.IsSuccessStatusCode)
                {
                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    var apidata = JsonConvert.DeserializeObject<CurrencyApiResponse>(ObjResponse);
                    foreach (var currency in apidata.Data)
                    {
                        var currencyExchangeRate = new currencyExchangeRate
                        {
                            CurrencyCode = currency.Key,
                            ExchangeRate = currency.Value
                        };
                        _db.ExchangeRates.Add(currencyExchangeRate);
                    }
                    _db.SaveChanges();

                }

                //return Ok();
            }
        }
    }
}
