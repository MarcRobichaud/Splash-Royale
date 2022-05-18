using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectApplier
{
    private const float TimeBetweenEffects = 0.5f;
    
    private float timeLastEffectStarted;
    private List<Effect> activeEffects = new List<Effect>();
    private bool IsApplyEffectReady => Time.time > timeLastEffectStarted + TimeBetweenEffects;

    public void AddEffect(Effect effect)
    {
        activeEffects.Add(effect);
    }

    public void TryApplyEffect(Unit unit)
    {
        if (activeEffects.Count <= 0 || !IsApplyEffectReady) return;

        timeLastEffectStarted = Time.time;

        for (int i = activeEffects.Count - 1; i >= 0 && i < activeEffects.Count; i--)
        {
            if (!activeEffects[i].IsEffectStillActive)
            {
                activeEffects.Remove(activeEffects[i]);
            }
            else
            {
                unit.OnHit(activeEffects[i].EffectStats);
            }
        }
    }

    public void Flush()
    {
        activeEffects.Clear();
        timeLastEffectStarted = 0;
    }
}
