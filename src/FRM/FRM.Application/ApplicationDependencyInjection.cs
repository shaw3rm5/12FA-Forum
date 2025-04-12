using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection service)
    {
        service.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
        service.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();
        service.AddScoped<ICreateTopicStorage>();
        service.AddScoped<IIntentionResolver, TopicIntentionResolver>();
        service.AddScoped<IIntentionManager, IntentionManager>();
        
        
        return service;
    }
}