namespace Domain.Entities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CascadeSoftDeleteAttribute : Attribute { }
}
