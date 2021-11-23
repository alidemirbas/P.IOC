using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace P.IOC
{
    public class IOCContainer
    {
        public IOCContainer(string name)
        {
            _regList = new List<Registry>();

            Name = name;
        }

        private List<Registry> _regList;

        public string Name { get; set; }
        //public IOC IOC { get; set; }

        public IOCContainer Set(Type type,Type concreteType)
        {
            if (_regList.Any(x => x.Type == type))
                throw new Exception();//todo bi kaydedilen tip bi daha kaydedilemez yani A:P ve B:P olsun P ya A olarak yada B olarak eklenebilir

            if (!type.IsAssignableFrom(concreteType))
                throw new Exception();//todo

            IEnumerable<Guid> regGuids = _regList.Select(x => x.Type.GUID);

            Registry reg = new Registry(type, concreteType, regGuids);
            _regList.Add(reg);

            return this;
        }

        public IOCContainer Set(Type concreteType)
        {
            Set(concreteType, concreteType);

            return this;
        }

        public IOCContainer Set<T, Tc>()
        {
            Set(typeof(T),typeof(Tc));

            return this;
        }

        public IOCContainer Set<T>()
        {
            Set(typeof(T));

            return this;
        }

        public IOCContainer Delete(Type type)
        {
            Registry reg = _regList.Find(x => x.Type == type);

            bool result=!(reg == null) || _regList.Remove(reg);

            return this;
        }

        public IOCContainer Delete<Tp>()
        {
            Delete(typeof(Tp));

            return this;
        }

        //recursive
        public object Get(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type == typeof(string))
                return null;

            Registry reg = _regList.Find(x => x.Type == type);

            if (reg == null)
                return null;

            ParameterInfo[] pInfs;
            List<object> args;

            pInfs = reg.ConstructorInfo.GetParameters();
            args = new List<object>();

            foreach (var pInf in pInfs)
            {
                object arg = Get(pInf.ParameterType);
                args.Add(arg);//null da olsa ekle
            }

            return reg.ConstructorInfo.Invoke(args.ToArray());//Activator ile Ambigious oluyor zati boylesi daga gozel
            //return Activator.CreateInstance(actualType, args.ToArray());

            //note "params"'a array pass ettin mi teker teker atamissin gibi davraniyor
            //orn x(params int[] n) icin
            // x(new int[]{1,2}) ile x(1,2) ayni isi yapiyor
            //bu sayede Activator.CreateInstance(type, args) gibi bir durum icin args olarak bir new object[] kullanabiliyorsun
            //bu ozellik icin tipin params'in tipi ile array'in tipi ayni olmasi sart aksi halde o params'ta sadece birinci param olarak davranir
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
    }
}
