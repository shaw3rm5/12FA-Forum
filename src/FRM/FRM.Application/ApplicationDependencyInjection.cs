using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Storage;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection service)
    {
        service
            .AddTransient<IGuidFactory, GuidFactory>()
            .AddTransient<IMomentProvider, MomentProvider>()
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIdentityProvider, IdentityProvider>()
            .AddValidatorsFromAssemblyContaining<CreateTopicUseCase>()
            .AddScoped<IGetForumStorage, GetForumStorage>();
        
        return service;
    }
}