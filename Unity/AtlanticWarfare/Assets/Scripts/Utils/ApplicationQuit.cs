using UnityEngine;

namespace Utils
{
    public class ApplicationQuit : MonoBehaviour
    {
        public void CloseApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}