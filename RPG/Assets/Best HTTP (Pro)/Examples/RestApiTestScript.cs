using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BestHTTP;

using LitJson;

/// <summary>
/// Class to hold the found users after json deserialization
/// </summary>
public class GithubUserSearchResult
{
    public List<GithubUser> users;
}

/// <summary>
/// Class to hold some of the infos that github will send back to us.
/// </summary>
public class GithubUser
{
    // These will be filled from the search result.
    public string username;
    public string fullname;
    public string location;

    // This will be filled with an additional query to the user's info.
    public string avatar_url;
    private bool getAvatarUrlStarted;

    // This will set, when the avatar_url arrived with the userinfo and the the picture on that url downloaded.
    public Texture2D texture;
    public bool getTextureStarted;

    public void StartGetAvatarUrl()
    {
        if (getAvatarUrlStarted || !string.IsNullOrEmpty(avatar_url))
            return;

        HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(RestApiTestScript.Api_GetUser, username)), OnUserInfoFinished);
        request.UseAlternateSSL = true;
        request.Send();
    }

    private void OnUserInfoFinished(HTTPRequest request, 
                                    HTTPResponse response)
    {
        getAvatarUrlStarted = false;

        if (response != null)
        {
            JsonReader reader = new JsonReader(response.DataAsText);
            reader.SkipNonMembers = true;
            avatar_url = JsonMapper.ToObject<GithubUser>(reader)
                .avatar_url;

            StartDownloadTexture();
        }
        else if (request.Exception != null)
            Debug.LogError(string.Format("{0}: {1}", request.Exception.Message, request.Exception.StackTrace));
    }

    public void StartDownloadTexture()
    {
        if (getTextureStarted || texture != null)
            return;
        HTTPManager.SendRequest(avatar_url, OnUserAvatarDownloaded);
    }


    private void OnUserAvatarDownloaded(HTTPRequest request, HTTPResponse response)
    {
        getTextureStarted = false;

        if (response != null)
            texture = response.DataAsTexture2D;
        else if (request.Exception != null)
            Debug.LogError(string.Format("{0}: {1}", request.Exception.Message, request.Exception.StackTrace));
    }

}

/// <summary>
/// Error message sent to us from github.
/// </summary>
public sealed class GithubMessage
{
    public string message;
}

/// <summary>
/// This sample uses the Github public REST Api to demonstrate how easy to use the BestHTTP library to access resful services.
/// </summary>
public class RestApiTestScript : MonoBehaviour {

    public const string Api_Base = "https://api.github.com/";
    public const string Api_SearchUser = Api_Base + "legacy/user/search/{0}";
    public const string Api_GetUser = Api_Base + "users/{0}";

    string searchFor = "king";

    GithubUserSearchResult searchResult;
    bool searchStarted;
    string searchText;

    GithubUser selectedUser;

    Vector2 scrollPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, Screen.height / 10), "Search for user:");
        searchFor = GUI.TextField(new Rect(110, 3, Screen.width - 215, Screen.height / 10), searchFor);

        if (!searchStarted && GUI.Button(new Rect(115 + Screen.width - 215, 3, Screen.width - (115 + Screen.width - 215) - 5, Screen.height / 10), "Search"))
        {
            searchResult = null;
            selectedUser = null;

            // Send search request ti Github
            HTTPRequest request = new HTTPRequest(new System.Uri(string.Format(Api_SearchUser, searchFor)), OnSearchForUsersFinished);
            request.UseAlternateSSL = true;
            request.Send();

            searchStarted = true;
            searchText = "Searching";
        }

        GUI.Box(new Rect(10, 5 + Screen.height / 10, Screen.width / 2, Screen.height - 35), string.Empty);
        if (!string.IsNullOrEmpty(searchText))
            GUI.Label(new Rect(10, 5 + Screen.height / 10, Screen.width / 2, Screen.height - 35), searchText);

        if (searchResult != null && searchResult.users != null)
        {
            GUI.Label(new Rect(), "Users found:");
            scrollPosition = GUI.BeginScrollView(new Rect(10, 5 + Screen.height / 10, Screen.width / 2, Screen.height - 35), scrollPosition, new Rect(10, 30, Screen.width / 2, searchResult.users.Count * 30), false, true);

            for (int i = 0; i < searchResult.users.Count; ++i)
                if (GUI.Button(new Rect(10, i * 30, (Screen.width / 2) - 20, 25), searchResult.users[i].fullname))
                {
                    // Button pressed: change the selected user, and start download the avartar_url
                    selectedUser = searchResult.users[i];
                    if (selectedUser.texture == null)
                        selectedUser.StartGetAvatarUrl();
                }

            GUI.EndScrollView();

            if (selectedUser != null)
            {
                float userinfoLeft = (Screen.width / 2) + 20;
                float userinfoWidth = (Screen.width / 2) - 35;

                GUI.Box(new Rect(userinfoLeft, 5 + Screen.height / 10, userinfoWidth, Screen.height - (Screen.height / 10) - 205), string.Empty);
                GUI.Label(new Rect(userinfoLeft + 5, 8 + Screen.height / 10, userinfoWidth, 25), selectedUser.fullname + "'s GitHub Profile");

                // Draw user's profile picture if it's downloaded
                if (selectedUser.texture != null)
                    GUI.DrawTexture(new Rect(userinfoLeft + 3, 45 + Screen.height / 10, 128, 128), selectedUser.texture, ScaleMode.ScaleToFit);
                else
                    GUI.Box(new Rect(userinfoLeft + 3, 45 + Screen.height / 10, 128, 128), "Loading...");

                GUI.Label(new Rect(userinfoLeft + 3, 180 + Screen.height / 10, userinfoWidth - 6, 25), string.Format("Username: {0}", selectedUser.username));
                GUI.Label(new Rect(userinfoLeft + 3, 210 + Screen.height / 10, userinfoWidth - 6, 25), string.Format("Full name: {0}", selectedUser.fullname));
                GUI.Label(new Rect(userinfoLeft + 3, 230 + Screen.height / 10, userinfoWidth - 6, 50), string.Format("Location: {0}", selectedUser.location));
            }
        }

        // Go back to the demo selector
        if (GUI.Button(new Rect(20 + Screen.width / 2, Screen.height - 200, -30 + Screen.width / 2, 195), "Back"))
            Application.LoadLevel(0);
    }

    /// <summary>
    /// This function called when searching for users finished.
    /// </summary>
    private void OnSearchForUsersFinished(HTTPRequest request, HTTPResponse response)
    {
        searchStarted = false;
        if (response != null)
        {
            searchText = string.Empty;
            JsonReader reader = new JsonReader(response.DataAsText);
            reader.SkipNonMembers = true;
            searchResult = JsonMapper.ToObject<GithubUserSearchResult>(reader);

            if (searchResult == null || searchResult.users == null)
            {
                GithubMessage msg = JsonMapper.ToObject<GithubMessage>(response.DataAsText);
                if (msg != null)
                    searchText = msg.message;
            }
        }
        else
        {
            if (request.Exception != null)
            {
                searchText = request.Exception.Message;
                Debug.LogError(string.Format("{0}: {1}", request.Exception.Message, request.Exception.StackTrace));
            }
            else
                searchText = "Search failed!";
        }
    }
}