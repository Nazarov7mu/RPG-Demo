using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _healthComponent;
        [SerializeField] private RectTransform _foreground;
        [SerializeField] private Canvas _rootCanvas;

        private void Update()
        {
            var healthValue = _healthComponent.GetFraction();
            if (Mathf.Approximately(healthValue, 1)
                || Mathf.Approximately(healthValue, 0))
            {
                _rootCanvas.enabled = false;
                return;
            }

            _rootCanvas.enabled = true;
            _foreground.localScale = new Vector3(healthValue, 1, 1);
        }
    }
}