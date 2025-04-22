using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    void Update()
    {
        Destroy(gameObject, ParticleSystem.main.duration);
    }
}
