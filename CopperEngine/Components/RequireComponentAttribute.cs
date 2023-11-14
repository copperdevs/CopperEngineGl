using CopperEngine.Logs;

namespace CopperEngine.Components;

// TODO: Implement this
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class RequireComponentAttribute : Attribute
{
    internal readonly Type? ComponentType;

    public RequireComponentAttribute(Type componentType)
    {
        if (componentType.BaseType == typeof(GameComponent)) 
            ComponentType = componentType;

        Log.Error($"{nameof(GetType)} input component type isn't a component. It's a {componentType.FullName}");
        
        ComponentType = null;
    }
}