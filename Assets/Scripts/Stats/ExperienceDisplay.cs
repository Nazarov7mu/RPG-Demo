using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;
        private Text _experienceValue;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            _experienceValue = GetComponent<Text>();
        }

        private void Update()
        {
            _experienceValue.text = String.Format("{0:0}", _experience.GetPoints());
        }
    }
}