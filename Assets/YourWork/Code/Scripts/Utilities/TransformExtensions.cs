using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TransformExtensions
{
    public static Bounds GetMaxBounds(this Transform t, bool includeInactive = false)
    {
        var renderers = t.GetComponentsInChildren<Renderer>(includeInactive: includeInactive);
        var bounds = new Bounds(t.position, Vector3.zero);

        foreach (var r in renderers)
        {
            if(r.GetComponent<TextMeshPro>()) continue;
            if(r.GetComponent<ParticleSystem>()) continue;
            bounds.Encapsulate(r.bounds);
        }

        return bounds;
    }
}
