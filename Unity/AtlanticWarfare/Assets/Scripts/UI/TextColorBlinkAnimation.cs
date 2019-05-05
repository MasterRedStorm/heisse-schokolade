using UnityEngine;
using TMPro;

namespace  UI
{
    public class TextColorBlinkAnimation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Vector2 _alphaBounds = new Vector2(0.1f, 1.0f);

        private Color _textColor;

        private void Reset()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            _textColor = _text.color;
            _textColor.a = _alphaBounds.x;
            _text.color = _textColor;
        }

        // Update is called once per frame
        private void Update()
        {
            _textColor.a = Mathf.PingPong(Time.time, 1) * _alphaBounds.y + _alphaBounds.x;
            _text.color = _textColor;
        }
    }
}