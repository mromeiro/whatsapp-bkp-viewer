using Microsoft.Extensions.DependencyInjection;
using Wbv.WhatsappDigester.Digester;

namespace Wbv.WhatsappDigester;

public static class WhatsappDigesterExtensions
{
    public static void RegisterWhatsappDigesterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IWhatsappDigester, Digester.WhatsappDigester>();
    }
}