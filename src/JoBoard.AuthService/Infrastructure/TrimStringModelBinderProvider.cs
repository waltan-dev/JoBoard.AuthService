using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace JoBoard.AuthService.Infrastructure;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding
public class TrimStringModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (context.Metadata.IsComplexType || context.Metadata.ModelType != typeof(string)) 
            return null;
        
        var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
        return new TrimStringModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType, loggerFactory));
    }
}