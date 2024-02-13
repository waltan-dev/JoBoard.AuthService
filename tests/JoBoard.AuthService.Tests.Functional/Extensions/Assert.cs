using System.Net.Http.Json;
using System.Net.Mime;
using JoBoard.AuthService.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.Tests.Functional.Extensions;

public static class Assert
{
    public static async Task SuccessAuthResponseAsync(HttpResponseMessage response)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<AuthResponse>();
        AssertBaseResponseBody(responseBody);
        
        Xunit.Assert.NotEqual(default, responseBody!.Data.UserId);
        Xunit.Assert.NotEmpty(responseBody.Data.FirstName);
        Xunit.Assert.NotEmpty(responseBody.Data.LastName);
        Xunit.Assert.NotEmpty(responseBody.Data.Email);
        Xunit.Assert.NotEmpty(responseBody.Data.Role);
        Xunit.Assert.NotEmpty(responseBody.Data.AccessToken);
        Xunit.Assert.NotEmpty(responseBody.Data.RefreshToken);
    }
    
    public static async Task SuccessEmptyResponseAsync(HttpResponseMessage response, int expectedStatusCode = StatusCodes.Status200OK)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<EmptyResponse>();
        AssertBaseResponseBody(responseBody);
    }
    
    public static async Task NotFoundResponseAsync(HttpResponseMessage response)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<NotFoundResponse>();
        AssertBaseResponseBody(responseBody);
    }
    
    public static async Task ConflictResponseAsync(HttpResponseMessage response)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<ConflictResponse>();
        AssertBaseResponseBody(responseBody);
    }
    
    public static async Task ValidationResponseAsync(HttpResponseMessage response, int expectedErrors)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(StatusCodes.Status422UnprocessableEntity, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<ValidationResponse>();
        AssertBaseResponseBody(responseBody);
        
        Xunit.Assert.NotNull(responseBody?.Errors);
        Xunit.Assert.Equal(expectedErrors, responseBody?.Errors.Count());
    }
    
    public static async Task UnauthorizedResponseAsync(HttpResponseMessage response)
    {
        // check header
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        Xunit.Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        Xunit.Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
        
        // check body
        var responseBody = await response.Content.ReadFromJsonAsync<UnauthorizedResponse>();
        AssertBaseResponseBody(responseBody);
    }
    
    private static void AssertBaseResponseBody(BaseResponse? responseBody)
    {
        Xunit.Assert.NotNull(responseBody);
        Xunit.Assert.NotEqual(default, responseBody!.Code);
        Xunit.Assert.NotNull(responseBody.Message);
        Xunit.Assert.NotEmpty(responseBody.Message);
    }
}