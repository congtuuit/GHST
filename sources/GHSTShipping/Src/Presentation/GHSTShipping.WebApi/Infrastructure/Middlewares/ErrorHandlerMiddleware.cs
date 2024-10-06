using FluentValidation;
using GHSTShipping.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;  // Add this using statement
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GHSTShipping.WebApi.Infrastructure.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;  // Add logger field

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)  // Update constructor
        {
            _next = next;
            _logger = logger;  // Initialize logger
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = BaseResult.Failure();

                // Log the exception
                _logger.LogError(error, "An unhandled exception occurred: {Message}", error.Message);

                switch (error)
                {
                    case ValidationException e:
                        // validation error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        foreach (var validationFailure in e.Errors)
                        {
                            responseModel.AddError(new Error(ErrorCode.ModelStateNotValid, validationFailure.ErrorMessage, validationFailure.PropertyName));
                        }
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.AddError(new Error(ErrorCode.NotFound, e.Message));
                        break;
                    case UnauthorizedAccessException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.AddError(new Error(ErrorCode.AccessDenied, e.Message));
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.AddError(new Error(ErrorCode.Exception, error.Message));
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await response.WriteAsync(result);
            }
        }
    }
}
