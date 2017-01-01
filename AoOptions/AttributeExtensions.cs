using System;
using System.Linq;
using System.Reflection;

namespace AdamOneilSoftware
{
	// adapted from http://dpatrickcaldwell.blogspot.com/2009/03/extension-method-to-get-custom.html
	internal static class AttributeExtensions
	{
		public static T FindAttribute<T>(this ICustomAttributeProvider propertyInfo) where T : Attribute
		{
			T[] attrs = FindAttributes<T>(propertyInfo);
			if (attrs.Any()) return attrs[0];
			return null;
		}

		public static T[] FindAttributes<T>(this ICustomAttributeProvider propertyInfo) where T : Attribute
		{
			object[] attrs = propertyInfo.GetCustomAttributes(typeof(T), false);
			return attrs as T[];
		}

		public static bool HasAttribute<T>(this ICustomAttributeProvider propertyInfo, out T attribute) where T : Attribute
		{			
			attribute = FindAttribute<T>(propertyInfo);
			return (attribute != null);
		}

		public static bool HasAttribute<T>(this ICustomAttributeProvider propertyInfo) where T : Attribute
		{
			T attribute;
			return HasAttribute<T>(propertyInfo, out attribute);
		}
	}
}
