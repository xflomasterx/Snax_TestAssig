using UnityEngine;

[ExecuteAlways]
public class HubGizmos : MonoBehaviour
{
    [Tooltip("Reference to the ZoneStreamer in the scene. If null, the script will try to find one automatically.")]
    public ZoneStreamer streamer; 

    [Header("Gizmos Options")]
    public bool drawEnter = true;
    public bool drawExit = true;
    public Color enterColor = Color.green;
    public Color exitColor = Color.red;
    public bool drawLabels = true;

    void OnValidate()
    {
        if (streamer == null)
            streamer = FindObjectOfType<ZoneStreamer>();
    }

    void OnDrawGizmos()
    {
        if (streamer == null || streamer.zones == null)
            return;

        foreach (var z in streamer.zones)
        {
            if (z == null)
                continue;

            Vector3 center = (z.zoneCenterTransform != null)
                ? z.zoneCenterTransform.position
                : z.centerPosition;

            if (drawEnter)
            {
                Gizmos.color = enterColor;
                Gizmos.DrawWireSphere(center, Mathf.Max(0f, z.enterRadius));
            }

            if (drawExit)
            {
                Gizmos.color = exitColor;
                Gizmos.DrawWireSphere(center, Mathf.Max(0f, z.exitRadius));
            }

#if UNITY_EDITOR
            if (drawLabels)
            {
                UnityEditor.Handles.Label(
                    center + Vector3.up * 0.15f,
                    $"{z.zoneName}\nEnter: {z.enterRadius}  Exit: {z.exitRadius}  (State: {z.state})"
                );
            }
#endif
        }
    }
}