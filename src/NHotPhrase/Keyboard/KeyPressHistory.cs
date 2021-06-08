using System;
using System.Collections.Generic;

namespace NHotPhrase.Keyboard
{
    public class KeyHistory : List<PKey>
    {
        public DateTime LastPressAt { get; set; } = DateTime.MinValue;
        public int MaxHistoryLength { get; set; } = 8;
        public int ClearAfterThisManySeconds { get; set; } = 5;

        public static readonly object SyncRoot = new();

        public KeyHistory()
        {
        }

        public KeyHistory(int maxHistoryLength, int clearAfterThisManySeconds, DateTime lastPressAt, List<PKey> history)
        {
            MaxHistoryLength = maxHistoryLength;
            ClearAfterThisManySeconds = clearAfterThisManySeconds;
            LastPressAt = lastPressAt;
            AddRange(history);
        }

        public new void Add(PKey pKey)
        {
            AddKeyPress(pKey);
        }
        public KeyHistory AddKeyPress(PKey pKey)
        {
            // If too much time has gone by, clear the queue
            if (Count > 0 && DateTime.Now.Subtract(LastPressAt).Seconds > ClearAfterThisManySeconds)
            {
                Clear();
            }

            // If the history is too long, truncate it keeping the newest entries
            while (Count > MaxHistoryLength)
            {
                RemoveAt(0);
            }

            LastPressAt = DateTime.Now;
            base.Add(pKey);
            return this;
        }

        public List<PKey> KeyList()
        {
            lock (SyncRoot)
            {
                return new List<PKey>(this);
            }
        }
    }
}