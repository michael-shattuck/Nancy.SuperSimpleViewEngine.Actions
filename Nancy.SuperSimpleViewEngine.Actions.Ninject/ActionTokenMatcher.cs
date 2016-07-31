using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy.ViewEngines.SuperSimpleViewEngine;
using Ninject;
using Ninject.Extensions.ChildKernel;

namespace Nancy.SuperSimpleViewEngine.Actions.Ninject
{
    public class ActionTokenMatcher : ISuperSimpleViewEngineMatcher
    {
        static readonly Regex TokenPattern;

        static ActionTokenMatcher()
        {
            TokenPattern = new Regex(@"@Action\[\'(?<ActionName>[a-zA-Z0-9-_]+)\'\]?", RegexOptions.Compiled);
        }

        public string Invoke(string content, dynamic model, IViewEngineHost host)
        {
            return TokenPattern.Replace(content, match =>
            {
                var actionName = match.Groups["ActionName"].Value;
                var matchingActions = ActionTypeProvider.RegisteredActions
                    .Where(x => x.Name == actionName || x.Name.Replace("Action", "") == actionName)
                    .ToList();

                if (!matchingActions.Any())
                {
                    throw new InvalidOperationException($"An action named '{actionName}' was not found");
                }

                if (matchingActions.Count > 1)
                {
                    throw new InvalidOperationException($"Multiple actions named '{actionName}' were found");
                }

                var container = (ChildKernel)((NancyContext)host.Context).Items.FirstOrDefault(x => x.Key.Contains("Container")).Value;
                var action = (NancyAction)container.Get(matchingActions.First());
                action.Configure(host);

                return action.Invoke();
            });
        }
    }
}