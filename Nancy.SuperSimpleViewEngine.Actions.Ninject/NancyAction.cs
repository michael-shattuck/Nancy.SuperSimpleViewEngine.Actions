using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace Nancy.SuperSimpleViewEngine.Actions.Ninject
{
    public abstract class NancyAction : INancyAction
    {
        protected NancyContext Context;
        protected IViewEngineHost Host;

        internal void Configure(IViewEngineHost host)
        {
            Context = (NancyContext)host.Context;
            Host = host;
        }

        public abstract string Invoke();

        protected string View(string viewName, dynamic model)
        {
            var template = Host.GetTemplate(viewName, model);
            return new ViewEngines.SuperSimpleViewEngine.SuperSimpleViewEngine().Render(template, model, Host);
        }

        protected string View(string viewName)
        {
            return View(viewName, null);
        }
    }
}