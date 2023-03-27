﻿using System.ComponentModel;
using System.Windows;

namespace FlowSimulator
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;

                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }));
        }
    }
}
