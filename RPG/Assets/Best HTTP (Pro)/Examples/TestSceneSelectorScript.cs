using UnityEngine;
using System.Collections;
using BestHTTP.Caching;

public class TestSceneSelectorScript : MonoBehaviour {

    public string[] TestScenes;

	// Use this for initialization
	void Start () {
        
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, Screen.width - 10, 30), string.Format("Cache size: {0,10:N0} Entity Count: {1}", HTTPCacheService.GetCacheSize(), HTTPCacheService.GetCacheEntityCount()));

        float height = (Screen.height - 300) / TestScenes.Length;

        if (GUI.Button(new Rect(5, 40, Screen.width, height), "Clear Cache"))
            BestHTTP.Caching.HTTPCacheService.BeginClear();

        for (int i = 0; i < TestScenes.Length; ++i)
            if (GUI.Button(new Rect(3, 300 + (i * height), Screen.width - 6, height), TestScenes[i]))
                Application.LoadLevel(TestScenes[i]);
    }
}