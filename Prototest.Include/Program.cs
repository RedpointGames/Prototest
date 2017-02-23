#if PLATFORM_IOS
using UIKit;
#elif PLATFORM_ANDROID
#else
using System;
#if !PLATFORM_UNITY
using System.Collections.Concurrent;
#endif
using System.Diagnostics;
using System.Reflection;
#endif

namespace Prototest.Include
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if PLATFORM_IOS
            UIApplication.Main(args, null, "AppDelegate");
#elif PLATFORM_ANDROID
#else
#if PLATFORM_MACOS
#if PLATFORM_MACOS_LEGACY
            MonoMac.AppKit.NSApplication.Init();
#else
            AppKit.NSApplication.Init();
#endif
#endif
            if (Prototest.Library.Runner.Run(
                Assembly.GetExecutingAssembly(),
                args))
            {
                Environment.Exit(0);
            }

            Environment.Exit(1);
#endif
        }
    }
}

