using System;
using System.Reflection;

namespace SpaceInvasionBot
{
	class ObjectWriter
	{        
        public static void Write(object obj)
        {
            foreach (FieldInfo field in obj.GetType().GetFields())
            {
                Console.WriteLine("{0}={1}", field.Name, field.GetValue(obj));
            }
        }
    }
}