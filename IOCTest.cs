using System;

namespace P.IOC
{
    public class IOCTest
    {
        public static void Test()
        {
            string containerName = "TestContainer";

            IOCContainer container = IOCManager.Create(containerName)
                .Set<ISubFoo, SubFoo>();

            IOCContainer sameContainer = IOCManager.Get(containerName);

            ISubFoo instance = sameContainer.Get<ISubFoo>();

            int s = instance.Number;
        }

        public static void Test1()
        {
            string containerName = "TestContainer";

            IOCContainer container = IOCManager.Create(containerName)
                .Set<IFoo<ISubFoo>, Foo<ISubFoo>>()
                .Set<ISubFoo, SubFoo>();

            IOCContainer sameContainer = IOCManager.Get(containerName);

            IFoo<ISubFoo> instance = sameContainer.Get<IFoo<ISubFoo>>();

            int s = instance.Sum(1);
        }

        public static void Test2()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}Content\\P-IOC.json";

            IOCInitializer.Init(path);

            var result = IOCManager.Get("Text")
                .Get(typeof(IFoo<ISubFoo>));
        }

        public static void Test3()
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}Content\\P-IOC.json";

            IOCInitializer.Init(path);

            var result = IOCManager.Get("Test")
                .Get(typeof(IComplexFoo<ISubFoo, ISubFoo2>));
        }

        //icin ef ekle
        //public static void Test4()
        //{
        //    IOCContainer container = IOCManager.Create("entp")
        //        .Set<IRepository<TestEntity>, EFRepository<TestEntity>>()
        //        .Set<DbContext, DataContext>();

        //    IRepository<TestEntity> repo = container.Get<IRepository<TestEntity>>();
        //    IRepository<TestEntity> repo2 = container.Get<IRepository<TestEntity>>();
        //    repo.Save(new TestEntity { Name = "LU2" });
        //}
    }

    //_________________________________________________________________________________________________________

    public interface ISubFoo
    {
        int Number { get; set; }
    }
    public class SubFoo : ISubFoo
    {
        public SubFoo()
        {
            Number = 3;
        }

        public int Number { get; set; }
    }

    //_________________________________________________________________________________________________________

    public interface IFoo<T>
    {
        T TProp { get; set; }
        int[] Numberss { get; set; }
        string Text { get; set; }

        int Sum(int x);
    }
    public class Foo<T> : IFoo<T> where T : ISubFoo
    {
        public Foo(string s)
        {
        }

        public Foo(T txProp)
        {
            TProp = txProp;
        }

        public Foo(T txProp, int[] nums)
        {
            TProp = txProp;
            Numberss = nums;
        }

        public T TProp { get; set; }
        public int[] Numberss { get; set; }
        public string Text { get; set; }

        public int Sum(int x)
        {
            return (TProp == null ? 0 : TProp.Number) + x;
        }
    }

    //_________________________________________________________________________________________________________

    public interface ISubFoo2
    {

    }

    public class SubFoo2 : ISubFoo2
    {
        public int Number2 { get; set; }
    }

    public interface IComplexFoo<T1, T2>
    {
        T1 T1Prop { get; set; }
        T2 T2Prop { get; set; }
    }

    public class ComplexFoo<T1, T2> : IComplexFoo<T1, T2>
    {
        public ComplexFoo(T1 t1, T2 t2)
        {
            T1Prop = t1;
            T2Prop = t2;
        }

        public T1 T1Prop { get; set; }
        public T2 T2Prop { get; set; }
    }
}
