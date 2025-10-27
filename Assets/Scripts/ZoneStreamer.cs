using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[System.Serializable]
public class ZoneEntry
{
    public string zoneName = "Zone";
    public AssetReference sceneReference;
    public Transform zoneCenterTransform; 
    public Vector3 centerPosition = Vector3.zero; 
    public float enterRadius = 5f; 
    public float exitRadius = 7f; 


    // runtime
    [System.NonSerialized] public ZoneState state = ZoneState.Unloaded;
    [System.NonSerialized] public AsyncOperationHandle<SceneInstance> handle;
}


public enum ZoneState { Unloaded, Loading, Loaded, Unloading }


public class ZoneStreamer : MonoBehaviour
{
    public List<ZoneEntry> zones = new List<ZoneEntry>();
    public Transform controllerTransform;
    public LoadingIndicator loadingIndicator;
    [Tooltip("Simulate slow loads for demo (adds a WaitForSeconds before completing load)")]
    public bool simulateSlowLoad = false;
    [Tooltip("seconds to delay when simulateSlowLoad is on")]
    public float simulatedDelay = 2f;



    void Start()
    {
        controllerTransform = FindObjectOfType<PuzzleController>().Marker.transform;
        foreach (var z in zones) z.state = ZoneState.Unloaded;
    }


    void Update()
    {
        if (controllerTransform == null) return;
            Vector3 pos = controllerTransform.position;
        for (int i = 0; i < zones.Count; i++)
        {
            var z = zones[i];
            Vector3 center = (z.zoneCenterTransform != null) ? z.zoneCenterTransform.position : z.centerPosition;
            float d = Vector3.Distance(pos, center);


            if (z.state == ZoneState.Unloaded || z.state == ZoneState.Unloading)
            {
                if (d <= z.enterRadius)
                {
                    TryLoadZone(z);
                }
            }
            if (z.state == ZoneState.Loaded || z.state == ZoneState.Loading)
            {
                if (d > z.exitRadius)
                {
                    TryUnloadZone(z);
                }
            }
        }
    }
    void TryLoadZone(ZoneEntry z)
    {
        if (z.state == ZoneState.Loaded || z.state == ZoneState.Loading) return;
        if (z.sceneReference == null)
        {
            Debug.LogWarning($"ZoneStreamer: SceneReference missing for zone '{z.zoneName}'");
            return;
        }
        Debug.Log($"[ZoneStreamer] Start loading {z.zoneName}");
        z.state = ZoneState.Loading;
        if (loadingIndicator) loadingIndicator.NotifyLoadStarted(z.zoneName);
        StartCoroutine(LoadRoutine(z));
    }


    IEnumerator LoadRoutine(ZoneEntry z)
    {
        var handle = z.sceneReference.LoadSceneAsync(UnityEngine.SceneManagement.LoadSceneMode.Additive, true);
        z.handle = handle;
        float startTime = Time.time;
        while (!handle.IsDone)
        {
            yield return null;
        }
        if (simulateSlowLoad) yield return new WaitForSeconds(simulatedDelay- Time.time + startTime);


        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            z.state = ZoneState.Loaded;
            Debug.Log($"[ZoneStreamer] Finished loading {z.zoneName}");
        }
        else
        {
            z.state = ZoneState.Unloaded;
            Debug.LogError($"[ZoneStreamer] Failed to load {z.zoneName}: {handle.OperationException}");
        }
        if (loadingIndicator) loadingIndicator.NotifyLoadFinished(z.zoneName);
    }
    void TryUnloadZone(ZoneEntry z)
    {
        if (z.state == ZoneState.Unloaded || z.state == ZoneState.Unloading) return;
        if (z.state == ZoneState.Loading)
        {
            Debug.Log($"[ZoneStreamer] Requested unload for {z.zoneName} while loading; ignoring until load completes.");
            return;
        }
        Debug.Log($"[ZoneStreamer] Start unloading {z.zoneName}");
        z.state = ZoneState.Unloading;
        if (loadingIndicator) loadingIndicator.NotifyUnloadStarted(z.zoneName);
        StartCoroutine(UnloadRoutine(z));
    }


    IEnumerator UnloadRoutine(ZoneEntry z)
    {
        if (z.handle.IsValid())
        {
            var h = Addressables.UnloadSceneAsync(z.handle, true);
            while (!h.IsDone)
            {
                Debug.Log($"[ZoneStreamer] waiting");
                yield return null;
            }
            z.state = ZoneState.Unloaded;
             Debug.Log($"[ZoneStreamer] Finished unloading {z.zoneName}");
            if (loadingIndicator) loadingIndicator.NotifyUnloadFinished(z.zoneName);
        }
        else
        {
            Debug.LogWarning($"[ZoneStreamer] No valid handle to unload for {z.zoneName}");
            z.state = ZoneState.Unloaded;
            if (loadingIndicator) loadingIndicator.NotifyUnloadFinished(z.zoneName);
        }
    }
}