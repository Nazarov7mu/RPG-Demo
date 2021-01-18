using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _baseStats;
        private Text _experienceValue;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _experienceValue = GetComponent<Text>();
        }

        private void Update()
        {
            _experienceValue.text = String.Format("{0:0}", _baseStats.GetLevel());
        }
    }
}