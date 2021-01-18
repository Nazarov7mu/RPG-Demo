using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,
            B,
            C,
            D
        }

        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;

        [SerializeField] private DestinationIdentifier _destinationIdentifier;

        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _fadeInTime = 2f;
        [SerializeField] private float _fadeWaitTime = .5f;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set");
                yield break;
            }


            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController =  GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            //Remove control
            playerController.enabled = false;
            
            yield return fader.FadeOut(_fadeOutTime);

            //Save current level

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            //Remove control
            PlayerController newPlayerController =  GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;



            // Load current level
            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            wrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);
            fader.FadeIn(_fadeInTime);

            
            // Restore control
            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>()
                .Warp(otherPortal._spawnPoint.position); //<<-- another way of setting player's position
            //player.transform.position = otherPortal._spawnPoint.position;
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal._destinationIdentifier != this._destinationIdentifier) continue;

                return portal;
            }

            return null;
        }
    }
}