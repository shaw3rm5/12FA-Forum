using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.GetForums;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Domain;

public static class DomainDependencyInjection
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection service)
    {
        service.AddScoped<IMomentProvider, MomentProvider>();
        service.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
        service.AddTransient<IGuidFactory, GuidFactory>();
        service.AddTransient<IMomentProvider, MomentProvider>(); 
        service.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();

        return service;
    }
}