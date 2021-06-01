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
            Manager = new HotPhraseManager(this);

            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Toggle phrase activation")
                    .WhenKeyRepeats(Keys.RControlKey, 3)
                    .ThenCall(OnTogglePhraseActivation)
            );

            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
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
                HotPhraseKeySequence
                    .Named("Write some text")
                    .WhenKeyRepeats(Keys.CapsLock, 2)   // <<< User must press the caps lock key twice
                    .ThenKeyPressed(Keys.W)             // <<<   then a W, a R and a G must be pressed
                    .ThenKeyPressed(Keys.R)
                    .ThenKeyPressed(Keys.G)
                    .ThenCall(OnWriteTextFromTextBox)   // <<< When that happens, this function is called
            );

            // Write some text plus any wildcards
            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Write some text and wildcards")
                    .WhenKeysPressed(Keys.CapsLock, Keys.CapsLock, Keys.N) // <<< Specify the entire key sequence at once
                    .FollowedByWildcards(WildcardMatchType.Digits, 1)      // <<< User must hit 1 and only 1 digit key to match
                    .ThenCall(OnWriteTextWithWildcards)                    // <<< That one digit passed to this function
            );

            // Here's the equivalent in a single line call syntax
            Manager.Keyboard.AddOrReplace(OnWriteTextWithWildcards, 1, WildcardMatchType.Digits, Keys.CapsLock, Keys.CapsLock, Keys.N);
        }

        private void OnWriteTextFromTextBox(object? sender, HotPhraseEventArgs e)
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

        public static void OnWriteTextWithWildcards(object? sender, HotPhraseEventArgs e)
        {
            if (e.State.MatchResult == null)
                return;

            var wildcards = e.State.MatchResult.Value;
            var wildcardsLength = wildcards?.Length ?? 0;
            if (wildcardsLength == 0) return;
            
            SendKeyHelpers.SendBackspaces(2);
            $"Your wildcard is {wildcards}".SendString();
            switch (e.State.MatchResult.ValueAsInt())
            {
                case 1:
                    "\n\n\tThis is specific to wildcard 1\n\n".SendString();
                    break;
                case 5:
                    "\n\n\tThis is specific to wildcard 5\n\n\tsomevalue@bold.one\n\n".SendString();
                    break;
            }
        }

        private void OnTogglePhraseActivation(object sender, HotPhraseEventArgs e)
        {
            lock(SyncRoot)
            {
                EnableGlobalHotkeysCheckBox.Checked = !EnableGlobalHotkeysCheckBox.Checked;
            }
        }

        public void OnIncrement(object sender, HotPhraseEventArgs e)
        {
            Value++;
            e.Handled = true;
        }

        public void OnDecrement(object sender, HotPhraseEventArgs e)
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

        private void UpdateGlobalThingy(bool enableThingy)
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
