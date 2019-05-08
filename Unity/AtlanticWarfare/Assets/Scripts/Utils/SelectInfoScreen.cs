using UnityEngine;

namespace Utils
{
    public class SelectInfoScreen : MonoBehaviour
    {
        private const string WinningState = "WinningState";
    
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _looseScreen;
    
        private void OnEnable()
        {
            if (!PlayerPrefs.HasKey(WinningState) || PlayerPrefs.GetInt(WinningState) == 0)
            {
                _looseScreen.SetActive(true);
                return;
            }

            _winScreen.SetActive(true);
            PlayerPrefs.SetInt(WinningState, 0);
           
        }
    }
}