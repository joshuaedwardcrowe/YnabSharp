using Microsoft.Extensions.DependencyInjection;
using YnabSharpa.Factories;
using YnabSharpa.Http;

namespace YnabSharpa.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYnab(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddHttpClient()
            .AddSingleton<YnabHttpClientBuilder>()
            .AddTransactionFactory();

    private static IServiceCollection AddTransactionFactory(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ITransactionFactory, DefaultTransactionFactory>();

    public static IServiceCollection AddYnabTransactionFactory<TYnabTransactionFactory>(this IServiceCollection services)
        where TYnabTransactionFactory : class, ITransactionFactory
        => services.AddInFrontOf<ITransactionFactory, TYnabTransactionFactory, DefaultTransactionFactory>();

    private static IServiceCollection AddInFrontOf<TInterface, TToAddToFront, TMoveToBack>(this IServiceCollection services)
        where TInterface : class where TToAddToFront : class, TInterface where TMoveToBack : class, TInterface
    {
        var moveToBack = services.First(serviceDescriptor => serviceDescriptor.ImplementationType == typeof(TMoveToBack));
        services.Remove(moveToBack);
        
        services.AddSingleton<TInterface, TToAddToFront>();
        services.AddSingleton<TInterface, TMoveToBack>();
        
        return services;
    }
}