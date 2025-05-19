using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Storage;
using Forum.Application.UseCases.CreateForum;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Forum.Application.UseCases.GetTopics;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application;

public static class ApplicationDependencyInjection
{
    public static void AddApplicationDependencies(this IServiceCollection service)
    {
        
        service
            .AddTransient<IGuidFactory, GuidFactory>()
            .AddTransient<IMomentProvider, MomentProvider>();
        
        //storages
        service
            .AddScoped<ICreateTopicStorage, CreateTopicStorage>()
            .AddScoped<IGetForumStorage, GetForumStorage>()
            .AddScoped<IGetTopicsStorage, GetTopicsStorage>()
            .AddScoped<ICreateForumStorage, CreateForumStorage>();
        
        // useCaseses
        service
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
            .AddScoped<ICreateForumUseCase, CreateForumUseCase>();

        // useCase validations
        service
            .AddValidatorsFromAssemblyContaining<GetTopicsUseCase>()
            .AddValidatorsFromAssemblyContaining<CreateTopicUseCase>();
        
        // identification
        service
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIdentityProvider, IdentityProvider>();

        service
            .AddMemoryCache();

    }
}