using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _isTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && !_isTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                _isTriggered = true;
            }
        }
    }
}