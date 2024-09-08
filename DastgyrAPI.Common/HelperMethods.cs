using DastgyrAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DastgyrAPI.Common
{
    public static class HelperMethods
    {
        public static T CreateErrorResponse<T>(List<Error> errors) where T : ObjectResult
        {
            var errorCode = StatusCodes.Status400BadRequest.ToString();
            if (typeof(T) == typeof(UnauthorizedObjectResult))
            {
                errorCode = StatusCodes.Status401Unauthorized.ToString();
            }
            else if (typeof(T) == typeof(NotFoundObjectResult))
            {
                errorCode = StatusCodes.Status404NotFound.ToString();
            }
            object[] args = new object[] { new ErrorResponse() { Code = errorCode, Errors = errors } };
            return (T)Activator.CreateInstance(typeof(T), args);
        }
        public static async Task<IActionResult> ResponseBasedOnType(Type t, ModelStateDictionary modelState)
        {
            if (t == typeof(UnauthorizedObjectResult))
            {
                return CreateErrorResponse<UnauthorizedObjectResult>(modelState.GetErrorsList());
            }
            else if (t == typeof(NotFoundObjectResult))
            {
                return CreateErrorResponse<NotFoundObjectResult>(modelState.GetErrorsList());
            }
            else
            {
                return CreateErrorResponse<BadRequestObjectResult>(modelState.GetErrorsList());
            }
        }
    }
}
