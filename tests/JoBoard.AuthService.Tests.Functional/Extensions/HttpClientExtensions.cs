﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.Login.CanLoginByPassword;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Models.Responses;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Functional.Extensions;

public static class HttpClientExtensions
{
    public static async Task AuthorizeAsync(this HttpClient httpClient, User user)
    {
        var request = new CanLoginByPasswordCommand
        {
            Email = user.Email.Value,
            Password = PasswordFixtures.DefaultPassword
        };
        var response = await httpClient.PostAsJsonAsync(AuthTokenV1Routes.TokenByPassword, request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<AuthResponse>();
        
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", responseBody!.Data.AccessToken);
    }
}