using System.Net.Http.Json;
using System.Net.Mime;
using JoBoard.AuthService.Models;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.IntegrationTests.API;

public class BaseApiTest
{
    public async Task AssertSuccessEmptyResponseAsync(HttpResponseMessage response, int expectedStatusCode = StatusCodes.Status200OK)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        Assert.Equal(string.Empty, responseBody);
        Assert.Null(responseContentType);
    }
    
    public async Task AssertConflictResponseAsync(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<ConflictResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Assert.NotNull(responseBody?.Message);
        Assert.NotEmpty(responseBody!.Message);
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }
    
    public async Task AssertValidationResponseAsync(HttpResponseMessage response, int expectedErrors)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<ValidationResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, (int)response.StatusCode);
        Assert.NotNull(responseBody?.Errors);
        Assert.Equal(expectedErrors, responseBody?.Errors.Count());
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }
}