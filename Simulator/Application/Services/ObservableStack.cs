﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Application.Services
{
    public class ObservableStack<T> : ObservableObject
    {
        #region fields 
        private ObservableCollection<T> _Collection = new ObservableCollection<T>();

        public ObservableCollection<T> Collection
        {
            get
            {
                return _Collection;
            }
            set
            {
                _Collection = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public ObservableStack()
        {

        }

        public ObservableStack(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                this.Collection.Add(item);
            }
        }

        public ObservableStack(List<T> list)
        {
            foreach (var item in list)
            {
                this.Collection.Add(item);
            }
        }

        public void Clear()
        {
            this.Collection = new ObservableCollection<T>();
        }

        public T Pop()
        {
            //Item holen
            var item = Collection[Collection.Count-1];
            //Item löschen
            System.Windows.Application a = System.Windows.Application.Current;
            a.Dispatcher.Invoke(
                DispatcherPriority.Background, new Action(() =>
                {
                    this.Collection.RemoveAt(Collection.Count - 1);
                }));
            return item;
        }

        public void Push(T item)
        {
            System.Windows.Application a = System.Windows.Application.Current;
            a.Dispatcher.Invoke(
                DispatcherPriority.Background, new Action(() =>
                {
                    this.Collection.Add(item);
                }));
        }

        public T Peek()
        {
            var item = Collection[Collection.Count-1];
            return item;
        }
    }
}