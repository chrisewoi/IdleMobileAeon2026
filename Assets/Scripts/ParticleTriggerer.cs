using UnityEngine;

public class ParticleTriggerer : MonoBehaviour
{
    public ParticleSystem ps;
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public void TriggerParticles()
    {
        //var em = ps.emission;
        //ps.noise.
        ps.Play();
    }
}
