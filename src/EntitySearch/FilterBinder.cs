using EntitySearch.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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
                            GetPropertyTypeBinders(filterProperty.PropertyType).ForEach(typeBinder =>
                            {
                                var filterName = $"{filterProperty.Name}{typeBinder}";
                                valueProviderResult = bindingContext.ValueProvider.GetValue(filterName);
                                if (valueProviderResult != ValueProviderResult.None)
                                {
                                    var listObjects = new List<object>();
                                    valueProviderResult.Values.ToList().ForEach(value =>
                                    {
                                        bool changed = false;
                                        object typedValue = TryChangeType(value, filterProperty.PropertyType, ref changed);
                                        if (changed)
                                        {
                                            listObjects.Add(typedValue);
                                        }
                                    });

                                    if (listObjects.Count > 0)
                                    {
                                        ((IFilter)bindingContext.Model).FilterProperties.Add(filterName, listObjects.Count > 1 ? listObjects : listObjects.FirstOrDefault());
                                    }
                                }
                            });
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

        private List<string> GetPropertyTypeBinders(Type propertyType)
        {
            List<string> comparationTypes = new List<string>();

            comparationTypes.Add("");
            comparationTypes.Add("_Not");
            if (propertyType == typeof(string) || propertyType == typeof(char))
            {
                comparationTypes.Add("_Contains");
                comparationTypes.Add("_NotContains");
            }

            if (propertyType == typeof(int)
                    || propertyType == typeof(long)
                    || propertyType == typeof(float)
                    || propertyType == typeof(float)
                    || propertyType == typeof(double)
                    || propertyType == typeof(decimal)
                    || propertyType == typeof(DateTime)
                    || propertyType == typeof(TimeSpan))
            {
                comparationTypes.Add("_GreaterThan");
                comparationTypes.Add("_SmallerThan");
                comparationTypes.Add("_GreaterEqual");
                comparationTypes.Add("_SmallerEqual");
            }

            return comparationTypes.Distinct().ToList();
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
