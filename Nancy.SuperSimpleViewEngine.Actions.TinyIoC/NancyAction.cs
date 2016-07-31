using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace Nancy.SuperSimpleViewEngine.Actions.TinyIoC
{
    public abstract class NancyAction : INancyAction
    {
        protected NancyContext Context { get; private set; }
        IViewEngineHost host;

        internal void Configure(IViewEngineHost viewEngineHost)
        {
            Context = (NancyContext)viewEngineHost.Context;
            host = viewEngineHost;
        }

        public abstract string Invoke();

        protected string View(string viewName, dynamic model)
        {
            var template = host.GetTemplate(viewName, model);
            return new ViewEngines.SuperSimpleViewEngine.SuperSimpleViewEngine().Render(template, model, host);
        }

        protected string View(string viewName)
        {
            return View(viewName, null);
        }
    }
}