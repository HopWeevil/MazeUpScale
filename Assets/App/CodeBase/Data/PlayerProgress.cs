using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public event Action KeyCollected;
        public int KeysCollected {  get; private set; }
        public int KeysToCollect { get; private set; }

        public void CollectKey()
        {
            KeysCollected++;
            KeyCollected?.Invoke();
        }

        public PlayerProgress(int keysToCollect)
        {
            KeysToCollect = keysToCollect;
        }
    }
}