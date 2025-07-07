using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pictura.Api.ModelBinders
{
    public class CommaSeparatedModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(new List<string>());
                return Task.CompletedTask;
            }
            
            var items = value.Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim())
                .ToList();
            
            bindingContext.Result = ModelBindingResult.Success(items);
            return Task.CompletedTask;
        }
    }
}
