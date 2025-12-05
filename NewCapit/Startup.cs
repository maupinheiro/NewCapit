using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(NewCapit.Startup))]

namespace NewCapit
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Permite chamadas cross-origin (ajuste conforme segurança)
            app.UseCors(CorsOptions.AllowAll);

            // Mapeia SignalR em /signalr
            app.Map("/signalr", map =>
            {
                var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJavaScriptProxies = true // isso gera /signalr/hubs
                };

                // Se for necessário JSON Net settings, configure aqui
                // hubConfiguration.Resolver = ... 

                map.RunSignalR(hubConfiguration);
            });

            // (Opcional) middleware de diagnóstico
            // app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
