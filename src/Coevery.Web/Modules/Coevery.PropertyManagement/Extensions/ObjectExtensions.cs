using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Coevery.PropertyManagement
{
    public static class ObjectExtensions {
        public static void CopyPropertiesTo(this object source, object target, params string[] excludeProperties) {
            var targetProperties = TypeDescriptor.GetProperties(target).Cast<PropertyDescriptor>().ToList();
            var sourceProperties = TypeDescriptor.GetProperties(source).Cast<PropertyDescriptor>();

            foreach (var entityProperty in sourceProperties) {
                var property = entityProperty;
                if (excludeProperties != null && excludeProperties.Contains(property.Name)) {
                    continue;
                }
                var convertProperty = targetProperties.FirstOrDefault(prop => prop.Name == property.Name);
                if (convertProperty != null && property.Name != "Id") {
                    convertProperty.SetValue(target, entityProperty.GetValue(source));
                }
            }
        }
    }
}