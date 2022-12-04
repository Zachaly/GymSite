namespace GymSite.Domain.Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Implementation : Attribute
    {
        public Type Type { get; private set; }

        public Implementation(Type type)
        {
            Type = type;
        }
    }
}
