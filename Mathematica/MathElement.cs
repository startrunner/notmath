using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;
using TinyMVVM;

namespace Mathematica
{
    [Serializable]
    public class MathElement : ObservableObject
    {
        private string _sup;
        private string _main;
        private string _sub;

        public string Sup
        {
            get => _sup;
            set
            {
                _sup = value;
                OnPropertyChanged();
            }
        }

        public string Main
        {
            get => _main;
            set
            {
                _main = value;
                OnPropertyChanged();
            }
        }

        public string Sub
        {
            get => _sub;
            set
            {
                _sub = value;
                OnPropertyChanged();
            }
        }
    }

    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}