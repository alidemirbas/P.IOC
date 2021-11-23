using System;
using System.Collections.Generic;
using System.Linq;

namespace P.IOC
{
    public class IOCManager
    {
        static IOCManager()
        {
            _containers = new List<IOCContainer>();
        }

        private static List<IOCContainer> _containers;

        public static IOCContainer Create(string name)
        {
            if (_containers.Any(x => x.Name == name))
                throw new Exception(); //todo ayni isimde birden fazla container olmaz

            IOCContainer container = new IOCContainer(name);

            _containers.Add(container);

            return container;
        }

        public static IOCContainer Get(string name)
        {
            return _containers.Find(x => x.Name == name);
        }
    }
}
