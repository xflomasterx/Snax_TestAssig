using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class LoadingIndicator : MonoBehaviour
{
    public float delay;
    float plannedTime = float.PositiveInfinity;
    Text uiText; 
    HashSet<string> activeLoads = new HashSet<string>();
    string recentlyLoaded = string.Empty;

    void Start()
    {
        UpdateText();
        uiText = this.GetComponent<Text>();
    }


    public void NotifyLoadStarted(string zoneName)
    {
        activeLoads.Add(zoneName);
        Debug.Log($"[LoadingIndicator] Load started: {zoneName}");
        uiText.enabled = true;
        plannedTime = float.PositiveInfinity;
        UpdateText();
    }
    public void NotifyLoadFinished(string zoneName)
    {
        activeLoads.Remove(zoneName);
        recentlyLoaded = zoneName;
        Debug.Log($"[LoadingIndicator] Load finished: {zoneName}");
        UpdateText();
        plannedTime = Time.time + delay;
    }
    public void NotifyUnloadStarted(string zoneName)
    {
        Debug.Log($"[LoadingIndicator] Unload started: {zoneName}");
    }
    public void NotifyUnloadFinished(string zoneName)
    {
        Debug.Log($"[LoadingIndicator] Unload finished: {zoneName}");
        UpdateText();
    }


    void UpdateText()
    {
        string text = "";
        if (activeLoads.Count > 0)
            text = "Loading: " + string.Join(", ", activeLoads);
        else if(!string.IsNullOrEmpty(recentlyLoaded))
            text = "Finished loading: " + recentlyLoaded;
        if (uiText != null) uiText.text = text;
    }


    void Update()
    {
        if (Time.time >= plannedTime)
            uiText.enabled = false;
    }

}