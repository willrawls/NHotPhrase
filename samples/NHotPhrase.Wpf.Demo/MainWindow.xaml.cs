using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using NHotPhrase.Phrase;

namespace NHotPhrase.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public HotPhraseManager Manager { get; set; }

        public MainWindow()
        {
            HotPhraseManager.Current.AddOrReplace("Increment", IncrementGesture, OnIncrement);
            HotPhraseManager.Current.AddOrReplace("Decrement", DecrementGesture, OnDecrement);
        }

        public void OnIncrement(object sender, PhraseEventArguments e)
        {
            Value++;
            e.Handled = true;
        }

        public void OnDecrement(object sender, PhraseEventArguments e)
        {
            Value--;
            e.Handled = true;
        }

        public int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand _negateCommand;
        public ICommand NegateCommand => _negateCommand ??= new DelegateCommand(Negate);

        public DelegateCommand _testCommand;
        public ICommand TestCommand => _testCommand ??= new DelegateCommand(Test);

        public string IncrementHotkey => IncrementGesture.GetDisplayStringForCulture(null);
        public string DecrementHotkey => DecrementGesture.GetDisplayStringForCulture(null);

        public bool IsHotkeyManagerEnabled
        {
            get => HotPhraseManager.Current.IsEnabled;
            set => HotPhraseManager.Current.IsEnabled = value;
        }

        public void Test()
        {
            MessageBox.Show("Test");
        }


        public void Negate()
        {
            Value = -Value;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
