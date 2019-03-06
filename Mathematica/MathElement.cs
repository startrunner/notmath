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
    public class MathElement : ObservableObject, IIdentifiable
    {
        private string _sup;
        private string _main;

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

        public Guid Id { get; } = Guid.NewGuid();

        protected bool Equals(MathElement other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MathElement)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}