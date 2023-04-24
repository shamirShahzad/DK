using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeUntilDestroy = 3f;
    private void Awake()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
