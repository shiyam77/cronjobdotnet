using cronjob.Authentication;
using cronjob.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Xml.Linq;

namespace cronjob.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class CurrencydataController : Controller
    {
        //private readonly ISchedulerFactory _factory;
        //private readonly IScheduler _scheduler;
        public readonly Appdbcontext _db;

        //public CurrencydataController(Appdbcontext db, IScheduler scheduler, ISchedulerFactory factory)
        //{
        //    _factory = factory;
        //    _db = db;
        //    _scheduler = scheduler;
        //}

        public CurrencydataController(Appdbcontext db)
        {
            _db = db; 
        }


        string Baseurl = "https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_z06Lk6x4m9UIgIPvxeX0rXpZyZzad7D3PmxG7y8f&currencies=EUR%2CUSD%2CCAD%2CINR%2CNZD";

        [HttpGet]
        public async Task<ActionResult>  GetCurrency()
        {
            //await Console.Out.WriteLineAsync("Greetings from HelloJob!");
            var properties = new NameValueCollection();

            //IScheduler scheduler = await _factory.GetScheduler();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync(Baseurl);
                if (Res.IsSuccessStatusCode)
                {
                    var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                    var apidata= JsonConvert.DeserializeObject<CurrencyApiResponse>(ObjResponse);
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
                    return Ok(ObjResponse);
                }
               
                return Ok();
            }
        }
    }
}
