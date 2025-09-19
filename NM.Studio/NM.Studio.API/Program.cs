﻿using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NM.Studio.API.Extensions;
using NM.Studio.Domain.Configs.Mapping;
using NM.Studio.Domain.Configs.Slugify;
using NM.Studio.Domain.Shared.Handler;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddAppConfiguration();
// Add Cors
builder.Services.ConfigureCors(builder.Configuration);
// Add serilog
builder.Host.AddSerilog(builder.Configuration);
// Exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.ConfigureQuartz();
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(
        new RouteTokenTransformerConvention(new SlugifyParameterTransformer())
    );
});

builder.Services.ConfigureContext(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureBind();
builder.Services.ConfigureAuth();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

// -----------------app-------------------------

var app = builder.Build();

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(ServiceExtensions.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthenticationMiddleware>();
app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.Lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("[Successfully] Server started successfully and is listening for requests...");
});

app.Run();