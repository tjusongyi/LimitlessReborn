using UnityEngine;
using System.Collections;
using BestHTTP;
using System;

public class test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        var request = new HTTPRequest(new Uri("http://127.0.0.1:8000/"), OnRequestFinished);

        // We will track the download progress
        //request.OnProgress = OnDownloadProgress;

        // if UseStreaming true then, the given callback will called as soon as possible if at least one 
        //  fragment downloaded
        //request.UseStreaming = true;

        //// how big a fragment is. 
        //request.StreamFragmentSize = 1024;

        // start proces the request
        HTTPManager.SendRequest(request);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        Debug.Log("Request Finished! Text received: " + response.DataAsText);
    }
}
