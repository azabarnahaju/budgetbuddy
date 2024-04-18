namespace BudgetBuddy.Services.FinancialNewsService;

public class FinancialNewsProvider : IFinancialNewsProvider
{
    private readonly ILogger<FinancialNewsProvider> _logger;
    private readonly IConfiguration _configuration;
    
    public FinancialNewsProvider(ILogger<FinancialNewsProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<string> GetFinancialNews()
    {
        var apiKey = _configuration["FINANCIAL_NEWS_API_KEY"];
        var url = $"https://financialmodelingprep.com/api/v3/fmp/articles?page=0&size=5&apikey={apiKey}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling Financial News API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}