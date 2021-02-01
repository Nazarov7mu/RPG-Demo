using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;
        [SerializeField]
        private PostProcessVolume _postProcessVolume;
        
        private Vignette _vignette;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }
        private void Start()
        {
            _postProcessVolume.profile.TryGetSettings(out _vignette);
        }
        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
            
            
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            print("Disable Contol");
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
            
            _vignette.intensity.value = 0.5f;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            print("Enable Control");
            _player.GetComponent<PlayerController>().enabled = true;
            
            _vignette.intensity.value = 0.0f;
        }
    }
}