﻿using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiteDbExplorer.Framework.Shell;
using LogManager = NLog.LogManager;

namespace LiteDbExplorer.Modules.Main
{
    [Export(typeof(IShell))]
    [PartCreationPolicy (CreationPolicy.Shared)]
    public sealed class ShellViewModel : Screen, IShell
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public ShellViewModel()
        {
            DisplayName = "LiteDB Explorer";
            
            WindowMenu = IoC.Get<IShellMenu>();

            WindowRightMenu = IoC.Get<IShellRightMenu>();

            StatusBarContent = IoC.Get<IShellStatusBar>();

            LeftContent = IoC.Get<IDocumentExplorer>();

            MainContent = IoC.Get<IDocumentSet>();
            
            MainContent.OpenDocument(MainContent.NewDocumentFactory());
            
        }
        
        public object WindowMenu { get; }

        public object WindowRightMenu { get; }

        public object StatusBarContent { get; set; }

        public object LeftContent { get; }
        
        public IDocumentSet MainContent { get; }
    }
}