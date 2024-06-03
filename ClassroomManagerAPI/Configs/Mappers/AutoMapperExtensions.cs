using AutoMapper;
using System.Reflection;

namespace ClassroomManagerAPI.Configs.Mappers
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination>? IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination>? expression)
        {
            if (expression == null)
            {
                return expression;
            }

            Type typeFromHandle = typeof(TSource);
            PropertyInfo[] properties = typeof(TDestination).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                if (typeFromHandle.GetProperties().FirstOrDefault((PropertyInfo p) => p.Name == property.Name) == null)
                {
                    expression.ForMember(property.Name, delegate (IMemberConfigurationExpression<TSource, TDestination, object> opt)
                    {
                        opt.Ignore();
                    });
                }
            }

            return expression;
        }
    }
}
