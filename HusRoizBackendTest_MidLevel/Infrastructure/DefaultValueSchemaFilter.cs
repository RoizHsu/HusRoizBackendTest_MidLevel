using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace HusRoizBackendTest_MidLevel.Infrastructure
{
    public class DefaultValueSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null)
            {
                return;
            }

            foreach (var property in schema.Properties)
            {
                var propertyInfo = context.Type.GetProperty(property.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null && defaultValueAttribute.Value != null)
                    {
                        property.Value.Example = OpenApiAnyFactory.CreateFromJson(System.Text.Json.JsonSerializer.Serialize(defaultValueAttribute.Value));
                    }
                }
            }
        }
    }
}