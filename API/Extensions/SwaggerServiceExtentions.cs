﻿using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class SwaggerServiceExtentions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c => 
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Voting system", Version = "v1"});
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Auth Bearer Scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement {{securitySchema, new[] {"Bearer"}}};
            c.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumention(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => {c
            .SwaggerEndpoint("/swagger/v1/swagger.json", "Voting system");});

        return app;
    }
}