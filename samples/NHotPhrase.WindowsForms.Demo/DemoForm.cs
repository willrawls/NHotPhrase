using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Demo
{
    public partial class DemoForm : Form
    {
        public int _value;

        public HotPhraseManager Manager { get; set; }
        public static object SyncRoot = new();
        public static bool UiChanging;

        public delegate void CheckedChangedDelegate(object sender, System.EventArgs e);

        public DemoForm()
        {
            InitializeComponent();
            SetupHotPhrases();
        }

        private void SetupHotPhrases()
        {
            Manager?.Dispose();
            Manager = new HotPhraseManager();

            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Named("Toggle hot phrase activation")
                    .WhenKeyRepeats(Keys.RControlKey, 3)
                    .ThenCall(OnTogglePhraseActivation)
            );

            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Named("Increment")
                    .WhenKeyPressed(Keys.ControlKey)
                    .ThenKeyPressed(Keys.Shift)
                    .ThenKeyPressed(Keys.Alt)
                    .ThenCall(OnIncrement)
            );

            // Use the NHotkey like syntax if you like
            Manager.Keyboard.AddOrReplace("Decrement", new[] {Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back}, OnDecrement);

            // Or spell it out
            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Named("Write some text")
                    .WhenKeyRepeats(Keys.CapsLock, 2)   // <<< User must press the caps lock key twice
                    .ThenKeyPressed(Keys.W)             // <<<   then a W, a R and a G must be pressed
                    .ThenKeyPressed(Keys.R)
                    .ThenKeyPressed(Keys.G)
                    .ThenCall(OnWriteTextFromTextBox)   // <<< When that happens, this function is called
            );

            // Write some text plus any wildcards
            Manager.Keyboard.AddOrReplace(
                KeySequence
                    .Factory()                                             // <<< Name isn't necessary and defaults to a new Guid
                    .WhenKeysPressed(Keys.CapsLock, Keys.CapsLock, Keys.N) // <<< Specify the entire key sequence at once
                    .FollowedByWildcards(WildcardMatchType.Digits, 1)      // <<< User must press 0-9 one time and only one time to match
                    .ThenCall(OnWriteTextWithWildcards)                    // <<< That one digit passed to this function
            );

            // Here's a near equivalent in a single line call syntax except any two a-Z or 0-9 characters match after the first static 3
            Manager.Keyboard.AddOrReplace(OnWriteTextWithWildcards, 2, WildcardMatchType.AlphaNumeric, Keys.CapsLock, Keys.CapsLock, Keys.M);
        }

        private void OnWriteTextFromTextBox(object? sender, PhraseEventArguments e)
        {
            SendKeyHelpers.SendBackspaces(3);

            var textPartsToSend = TextToSend.Text.MakeReadyForSendKeys();
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend)
            {
                SendKeys.SendWait(part);
                Thread.Sleep(2);
            }
        }

        public static void OnWriteTextWithWildcards(object? sender, PhraseEventArguments e)
        {
            if (e.State.MatchResult == null)
                return;

            var wildcards = e.State.MatchResult.Value;
            var wildcardsLength = wildcards?.Length ?? 0;
            if (wildcardsLength == 0) return;
            
            SendKeyHelpers.SendBackspaces(1 + e.State.MatchResult.Value.Length);
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
                    $"\n\n\t### Other\n- This is specific to any other wildcard\n- {e.State.MatchResult.Value}\n- ".SendString();
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
            Value++;
            e.Handled = true;
        }

        public void OnDecrement(object sender, PhraseEventArguments e)
        {
            Value--;
            e.Handled = true;
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                lblValue.Text = _value.ToString();
            }
        }

        public void EnableGlobalHotkeysCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new CheckedChangedDelegate(EnableGlobalHotkeysCheckBox_CheckedChanged), null, null);
                return;
            }

            UpdateGlobalThingy(EnableGlobalHotkeysCheckBox.Checked);
        }

        private void UpdateGlobalThingy(bool enableThingy) // Safely setup/tear down hot phrases
        {
            if (UiChanging || !Monitor.TryEnter(SyncRoot)) return;

            UiChanging = true;
            try
            {
                if (enableThingy)
                {
                    SetupHotPhrases();
                }
                else
                {
                    Manager?.Dispose();
                    Manager = null;
                }
            }
            finally
            {
                UiChanging = false;
                Monitor.Exit(SyncRoot);
            }
        }
    }
}
