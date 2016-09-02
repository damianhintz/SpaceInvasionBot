using System;
using System.Reflection;

namespace SpaceInvasionBot
{
	class ObjectCopy
	{
		private static FieldInfo GetField(object obj, string name)
		{
			foreach (FieldInfo field in obj.GetType().GetFields())
            {
	            if(field.Name == name)
	            {
		            return field;
	            }
            }
            return null;
		}
		
		public static void Copy(object src, object dst)
		{
			Type type = dst.GetType();
			foreach (FieldInfo field in type.GetFields())
            {
	            FieldInfo srcField = GetField(src, field.Name);
	            if(srcField != null)
	            {
		            object value = srcField.GetValue(src);
		            type.InvokeMember(field.Name, BindingFlags.SetField, null, dst, new object[] { value });
	            }
            }
		}
	}
}