﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Prototest.Library.Version1;
using Prototest.Library.Version13;

namespace Prototest.Library.Version11
{
    public class DefaultTestSetProvider : Version13.ITestSetProvider
    {
        public List<TestSet> GetTestSets(List<TestInputEntry> entries, Dictionary<Type, Func<object>> assertTypes)
        {
            var e = (from cls in entries
                     where (cls.Constructor.GetParameters().All(z => z.ParameterType != typeof(Version1.ICategorize) && z.ParameterType != typeof(Version13.ICategorize)
#if !PLATFORM_UNITY
                     && z.ParameterType != typeof(Version14.ICategorize)
#endif
                     ))
                     let obj = cls.Constructor.Invoke(
                         cls.Constructor.GetParameters()
                             .Select(z => assertTypes[z.ParameterType]())
                             .ToArray())
                     let threadControlState = ((ThreadControl)assertTypes[typeof(IThreadControl)]()).GetAndClearThreadControlMarked()
                     from method in cls.TestMethods
                     where method.GetParameters().Length == 0
#if !PLATFORM_UNITY
                     where method.ReturnType != typeof(System.Threading.Tasks.Task)
#endif
                     select new
                     {
                         TestClass = cls.Type,
                         TestConstructor = cls.Constructor,
                         TestMethod = method,
#if PLATFORM_PCL
                         RunTestMethod = (Action<object>)(objj => ((Action)method.CreateDelegate(typeof(Action), objj))()),
#else
                         RunTestMethod = (Action<object>)(objj => ((Action)Delegate.CreateDelegate(typeof(Action), objj, method))()),
#endif
                         RunOnSingleThread = threadControlState
                     }).ToList();

            return new List<TestSet>
            {
                new TestSet
                {
                    Name = "default",
                    RunSingleThreadedOnMainThread = false,
                    Entries = e.Where(x => !x.RunOnSingleThread).Select(x => new TestSetEntry
                    {
                        TestClass = x.TestClass,
                        TestConstructor = x.TestConstructor,
                        TestMethod = x.TestMethod,
                        RunTestMethod = x.RunTestMethod
                    }).ToList()
                },
                new TestSet
                {
                    Name = "default-singlethreaded",
                    RunSingleThreadedOnMainThread = true,
                    Entries = e.Where(x => x.RunOnSingleThread).Select(x => new TestSetEntry
                    {
                        TestClass = x.TestClass,
                        TestConstructor = x.TestConstructor,
                        TestMethod = x.TestMethod,
                        RunTestMethod = x.RunTestMethod
                    }).ToList()
                },
            };
        }
    }
}
