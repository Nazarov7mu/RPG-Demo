using System;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private Text _healthValue;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _healthValue = GetComponent<Text>();
        }

        private void Update()
        {
            if (_fighter.GetTarget() == null)
            {
                _healthValue.text = "N/A";
                return;
            }

            var health = _fighter.GetTarget();
            _healthValue.text = $"{health.GetHealthPoints():0}/{health.GetMaxHealthPoints():0}";
        }
    }
}