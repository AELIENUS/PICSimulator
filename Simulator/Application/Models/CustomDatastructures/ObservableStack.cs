﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Application.Annotations;
using Application.Constants;

namespace Application.Models.CustomDatastructures
{
    /// <summary>
    /// To update the ObservableCollection nested in this class and the controls, we need to get access to the thread that handles
    /// the view and the controls, which is the main application thread and update the collection from there. This is done by
    /// accessing the threads dispatcher.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableStack<T> : Stack<T>, INotifyPropertyChanged
    {
        #region fields 
        private ObservableCollection<T> _collection = new ObservableCollection<T>();
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<T> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
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

        public new void Clear()
        {
            this.Collection = new ObservableCollection<T>();
        }

        public new T Pop()
        {
            //Item holen
            var item = Collection[Collection.Count-1];
            //Item löschen
            var a = System.Windows.Application.Current;
            a.Dispatcher.Invoke(
                DispatcherPriority.Background, new Action(() =>
                {
                    this.Collection.RemoveAt(Collection.Count - 1);
                }));
            return item;
        }

        public new void Push(T item)
        {
            System.Windows.Application a = System.Windows.Application.Current;
            if (Collection.Count>= MemoryConstants.PC_STACK_CAPACITY)
            {
                a.Dispatcher.Invoke(
                    DispatcherPriority.Background, new Action(() =>
                        {
                            Collection.RemoveAt(0);
                        }));
            }
            a.Dispatcher.Invoke(
                DispatcherPriority.Background, new Action(() =>
                {
                    this.Collection.Add(item);
                }));
        }

        public new T Peek()
        {
            var item = Collection[Collection.Count-1];
            return item;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            propertyChanged?.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
