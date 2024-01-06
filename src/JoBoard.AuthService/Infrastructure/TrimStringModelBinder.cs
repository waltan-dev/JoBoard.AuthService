using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JoBoard.AuthService.Infrastructure;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding
public class TrimStringModelBinder : IModelBinder
{
    private readonly IModelBinder _fallbackBinder;
        
    public TrimStringModelBinder(IModelBinder fallbackBinder)
    {
        _fallbackBinder = fallbackBinder;
    }
    
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));
        
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if(string.IsNullOrEmpty(valueProviderResult.FirstValue))
            return _fallbackBinder.BindModelAsync(bindingContext);
        
        bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue.Trim());
        return Task.CompletedTask;
    }
}