using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text _damageText = null;

        public void SetValue(float amount)
        {
            _damageText.text = $"{amount:0}";
        }
    }
}