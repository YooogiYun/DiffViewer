﻿using DiffViewer.Managers;
using DiffViewer.ViewModels;
using DiffViewer.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using Serilog;
using Serilog.Events;
using System;
using System.Windows;
using VSTSDataProvider.ViewModels;

namespace DiffViewer.Services;


public static class WindowHelper
{
    public static Window SetDataContext(this Window window , object dataContext)
    {
        window.DataContext = dataContext;
        return window;
    }
    public static Window SetOwnerWindow(this Window window , Window owner)
    {
        window.Owner = owner;
        return window;
    }
}

public class ViewModelLocator
{
    public ViewModelLocator( )
    {
        // init IOC Container and register VM services
        Services = InitIoc();
    }

    public IServiceProvider Services { get; set; }

    //private AboutWindow AboutWindowSingleton = new AboutWindow();
    //private object AboutWindowLock = new object();
    //public AboutWindow About_WindowSingleton
    //{
    //    get
    //    {
    //        if( AboutWindowSingleton is null )
    //        {
    //            lock( AboutWindowLock )
    //            {
    //                AboutWindowSingleton ??= new AboutWindow();
    //                return AboutWindowSingleton;
    //            }
    //        }
    //        return AboutWindowSingleton;
    //    }
    //}



    /// <summary>
    /// Init IOC Container
    /// </summary>
    /// <returns></returns>
    public IServiceProvider InitIoc( )
    {
        // create IOC Container
        var services = new ServiceCollection();

        services.AddSingleton<ILogger>(_ =>
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Debug)
                    .WriteTo.Console())
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Information)
                    .WriteTo.File("./Log/log.txt" ,
                                       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}" ,
                                       rollingInterval: RollingInterval.Hour ,
                                       fileSizeLimitBytes: 1024 * 1024 * 10 ,
                                       retainedFileCountLimit: 31 ,
                                       rollOnFileSizeLimit: true ,
                                       flushToDiskInterval: new(
                                                                          0 ,
                                                                          0 ,
                                                                          30
                                                                      ) ,
                                       buffered: false))
                .CreateLogger();
        });
        services.AddSingleton<IDialogService>(_ => { return new DialogService(); });
        services.AddSingleton<AppConfigManager>();
        // register VM services
        services.AddTransient<AboutViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddSingleton<MainWindow>(sp => new MainWindow { DataContext = MainWindow_ViewModel });
        services.AddTransient<AboutWindow>(sp => new AboutWindow { DataContext = About_ViewModel , Owner = Main_Window });

        //var a = services.BuildServiceProvider();
        //Console.WriteLine(a.GetRequiredService<AboutViewModel>().loggerAbout.Equals(a.GetRequiredService<MainWindowViewModel>().logger1));
        //int ab = 0;

        // return IOC Container
        return services.BuildServiceProvider();
    }

    public ILogger Logger => Services.GetRequiredService<ILogger>();

    public MainWindowViewModel MainWindow_ViewModel => Services.GetRequiredService<MainWindowViewModel>();

    public AboutViewModel About_ViewModel => Services.GetRequiredService<AboutViewModel>();

    public MainWindow Main_Window => Services.GetRequiredService<MainWindow>();

    public AboutWindow About_Window => Services.GetRequiredService<AboutWindow>();

}