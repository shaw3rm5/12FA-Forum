using FluentValidation;
using Forum.Application.Authorization;
using Forum.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FRM.API.Middlewares;

public static class ProblemDetailsFactoryExtension
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        IntentionManagerException intentionManagerException)
    {
        return factory.CreateProblemDetails(httpContext, StatusCodes.Status403Forbidden, 
            "Authorization Failed", detail: intentionManagerException.Message);
    }

    public static ProblemDetails CreateFrom(
        this ProblemDetailsFactory factory,
        HttpContext httpContext,
        ApplicationLayerException applicationLayerException)
    {
        return factory.CreateProblemDetails(httpContext, applicationLayerException.ErrorCode switch
        {
            ErrorCodes.Gone => StatusCodes.Status410Gone,   
            ErrorCodes.NotFound => StatusCodes.Status404NotFound,
            ErrorCodes.BadRequest => StatusCodes.Status400BadRequest,
            ErrorCodes.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        }, applicationLayerException.Message);
        
        
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }
        return factory.CreateValidationProblemDetails(httpContext, modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Invalid Request", detail: validationException.Message);
    }
}
