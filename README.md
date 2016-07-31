# Nancy.SuperSimpleViewEngine.Actions
The goal of this library is to provide a simple way to include injectable actions (submodules? maybe?) in views. Actions, at their core, exist to enable developers to seperate sections of a view. Actions act like Nancy modules. They are injectable and can render views, but are not tied to a specific url. Instead actions are referenced from inside a view, such as:
`@Action['Test']`. This is similar to rendering a partial view.

## Documentation
An action is simply a class that inherits from the `NancyAction` abstract class. It will be auto-descovered by the framework.
Currently `NancyAction` exposes the following items:
* `protected NancyContext Context { get; }`: The current request context.
* `protected string View(string viewName);`: Renders the referenced view
* `protected string View(string viewName, dynamic model);`: Renders the referenced view and passes a dynamic model

Views are located using default and custom view locations.

Example action:
```csharp
public class TestAction : NancyAction
{
    readonly IAmazingService amazingService;

    public TestAction(IAmazingService amazingService)
    {
        this.amazingService = amazingService;
    }

    public override string Invoke()
    {
        return View("test", amazingService.ShowMe());
    }
}
```

Example usage:
```html
<div>
	@Action['Test']
</div>
```

## Container Support
Currently only `TinyIoC` and `Ninject` are supported.

## Installation using NuGet
* For TinyIoC: `Install-Package Nancy.SuperSimpleViewEngine.Actions.TinyIoC`
* For Ninject: `Install-Package Nancy.SuperSimpleViewEngine.Actions.Ninject`
