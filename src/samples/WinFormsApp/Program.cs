﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unleash;

namespace WinFormsApp
{
    static class Program
    {
        private static IUnleash unleash;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new UnleashForm();

            var unleashSettings = new UnleashSettings
            {
                UnleashApi = new Uri("http://unleash.herokuapp.com/"),
                //UnleashApi = new Uri("http://localhost:4242/"),
                AppName = "dotnet-forms-test",
                InstanceTag = "instance 1",
                SendMetricsInterval = TimeSpan.FromSeconds(20),
                UnleashContextProvider = new WinFormsContextProvider(form),
                //JsonSerializer = new JsonNetSerializer()
            };

            unleash = new DefaultUnleash(unleashSettings);
            form.Unleash = unleash;

            Application.ApplicationExit += (sender, args) =>
            {
                unleash?.Dispose();
            };

            Application.Run(form);
        }
    }

    internal class WinFormsContextProvider : IUnleashContextProvider
    {
        private readonly UnleashForm form;

        public WinFormsContextProvider(UnleashForm form)
        {
            this.form = form;
        }

        public UnleashContext Context => new UnleashContext(
            form.UsernameTextBox.Text, 
            "session1", 
            "remoteAddress", 
            new Dictionary<string, string>());
    }
}