using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime;

    private void Start()
    {
        StartCoroutine(DestroyAsync(_destroyTime));
    }

    private IEnumerator DestroyAsync(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
