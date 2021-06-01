﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

            // Spell it out long hand
            /*
            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Decrement")
                    .WhenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.D)
                    .ThenKeyPressed(Keys.Back)
                    .ThenCall(OnDecrement)
            );
            */
            // Or use the NHotkey like syntax 
            Manager.Keyboard.AddOrReplace("Decrement", new[] {Keys.CapsLock, Keys.CapsLock, Keys.D, Keys.Back}, OnDecrement);

            // Write some text
            Manager.Keyboard.AddOrReplace(
                HotPhraseKeySequence
                    .Named("Write some text")
                    .WhenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.CapsLock)
                    .ThenKeyPressed(Keys.W)
                    .ThenKeyPressed(Keys.R)
                    .ThenKeyPressed(Keys.G)
                    .ThenCall(OnWriteEmail)
            );
        }

        private void OnWriteEmail(object? sender, HotPhraseEventArgs e)
        {
            SendKeys.SendWait("{BACKSPACE}{BACKSPACE}{BACKSPACE}");
            Thread.Sleep(2);

            var textPartsToSend = TextToSend.Text.MakeReadyForSendKeys();
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend)
            {
                SendKeys.SendWait(part);
                Thread.Sleep(2);
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
