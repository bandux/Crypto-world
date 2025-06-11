using Newtonsoft.Json.Linq;

namespace RatesService.Services;

public class CoinMarketCapClient
{
	private readonly HttpClient _httpClient;
	private readonly string _apiKey;
	private readonly string _apiUrl;

	public CoinMarketCapClient(HttpClient httpClient, IConfiguration config)
	{
		_httpClient = httpClient;
		_apiKey = config["CoinMarketCap:ApiKey"]!;
		_apiUrl = config["CoinMarketCap:ApiUrl"]!;
	}

	public async Task<Dictionary<string, decimal>> GetLatestRatesAsync()
	{
		var url = _apiUrl;

		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _apiKey);

		var response = await _httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();

		var json = JObject.Parse(await response.Content.ReadAsStringAsync());

		var rates = new Dictionary<string, decimal>();
		foreach (var item in json["data"]!)
		{
			var symbol = item["symbol"]!.ToString();
			var price = item["quote"]!["USD"]!["price"]!.Value<decimal>();
			rates[symbol] = price;
		}

		return rates;
	}
}
