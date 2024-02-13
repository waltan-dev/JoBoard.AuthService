using System.Net.Http.Json;
using System.Net.Mime;
using JoBoard.AuthService.Models;
using JoBoard.AuthService.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.Tests.Functional;

public static class Assert
{
    public static async Task SuccessAuthResponseAsync(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<AuthResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Xunit.Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        Xunit.Assert.NotNull(responseBody);
        
        Xunit.Assert.NotEqual(default, responseBody!.UserId);
        Xunit.Assert.NotEmpty(responseBody.FirstName);
        Xunit.Assert.NotEmpty(responseBody.LastName);
        Xunit.Assert.NotEmpty(responseBody.Email);
        Xunit.Assert.NotEmpty(responseBody.Role);
        Xunit.Assert.NotEmpty(responseBody.AccessToken);
        Xunit.Assert.NotEmpty(responseBody.RefreshToken);
    }
    
    public static async Task SuccessEmptyResponseAsync(HttpResponseMessage response, int expectedStatusCode = StatusCodes.Status200OK)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Xunit.Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        Xunit.Assert.Equal(string.Empty, responseBody);
        Xunit.Assert.Null(responseContentType);
    }
    
    public static async Task ConflictResponseAsync(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<ConflictResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Xunit.Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        Xunit.Assert.NotNull(responseBody?.Message);
        Xunit.Assert.NotEmpty(responseBody!.Message);
    }
    
    public static async Task ValidationResponseAsync(HttpResponseMessage response, int expectedErrors)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<ValidationResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Xunit.Assert.Equal(StatusCodes.Status422UnprocessableEntity, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        Xunit.Assert.NotNull(responseBody?.Errors);
        Xunit.Assert.Equal(expectedErrors, responseBody?.Errors.Count());
    }
    
    public static async Task UnauthorizedResponseAsync(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadFromJsonAsync<UnauthorizedResponse>();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Xunit.Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        Xunit.Assert.NotNull(responseBody?.Message);
    }
}