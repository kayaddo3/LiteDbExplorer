﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LiteDB;

namespace LiteDbExplorer
{
    public enum DocumentTypeFilter
    {
        All = -1,
        BsonDocument = 0,
        File = 1
    }

    public enum DocumentType
    {
        BsonDocument = 0,
        File = 1
    }

    public class TypedDocumentReference
    {
        public TypedDocumentReference(DocumentType type, DocumentReference documentReference)
        {
            Type = type;
            DocumentReference = documentReference;
        }

        public DocumentType Type { get; }
        public DocumentReference DocumentReference { get; }
    }

    public class DocumentReference : INotifyPropertyChanging, INotifyPropertyChanged, IReferenceNode
    {
        private BsonDocument _liteDocument;
        private CollectionReference _collection;

        public DocumentReference()
        {
            InstanceId = Guid.NewGuid().ToString();
        }

        public DocumentReference(BsonDocument document, CollectionReference collection) : this()
        {
            LiteDocument = document;
            Collection = collection;
        }

        public string InstanceId { get; }

        public BsonDocument LiteDocument
        {
            get => _liteDocument;
            set
            {
                OnPropertyChanging();
                _liteDocument = value;
                OnPropertyChanged();
            }
        }

        public CollectionReference Collection
        {
            get => _collection;
            set
            {
                OnPropertyChanging();
                _collection = value;
                OnPropertyChanged();
            }
        }

        public bool BelongsToCollectionInstance(CollectionReference collectionReference)
        {
            if (Collection == null)
            {
                return false;
            }

            return Collection.InstanceId.Equals(collectionReference?.InstanceId);
        }

        public void RemoveSelf()
        {
            Collection?.RemoveItem(this);
        }

        [UsedImplicitly]
        public event PropertyChangedEventHandler PropertyChanged;

        public void InvalidateProperties()
        {
            OnPropertyChanged(string.Empty);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;
        
        protected virtual void OnPropertyChanging([CallerMemberName] string name = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }
    }
}