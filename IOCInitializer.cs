using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace P.IOC
{
    public class IOCInitializer
    {
        static IOCInitializer()
        {
            List<Type> types = new List<Type>();
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            types.AddRange(entryAssembly.DefinedTypes);

            string entryAssemblyPath = Path.GetDirectoryName(entryAssembly.Location);

            IEnumerable<AssemblyName> asmNames = entryAssembly.GetReferencedAssemblies();

            foreach (var reference in asmNames)
            {
                Assembly asm = Assembly.Load(reference.FullName);

                if (entryAssemblyPath == Path.GetDirectoryName(asm.Location))//sadece solution'dakileri ekele
                    types.AddRange(asm.DefinedTypes);

            }

            _types = types;
        }

        private static IEnumerable<Type> _types;
        private static IEnumerable<IOCContainerConfiguration> _containerConfigurations;

        public static void Init(string configPath)
        {
            string json = File.ReadAllText(configPath);
            _containerConfigurations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IOCContainerConfiguration>>(json);

            Type type;
            Type concreteType;
            IOCContainer container;

            foreach (var containerConfiguration in _containerConfigurations)
            {
                container = IOCManager.Create(containerConfiguration.ContainerName);

                foreach (var typeConfiguration in containerConfiguration.TypeConfigurations)
                {

                    GetType(containerConfiguration.ContainerName, typeConfiguration, out type, out concreteType);

                    container.Set(type, concreteType);
                }
            }
        }

        private static void GetType(string containerName, TypeConfiguration typeConfiguration, out Type type, out Type concreteType)//todo su recursive metod daha az parametreli daha iyi olabilir mi?
        {

            if (typeConfiguration.GenericArgumentNames != null && typeConfiguration.GenericArgumentNames.Length > 0)
            {
                Type[] innerTypes = new Type[typeConfiguration.GenericArgumentNames.Length];
                Type innerType;
                Type innerConcreteType;

                for (int i = 0; i < typeConfiguration.GenericArgumentNames.Length; i++)
                {
                    GetType(
                        containerName,
                        _containerConfigurations.Single(x => x.ContainerName == containerName).TypeConfigurations.Single(x => x.TypeName == typeConfiguration.GenericArgumentNames[i]),
                        out innerType,
                        out innerConcreteType
                        );//rule tum T'ler json'a kaydedilmek zorunda yoksa _collection'da bulamadigi icin hata firlatir

                    innerTypes[i] = innerType;
                }

                type = _types.Single(x => x.FullName == $"{typeConfiguration.TypeName}`{typeConfiguration.GenericArgumentNames.Length.ToString()}");
                type = type.MakeGenericType(innerTypes);

                concreteType = _types.Single(x => x.FullName == $"{typeConfiguration.ConcreteTypeName}`{typeConfiguration.GenericArgumentNames.Length.ToString()}");
                concreteType = concreteType.MakeGenericType(innerTypes);
            }
            else
            {
                type = _types.Single(x => x.FullName == typeConfiguration.TypeName);
                concreteType = _types.Single(x => x.FullName == typeConfiguration.ConcreteTypeName);
            }

        }
    }
}
