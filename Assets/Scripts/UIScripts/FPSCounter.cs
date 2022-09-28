using System.Globalization;

using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class FPSCounter : MonoBehaviour
    {
        public TextMeshProUGUI fpsText;
    
        private float _deltaTime;

        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString(CultureInfo.CurrentCulture);
        }
    }
}
