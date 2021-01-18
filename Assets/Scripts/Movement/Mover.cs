using System.Collections.Generic;
using RPG.Core;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _maxSpeed = 6f;

        [SerializeField] private float _maxNavPathLength = 40f;

        private NavMeshAgent _navMeshAgent; // Handle for NavMeshAgent
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private Health _health;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath
                (transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > _maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                total += distance;
            }

            return total;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
            //Debug.Log("Stop");
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity; //Global coordinates
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //Need to convert to Local Velocity
            float speed = localVelocity.z;

            _animator.SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>(); //other option is to use struct
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);

            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>) state;
            //SerializableVector3 x = state as SerializableVector3; // <= Second option, will return null if smth wrong
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3) data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3) data["rotation"]).ToVector();
            _navMeshAgent.enabled = true;
        }
    }
}