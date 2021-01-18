using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private LazyValue<float> _healthPoints;

        private bool _isDead;

        private Animator _animator;

        private float _initialHealthPoints;
        private BaseStats _baseStats;

        [SerializeField] private float _regenerationPercentage = 70f;
        [SerializeField] private TakeDamageEvent _takeDamage;
        [SerializeField] private UnityEvent _onDie;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _baseStats = GetComponent<BaseStats>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
            _initialHealthPoints = _healthPoints.value;
        }

        private void OnEnable()
        {
            _baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = _baseStats.GetStat(Stat.Health) * (_regenerationPercentage / 100);
            _healthPoints.value = Mathf.Max(_healthPoints.value, regenHealthPoints);
        }


        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + "   " + damage);

            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);

            if (_healthPoints.value == 0)
            {
                _onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                _takeDamage.Invoke(damage);
            }
        }
        
        public void Heal(float healthToRestore)
        {
            _healthPoints.value = Mathf.Min(_healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        public float GetHealthPoints()
        {
            return _healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return GetFraction() * 100;
        }

        public float GetFraction()
        {
            return _healthPoints.value / _initialHealthPoints;
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _animator.SetTrigger("die");

            GetComponent<ActionScheduler>().CancelCurrentAction();

            //GetComponent<CapsuleCollider>().enabled = false;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            float savedHealth = (float) state;
            _healthPoints.value = savedHealth;

            if (_healthPoints.value == 0)
            {
                Die();
            }
        }

    }
}