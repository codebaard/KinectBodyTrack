﻿namespace Microsoft.Samples.Kinect.BodyBasics
{
    using System;
    using System.Windows;

    // entry point
    public partial class App : Application
    {
        public static string[] Args;
        void app_Startup(object sender, StartupEventArgs e)
        {
            // If no command line arguments were provided, don't process them if (e.Args.Length == 0) return;  
            if (e.Args.Length > 0)
            {
                Args = e.Args;
            }
        }
    }
}
