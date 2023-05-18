using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [SerializeField] GameObject _destroyedVersion;
    [SerializeField] GameObject _nonDestroyedVersion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Breakable")
        {
            Instantiate(_destroyedVersion, _nonDestroyedVersion.transform.position, _nonDestroyedVersion.transform.rotation);
            Destroy(_nonDestroyedVersion);
            StartCoroutine(DestroyObject());
        }
    }

    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(3f);
        Destroy(_destroyedVersion);
    }
}
