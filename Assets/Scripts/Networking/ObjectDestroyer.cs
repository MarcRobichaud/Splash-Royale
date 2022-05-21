using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public float aliveTime;
    
    private float timeSarted;

    public void Awake()
    {
        timeSarted = Time.time;
    }

    public void Update()
    {
        if (Time.time > timeSarted + aliveTime)
            Destroy(gameObject);
    }
}
