using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using testovoe.Models;
using testovoe.Services;

namespace testovoe
{
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrencyContext _context;
        private readonly JsonDownloader jsonDownloader;

        public CurrenciesController(CurrencyContext context, ILogger<JsonDownloader> jsonDownloaderLogger)
        {
            _context = context;
            jsonDownloader = new JsonDownloader(jsonDownloaderLogger);
        }

        [HttpGet]
        [Route("currencies")]
        public IActionResult GetCurrencies([FromQuery] PageParameters pageParametrs) 
        {
            if (!_context.AllCurrencies.Any())
            {
                string file = jsonDownloader.Download();
                if (file != null)
                {
                    JObject jObject = JObject.Parse(file);
                    var valutes = jObject["Valute"];
                    Dictionary<string, Currency> dict = JsonConvert.DeserializeObject<Dictionary<string, Currency>>(valutes.ToString());
                    var currencies = dict.Values;
                    _context.AddRange(currencies);
                    _context.SaveChanges();
                }  
            }
            var list = _context.AllCurrencies.ToList();
            return Ok(PagedList<Currency>.ToPagedList(list, pageParametrs.PageNumber, pageParametrs.PageSize));
        }

        [HttpGet("{id}")]
        [Route("currency/{id}")]
        public IActionResult GetCurrency(string id)
        {
            foreach(var currency in _context.AllCurrencies)
            {
                if (currency.ID == id)
                    return Ok(currency);
            }
            return NotFound();
        }
    }
}
