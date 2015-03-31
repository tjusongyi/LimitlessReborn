using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class LoadImageTestScript : MonoBehaviour {

    /// <summary>
    /// Default url that points to an image somewhere on the internet.
    /// </summary>
    public string ImageUrl = "http://1.bp.blogspot.com/_EmX0HOnldCg/TODtaqLAHeI/AAAAAAAABnM/fLPKmMD66ng/s1600/funnycat.jpg";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);
    }

    // Called by Unity3D
    void OnGUI()
    {
        // Render a texfield so we can change the texture to an other image by copy-pasting a url.
        ImageUrl = GUI.TextField(new Rect(10, 10, Screen.width - 20, 40), ImageUrl);

        // Clicking on the button, we will send out a http request, the OnImageDownloaded function will be called when
        //  the download finished. It's a non-blocking call, 
        if (GUI.Button(new Rect(10, 60, 150, 30), "Download Image"))
        {
            // Send the request to the server.
            BestHTTP.HTTPManager.SendRequest(ImageUrl, OnImageDownloaded);

            Debug.Log("Download started!");
        }

        // Go back to the demo selector
        if (GUI.Button(new Rect(20 + Screen.width / 2, Screen.height - 200, -30 + Screen.width / 2, 195), "Back"))
            Application.LoadLevel(0);
    }

    /// <summary>
    /// Called when the response downloaded, or if an error occured.
    /// </summary>
    /// <param name="request">The original request that automatically generated in the SendRequest call.</param>
    /// <param name="response">The response object that holds everything the server sent to us. Or null, if an error occured.</param>
    private void OnImageDownloaded(BestHTTP.HTTPRequest request, BestHTTP.HTTPResponse response)
    {
        if (response != null)
        {
            Debug.Log("Download finished!");

            // Set the texture to the newly downloaded one
            this.GetComponent<GUITexture>().texture = response.DataAsTexture2D;
        }
        else
            Debug.LogError("No response received: " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception"));
    }
}