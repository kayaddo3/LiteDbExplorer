﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using JetBrains.Annotations;
using LiteDbExplorer.Framework;
using LiteDbExplorer.Framework.Services;
using LiteDbExplorer.Modules.DbCollection;
using LiteDbExplorer.Modules.Main;

namespace LiteDbExplorer.Modules.Database
{
    [Export(typeof(IDocumentExplorer))]
    [PartCreationPolicy (CreationPolicy.Shared)]
    public class DatabasesExplorerViewModel : Screen, IDocumentExplorer
    {
        private readonly IDatabaseInteractions _databaseInteractions;
        private IFileDropSource _view;

        [ImportingConstructor]
        public DatabasesExplorerViewModel(IDatabaseInteractions databaseInteractions)
        {
            _databaseInteractions = databaseInteractions;

            PathDefinitions = databaseInteractions.PathDefinitions;

            OpenRecentItemCommand = new RelayCommand<string>(OpenRecentItem);

            ItemDoubleClickCommand = new RelayCommand<CollectionReference>(NodeDoubleClick);
        }
        
        public Paths PathDefinitions { get; }

        public ICommand OpenRecentItemCommand { get; }

        public ICommand ItemDoubleClickCommand { get; }
        
        [UsedImplicitly]
        public void OpenDatabase()
        {
            _databaseInteractions.OpenDatabase();
        }

        [UsedImplicitly]
        public void OpenRecentItem(string path)
        {
            _databaseInteractions.OpenDatabase(path);
        }

        [UsedImplicitly]
        public void OpenDatabases(IEnumerable<string> paths)
        {
            _databaseInteractions.OpenDatabases(paths);
        }

        public DatabaseReference SelectedDatabase { get; private set; }

        public CollectionReference SelectedCollection { get; private set; }

        [UsedImplicitly]
        public void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            var value = e.NewValue as IReferenceNode;

            switch (value)
            {
                case null:
                    SelectedDatabase = null;
                    SelectedCollection = null;
                    break;
                case CollectionReference collection:
                    SelectedDatabase = collection.Database;
                    SelectedCollection = collection;
                    break;
                case DatabaseReference reference:
                {
                    SelectedDatabase = reference;
                    if (SelectedCollection != null && SelectedCollection.Database != SelectedDatabase)
                    {
                        SelectedCollection = null;
                    }

                    break;
                }
                default:
                    SelectedDatabase = null;
                    SelectedCollection = null;
                    break;
            }
        }
        
        public void NodeDoubleClick(CollectionReference value)
        {
            var documentSet = IoC.Get<IDocumentSet>();
            documentSet.OpenDocument<CollectionExplorerViewModel, CollectionReference>(value);
        }

        protected override void OnViewLoaded(object view)
        {
            _view = view as IFileDropSource;
            if (_view != null)
            {
                _view.FilesDropped = OpenDatabases;
            }
            base.OnViewLoaded(view);
        }
    }
}
