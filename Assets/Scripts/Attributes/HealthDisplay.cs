using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _healthValue;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
            _healthValue = GetComponent<Text>();
        }

        private void Update()
        {
            _healthValue.text = $"{_health.GetHealthPoints():0}/{_health.GetMaxHealthPoints():0}";
        }
    }
}