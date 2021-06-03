using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Demo
{
    public partial class DemoForm : Form
    {

        public HotPhraseManager Manager { get; set; }
        public static readonly object SyncRoot = new();
        public static bool UiChanging { get; set; }

        public delegate void CheckedChangedDelegate(object sender, System.EventArgs e);

        public DemoForm()
        {
            InitializeComponent();
            SetupHotPhrases();
        }

        private void SetupHotPhrases()
        {
            Manager?.Dispose();
            Manager = new HotPhraseManagerForWinForms();

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
            Manager.Keyboard.AddOrReplace("Decrement", new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.D, PKey.Back}, OnDecrement);

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
                    .WhenKeysPressed(new List<PKey> { PKey.CapsLock, PKey.CapsLock, PKey.N }) // <<< Specify the entire pKey sequence at once
                    .FollowedByWildcards(WildcardMatchType.Digits, 1)      // <<< User must press 0-9 one time and only one time to match
                    .ThenCall(OnWriteTextWithWildcards)                    // <<< That one digit passed to this function
            );

            // Here's a near equivalent in a single line call syntax except any two a-Z or 0-9 characters match after the first static 3
            Manager.Keyboard.AddOrReplace(OnWriteTextWithWildcards, 2, WildcardMatchType.AlphaNumeric, new List<PKey> {PKey.CapsLock, PKey.CapsLock, PKey.M });
        }

        private void OnWriteTextFromTextBox(object sender, PhraseEventArguments e)
        {
            SendPKeys.SendBackspaces(3);

            var textPartsToSend = TextToSend.Text.MakeReadyForSending();
            if (textPartsToSend.Count <= 0) return;

            SendPKeys.Singleton.SendKeysAndWait(textPartsToSend, 2);
        }

        public static void OnWriteTextWithWildcards(object sender, PhraseEventArguments e)
        {
            if (e.State.MatchResult == null)
                return;  

            // The wildcard character(s) entered by the user are stored in : e.State.MatchResult.Value
            var wildcards = e.State.MatchResult.Value;
            var wildcardsLength = wildcards?.Length ?? 0;
            if (wildcardsLength == 0) return;
            
            // Send enough backspaces to cover the extra keys typed during the match
            SendPKeys.SendBackspaces(1 + e.State.MatchResult.Value.Length);

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
