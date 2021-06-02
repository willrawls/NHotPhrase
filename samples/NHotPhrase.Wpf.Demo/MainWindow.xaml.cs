using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public HotPhraseManager Manager { get; set; }
        public static object SyncRoot = new();
        public static bool UiChanging;

        public MainWindow()
        {
            SetupHotPhrases();
        }

        private void SetupHotPhrases()
        {
            Manager?.Dispose();
            Manager = new HotPhraseManager();

            Manager.Keyboard.AddOrReplace(
                KeySequence 
                    .Named("Toggle hot phrase activation")
                    .WhenKeyRepeats(PKey.RControlKey, 3)
                    .ThenCall(OnTogglePhraseActivation)
            );

            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Named("Increment")
                    .WhenKeyPressed(PKey.ControlKey)
                    .ThenKeyPressed(PKey.Shift)
                    .ThenKeyPressed(PKey.Alt)
                    .ThenCall(OnIncrement)
            );

            // Use the NHotkey like syntax if you like
            Manager.Keyboard.AddOrReplace("Decrement", new[] {PKey.CapsLock, PKey.CapsLock, PKey.D, PKey.Back}, OnDecrement);

            // Or spell it out
            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Named("Write some text")
                    .WhenKeyRepeats(PKey.CapsLock, 2)   // <<< User must press the caps lock pKey twice
                    .ThenKeyPressed(PKey.W)             // <<<   then a W, a R and a G must be pressed
                    .ThenKeyPressed(PKey.R)
                    .ThenKeyPressed(PKey.G)
                    .ThenCall(OnWriteTextFromTextBox)   // <<< When that happens, this function is called
            );

            // Write some text plus any wildcards
            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Factory()                                             // <<< Name isn't necessary and defaults to a new Guid
                    .WhenKeysPressed(PKey.CapsLock, PKey.CapsLock, PKey.N) // <<< Specify the entire pKey sequence at once
                    .FollowedByWildcards(WildcardMatchType.Digits, 1)      // <<< User must press 0-9 one time and only one time to match
                    .ThenCall(OnWriteTextWithWildcards)                    // <<< That one digit passed to this function
            );

            // Here's a near equivalent in a single line call syntax except any two a-Z or 0-9 characters match after the first static 3
            Manager.Keyboard.AddOrReplace(OnWriteTextWithWildcards, 2, WildcardMatchType.AlphaNumeric, PKey.CapsLock, PKey.CapsLock, PKey.M);
        }


        private void OnWriteTextFromTextBox(object? sender, PhraseEventArguments e)
        {
            ForSendingKeys.SendBackspaces(3);

            var textPartsToSend = TextToSend.Text.MakeReadyForSendKeys();
            if (textPartsToSend.Count <= 0) return;

            SendKeysProxyForWinForms.Singleton.SendKeysAndWait(textPartsToSend, 2);
        }

        public static void OnWriteTextWithWildcards(object? sender, PhraseEventArguments e)
        {
            if (e.State.MatchResult == null)
                return;  

            // The wildcard character(s) entered by the user are stored in : e.State.MatchResult.Value
            var wildcards = e.State.MatchResult.Value;
            var wildcardsLength = wildcards?.Length ?? 0;
            if (wildcardsLength == 0) return;
            
            // Send enough backspaces to cover the extra keys typed during the match
            SendKeysHelper.SendBackspaces(1 + e.State.MatchResult.Value.Length);

            // Send some strings based on the wildcard character(s)
            $"Your wildcard is {wildcards}".SendString();
            switch (e.State.MatchResult.Value.ToUpper())
            {
                case "1":
                    "\n\n\tThis is specific to wildcard 1\n\n".SendString();
                    break;
                case "5":
                    "\n\n\tThis is specific to wildcard 5\n\n\tsomevalue@bold.one\n\n".SendString();
                    break;

                default:
                    $"\n\n\t### Other\n- This is a double character wildcard\n- You typed: {e.State.MatchResult.Value}\n- ".SendString();
                    break;
            }
        }

        private void OnTogglePhraseActivation(object sender, PhraseEventArguments e)
        {
            lock(SyncRoot)
            {
                EnableGlobalHotkeysCheckBox.Checked = !EnableGlobalHotkeysCheckBox.Checked;
            }
        }

        public void OnIncrement(object sender, PhraseEventArguments e)
        {
            _value++;
            e.Handled = true;
        }

        public void OnDecrement(object sender, PhraseEventArguments e)
        {
            _value--;
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
            get => ForSending
                HotPhraseManager.Current.IsEnabled;
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
