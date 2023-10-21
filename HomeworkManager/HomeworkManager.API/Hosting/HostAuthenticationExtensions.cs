using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HomeworkManager.API.Hosting
{
    public static class HostAuthenticationExtensions
    {
        public static void AddJwtAuthentication(this WebApplicationBuilder builder)
        {
            var jwtConfiguration = builder.Configuration.GetSection("JWT");
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtConfiguration["Audience"],
                        ValidIssuer = jwtConfiguration["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration["Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}