using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.Storage;
using Forum.Application.UseCases.CreateForum;
using Forum.Application.UseCases.CreateTopic;
using Forum.Application.UseCases.GetForums;
using Forum.Application.UseCases.GetTopics;
using Forum.Application.UseCases.SignIn;
using Forum.Application.UseCases.SignOut;
using Forum.Application.UseCases.SignUp;
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
            .AddScoped<ICreateForumStorage, CreateForumStorage>()
            .AddScoped<ISignInStorage, SignInStorage>()
            .AddScoped<ISignUpStorage, SignUpStorage>()
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .AddScoped<ISignOutStorage, SignOutStorage>();
        
        // useCaseses
        service
            .AddScoped<IGetForumsUseCase, GetForumsUseCase>()
            .AddScoped<ICreateTopicUseCase, CreateTopicUseCase>()
            .AddScoped<IGetTopicsUseCase, GetTopicsUseCase>()
            .AddScoped<ICreateForumUseCase, CreateForumUseCase>()
            .AddScoped<ISignUpUseCase, SignUpUseCase>()
            .AddScoped<ISignInUseCase, SignInUseCase>()
            .AddScoped<ISignOutUseCase, SignOutUseCase>();

        // useCase validations
        service
            .AddValidatorsFromAssemblyContaining<GetTopicsUseCase>(includeInternalTypes: true);
            
        
        // identification
        service
            .AddScoped<IIntentionManager, IntentionManager>()
            .AddScoped<IIntentionResolver, TopicIntentionResolver>()
            .AddScoped<IIdentityProvider, IdentityProvider>();
        
        // authentication
        service
            .AddScoped<ISymmetricDecryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<ISymmetricEncryptor, AesSymmetricEncryptorDecryptor>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddScoped<IAuthenticationService, AuthenticationService>();
            
        service
            .AddMemoryCache();

    }
}