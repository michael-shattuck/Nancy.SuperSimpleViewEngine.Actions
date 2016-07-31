using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.SuperSimpleViewEngine.Actions.TinyIoC
{
    internal static class ActionTypeProvider
    {
        public static readonly IEnumerable<Type> RegisteredActions = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && x.GetInterface(nameof(INancyAction)) != null).ToList();
    }
}