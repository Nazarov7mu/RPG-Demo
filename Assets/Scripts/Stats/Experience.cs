using RPG.Saving;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate();

        public event Action onExperienceGained;
        
        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            float savedXP = (float) state;
            _experiencePoints = savedXP;
        }

        public float GetPoints()
        {
            return _experiencePoints;
        }
    }
}