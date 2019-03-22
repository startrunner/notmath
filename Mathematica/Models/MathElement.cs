using System;
using JetBrains.Annotations;
using TinyMVVM;

namespace Mathematica.Models
{
    [Serializable]
    public class MathElement : ObservableObject
    {
        private string _text;
        private MathElement _sup;
        private MathElement _main;
        private MathElement _sub;

        public string Text
        {
            get => _text;
            set
            {
                _text = value; 
                OnPropertyChanged();
            }
        }
        public MathElement Sup
        {
            get => _sup;
            set
            {
                _sup = value;
                OnPropertyChanged();
            }
        }
        public MathElement Main
        {
            get => _main;
            set
            {
                _main = value;
                OnPropertyChanged();
            }
        }
        public MathElement Sub
        {
            get => _sub;
            set
            {
                _sub = value;
                OnPropertyChanged();
            }
        }

        public MathElement():this(null){}
        public MathElement([CanBeNull] string text)
        {
            Text = text;
        }
    }
}