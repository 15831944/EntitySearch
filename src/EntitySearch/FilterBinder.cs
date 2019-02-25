using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace EntitySearch
{
    public class FilterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);

            foreach(var property in bindingContext.ModelType.GetProperties())
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(property.Name);
                if (valueProviderResult != ValueProviderResult.None)
                {
                    //var value = Activator.CreateInstance(property.PropertyType);
                    if (valueProviderResult.Length > 1)
                    {
                        bindingContext.ModelState.SetModelValue(property.Name, valueProviderResult);
                        foreach(var value in valueProviderResult.Values)
                        {

                        }
                        property.SetValue(bindingContext.Model, Convert.ChangeType(valueProviderResult.FirstValue, property.PropertyType));
                    }
                    else
                    {
                        bindingContext.ModelState.SetModelValue(property.Name, valueProviderResult);
                        property.SetValue(bindingContext.Model, Convert.ChangeType(valueProviderResult.FirstValue, property.PropertyType));
                    }
                }
            }

            if (bindingContext.Model == null)
            {
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
