// using System.Net.Http.Headers;
// using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
// using Newtonsoft.Json;
//
// namespace BudgetBuddy.IntegrationTests.IntegrationTests;
//
// public class DeleteEndpointTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
// {
//     private readonly BudgetBuddyWebApplicationFactory<Program> _factory;
//     private readonly HttpClient _client;
//     
//     public DeleteEndpointTests(BudgetBuddyWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//         _client = _factory.CreateClient();
//     }
//     
//     [Theory]
//     [InlineData("/account/1")]
//     public async Task Delete_Account_Return_Success(string url)
//     {
//         var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//         
//         var response = await _client.DeleteAsync(url);
//         
//         response.EnsureSuccessStatusCode();
//     }
//     
//     
//     [Theory]
//     [InlineData("/achievement/delete/1")]
//     public async Task Delete_Achievement_Return_Success(string url)
//     {
//         var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//
//         var response = await _client.DeleteAsync(url);
//         response.EnsureSuccessStatusCode();
//     }
//     
//     
//     [Theory]
//     [InlineData("/goal/1")]
//     public async Task Delete_Goal_Returns_Success(string url)
//     {
//         var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//
//         var response = await _client.DeleteAsync(url);
//         
//         response.EnsureSuccessStatusCode();
//     }
//     
//     [Theory]
//     [InlineData("/report/1")]
//     public async Task Delete_Report_Returns_Success(string url)
//     {
//         var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//
//         var response = await _client.DeleteAsync(url);
//         
//         response.EnsureSuccessStatusCode();
//     }
//     
//     
//     [Theory]
//     [InlineData("/transaction/delete/1")]
//     public async Task Delete_Transaction_Returns_Success(string url)
//     {
//         var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//     
//         var response = await _client.DeleteAsync(url);
//         
//         response.EnsureSuccessStatusCode();
//     }
//     
//     async Task<dynamic> ConvertResponseData<T>(HttpResponseMessage response)
//     {
//         var responseContent = await response.Content.ReadAsStringAsync();
//         var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
//         var result = responseData.data;
//         T converted = result.ToObject<T>();
//         return converted;
//     }
// }
//
