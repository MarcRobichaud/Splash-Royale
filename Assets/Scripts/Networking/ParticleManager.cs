using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleManager : NetworkBehaviour
{
    private static ParticleManager instance;

    public static ParticleManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            Init();
            instance = this;
        }
    }
    
    private Dictionary<ParticlesEffects, GameObject>
        particlesResources = new Dictionary<ParticlesEffects, GameObject>();

    public void Init()
    {
        GameObject[] gos = Resources.LoadAll<GameObject>("prefabs/Particles/");

        foreach (var go in gos)
        {
            foreach (ParticlesEffects particle in (ParticlesEffects[]) ParticlesEffects.GetValues(typeof(ParticlesEffects)))
            {
                if (go.name == particle.ToString())
                {
                    particlesResources.Add(particle, go);
                    break;
                }
            }
        }
    }
    
    public void InstantiateParticle(ParticlesEffects particlesEffects, Vector3 position, Quaternion rotation)
    {
        if (IsHost)
        {
            InstantiateParticleClientRpc(particlesEffects, position, rotation);
        }
    }
    
    [ClientRpc]
    private void InstantiateParticleClientRpc(ParticlesEffects particlesEffects, Vector3 position, Quaternion rotation)
    {
        InstantiateIt(particlesEffects, position, rotation);
    }

    private void InstantiateIt(ParticlesEffects particlesEffects, Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(particlesResources[particlesEffects], position, rotation);
    }
}

