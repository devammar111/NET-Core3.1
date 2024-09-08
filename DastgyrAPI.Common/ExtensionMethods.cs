using DastgyrAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace DastgyrAPI.Common
{
    public static class ModelStateExtensions
    {
        public static List<Error> GetErrorsList(this ModelStateDictionary modelStateDictionary)
        {
            var newModelStateDictionary = modelStateDictionary.Where(x => x.Value.Errors.Count > 0).ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.FirstOrDefault().ErrorMessage
            );
            return newModelStateDictionary.Select(e => new Error { Title = e.Key, Description = e.Value }).ToList();
        }
        
    }
}
