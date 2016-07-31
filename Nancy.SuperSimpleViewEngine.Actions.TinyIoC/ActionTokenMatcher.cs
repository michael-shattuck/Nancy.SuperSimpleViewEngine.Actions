using System;
using System.Linq;
using System.Text.RegularExpressions;
using Nancy.TinyIoc;
using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace Nancy.SuperSimpleViewEngine.Actions.TinyIoC
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
                var matchingActions = GetType().Assembly.GetTypes().Where(x =>
                    (x.Name == actionName || x.Name.Replace("Action", "") == actionName)
                    && x.IsClass
                    && x.GetInterface(nameof(INancyAction)) != null).ToList();

                if (!matchingActions.Any())
                {
                    throw new InvalidOperationException($"An action named '{actionName}' was not found");
                }

                if (matchingActions.Count > 1)
                {
                    throw new InvalidOperationException($"Multiple actions named '{actionName}' were found");
                }

                var container = (TinyIoCContainer)((NancyContext)host.Context).Items.FirstOrDefault(x => x.Key.Contains("Container")).Value;
                var action = (NancyAction)container.Resolve(matchingActions.First());
                action.Configure(host);

                return action.Invoke();
            });
        }
    }
}