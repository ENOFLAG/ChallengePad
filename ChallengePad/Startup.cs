using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ChallengePad.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChallengePad
{
    public class StartupException : Exception
    {
        public StartupException(string message) : base(message) { }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ChallengePadSettings>(Configuration.GetSection("ChallengePad"));
            var challengePadSettings = Configuration
                .GetSection("ChallengePad")
                .Get<ChallengePadSettings>();
            challengePadSettings.ValidateSettings();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            services.AddAuthorization();
            var authBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            authBuilder.AddCookie();
            if (challengePadSettings.HasCompleteOAuthSettings)
            {
                authBuilder.AddOAuth("OAuth", options =>
                {
                    options.ClientId = challengePadSettings.OAuthClientId;
                    options.ClientSecret = challengePadSettings.OAuthClientSecret;
                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "uid");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Role, "uid");
                    options.CallbackPath = new PathString("/authorized");
                    options.AuthorizationEndpoint = challengePadSettings.OAuthAuthorizationEndpoint;
                    options.TokenEndpoint = challengePadSettings.OAuthTokenEndpoint;
                    options.UserInformationEndpoint = challengePadSettings.OAuthUserInformationEndpoint;
                    options.Scope.Add(challengePadSettings.OAuthScope);
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var info = await response.Content.ReadAsStringAsync();
                            var user = JsonDocument.Parse(info);

                            context.RunClaimActions(user.RootElement);
                        }
                    };
                });
            }

            services.AddChallengePadDb(challengePadSettings);
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMvc();
            services.AddSingleton(typeof(SubscriptionManager));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var db = serviceScope.ServiceProvider.GetRequiredService<IChallengePadDb>();
            db.Migrate().Wait();
        }
    }
}
