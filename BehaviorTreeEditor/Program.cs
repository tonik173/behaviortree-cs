// Copyright(c) 2015 Eugeny Novikov. Code under MIT license.

using BehaviorTreesEditor;
using System;
using System.Windows.Forms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BehaviorTrees.Engine;

namespace BehaviorTreesExample
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var host = CreateHostBuilder().Build();
			var serviceProvider = host.Services;

			Application.Run(serviceProvider.GetRequiredService<BTEditorForm>());
		}

		static IHostBuilder CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<Engine>();
					services.AddTransient<BTEditorForm>();
				});
		}
	}
}
