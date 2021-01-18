using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private GameObject _targetToDestroy;

    public void DestroyTarget()
    {
        Destroy(_targetToDestroy);
    }
}
