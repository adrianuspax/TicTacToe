using UnityEngine;

namespace TicTacToe.Safety
{
    public class Debug : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool isActive;
        private static bool _isActive;
        /// <inheritdoc/>
        private void Awake()
        {
            _isActive = isActive;
        }
#endif

        public static void Log(object message)
        {
#if UNITY_EDITOR
            if (_isActive)
                print(message);
#else
            return;
#endif
        }
    }
}
