
namespace P.IOC
{
    public class IOCContainerConfiguration
    {
        public string ContainerName { get; set; }
        public TypeConfiguration[] TypeConfigurations { get; set; }
    }

    public class TypeConfiguration
    {
        public string TypeName { get; set; }
        public string[] GenericArgumentNames { get; set; }
        public string ConcreteTypeName { get; set; }
    }
}
