﻿#if !PLATFORM_UNITY

using System.Reflection;

namespace Prototest.Library.Version11
{
    public interface ITestRunner
    {
        bool Run(
            Assembly assembly,
            ITestConnector connector,
            string[] args);
    }
}

#endif