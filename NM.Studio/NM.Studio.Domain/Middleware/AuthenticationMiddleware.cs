﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;

namespace NM.Studio.Domain.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // InformationUser.User = null;
        // var origin = context.Request.Headers["Origin"].ToString(); 
        // var referer = context.Request.Headers["Referer"].ToString(); 
        // InformationUser.Origin = origin;
        // InformationUser.Referer = referer;
        
        
        // Lấy token từ cookies
        // var token = context.Request.Cookies["accessToken"];
        //
        // if (!string.IsNullOrEmpty(token) && token != "null")
        // {
        //     var id = GetUserIdFromToken(token);
        //     if (id != Guid.Empty)
        //     {
        //         using (var scope = _serviceScopeFactory.CreateScope())
        //         {
        //             var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        //             var userRepository = unitOfWork.UserRepository;
        //             var user = await userRepository.GetById(id);
        //             InformationUser.User = user;
        //         }
        //     }
        // }

        await _next(context);
    }

    // private Guid GetUserIdFromToken(string token)
    // {
    //     try
    //     {
    //         var handler = new JwtSecurityTokenHandler();
    //         var jwtToken = handler.ReadJwtToken(token);
    //         var id = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    //         return Guid.Parse(id);
    //     }
    //     catch (Exception ex)
    //     {
    //         // Log exception if necessary
    //         return Guid.Empty;
    //     }
    // }
}