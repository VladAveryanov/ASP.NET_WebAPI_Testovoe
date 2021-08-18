using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace testovoe.Services
{
    
    public class JsonDownloader
    {
        
        private readonly ILogger<JsonDownloader> _logger;
        const string CbrDailyCurrenciesJsonPath = "https://www.cbr-xml-daily.ru/daily_json.js";
        public JsonDownloader(ILogger<JsonDownloader> logger)
        {
            _logger = logger;
        }

        public string Download()
        {
            string file = null;
            var client = new HttpClient();
            Task<string> json = null;

            try
            {  
                json = client.GetStringAsync(CbrDailyCurrenciesJsonPath);
                file = json.Result;
            }
            catch (Exception e) when (e is HttpRequestException || e is AggregateException)
            {
                _logger.LogError(e.Message);
            }
            return file;
        }
    }
}
