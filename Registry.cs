using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace P.IOC
{
    class Registry
    {
        public Registry(Type type, Type concreteType, IEnumerable<Guid> typeGuids)
        {
            Index = _index++;
            this.Type = type;
            ConcreteType = concreteType;
            this.TypeGuids = typeGuids;
        }

        private static int _index = 0;

        public int Index { get; set; }
        public Type Type { get; set; }
        public Type ConcreteType { get; set; }

        private ConstructorInfo _constructorInfo;
        public ConstructorInfo ConstructorInfo
        {
            get
            {
                //bunu yapma cunku json config degisebilir
                //if (_constructorInfo != null)
                //    return _constructorInfo;

                ConstructorInfo[] ctors = ConcreteType.GetConstructors();
                _constructorInfo = ctors[0]; //maksat null donmemek hani olur da ctor'daki hic bir parameter kaydedilmemistir. hic degilse reflection ile instance olustururken ilk ctor'u gorup kullanalim diye

                int i = 0;
                int m = 0;
                foreach (var ctor in ctors)
                {
                    i = 0;

                    var pInfs = ctor.GetParameters();

                    foreach (var pInf in pInfs)
                    {
                        if (IsExist(pInf.ParameterType.GUID))
                            i++;//ctorun kaydedilmis prm sayisi
                    }


                    if (i > m)
                    {
                        m = i;//ctor'lardan kaydedilmis prm sayisi en fazla olan (gostermelik)
                        _constructorInfo = ctor;//suanlik parametresi en cok kaydedilmis bu
                    }

                    //rule
                    //dinamik prop boylesi daha guzel . boyle olursa kayit sirasi onemli hale geliyor
                    //siranin onemsiz olmasi ve istenildigi gibi ekle / cikar yapilabilmesi icin bu islemler get'e tasindi
                }

                return _constructorInfo;
            }
        }

        public IEnumerable<Guid> TypeGuids { get; set; }

        private bool IsExist(Guid typeGuid)
        {
            return TypeGuids.Contains(typeGuid);
        }
    }
}
