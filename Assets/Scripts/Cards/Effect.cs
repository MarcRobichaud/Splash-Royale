using UnityEngine;

public struct Effect
{
    public bool IsEffectStillActive => Time.time < timeStarted + duration;
    public Stats EffectStats => stats;
    
    private float timeStarted;
    private float duration;
    private Stats stats;

    public Effect(Stats _stats, float _timeStarted, float _duration)
    {
        stats = _stats;
        timeStarted = _timeStarted;
        duration = _duration;
    }
}