using EntitySearch.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
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

            foreach (var property in bindingContext.ModelType.GetProperties())
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(property.Name);
                if (valueProviderResult != ValueProviderResult.None)
                {
                    if (valueProviderResult.Length > 1)
                    {
                        if (property.Name == "QueryProperties")
                        {
                            valueProviderResult.Values.ToList().ForEach(value => ((IFilter)bindingContext.Model).QueryProperties.Add(value));
                        }
                    }
                    else
                    {
                        property.SetValue(bindingContext.Model, Convert.ChangeType(valueProviderResult.FirstValue, property.PropertyType));
                    }

                    bindingContext.ModelState.SetModelValue(property.Name, valueProviderResult);
                }
                else
                {
                    if (property.Name == "FilterProperties")
                    {
                        foreach (var filterProperty in bindingContext.ModelType.BaseType.GenericTypeArguments[0].GetProperties())
                        {
                            valueProviderResult = bindingContext.ValueProvider.GetValue(filterProperty.Name);
                            if (valueProviderResult != ValueProviderResult.None)
                            {
                                if (valueProviderResult.Length == 1)
                                {
                                    bool changed = false;
                                    object typedValue = TryChangeType(valueProviderResult.FirstValue, filterProperty.PropertyType, ref changed);
                                    if (changed)
                                    {
                                        ((IFilter)bindingContext.Model).FilterProperties.Add(filterProperty.Name, typedValue);
                                    }
                                }
                            }
                        }
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

        private object TryChangeType(string value, Type typeTo, ref bool changed)
        {
            try
            {
                object convertedObject = Convert.ChangeType(value, typeTo);
                changed = true;
                return convertedObject;
            }
            catch (Exception)
            {
                changed = false;
                return Activator.CreateInstance(typeTo);
            }
        }
    }
}
