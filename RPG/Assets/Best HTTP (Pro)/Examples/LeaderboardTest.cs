using System;
using System.Collections.Generic;
using UnityEngine;

using BestHTTP;
using System.IO;


public class LeaderboardTest : MonoBehaviour
{
    const int count = 10;

    private int from = 0;
    private string downloaded = string.Empty;
    private List<LeaderboardEntry> Entries = new List<LeaderboardEntry>(count);
    private HTTPResponse response;

    void Update()
    {
        // Go back to the demo selector
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 60), "Refresh") && (response == null || response.IsStreamingFinished))
            StartRefreshLeaderboard();

        if (GUI.Button(new Rect(240, 10, 200, 60), "Clear Cache"))
            BestHTTP.Caching.HTTPCacheService.BeginClear();

        GUI.Label(new Rect(10, 80, 200, 20), "Rank          Name            Points");
        if (Entries.Count > 0)
            for (int i = 0; i < Entries.Count; ++i)
                GUI.Label(new Rect(10, 100 + (i * 30), 400, 25), string.Format("{0,5}{1,20}{2,6}", from + i + 1, Entries[i].Name, Entries[i].Points));

        if (response != null)
        {
            GUI.color = response.IsStreamingFinished ? Color.green : Color.red;
            string status = response.IsStreamingFinished ? "Download Finished" : "Downloading";
            if (response.IsFromCache)
            {
                GUI.color = Color.yellow;
                status += " From Cache!";
            }

            GUI.Label(new Rect(80, 100 + (Entries.Count + 1) * 30, 250, 20), status);
            GUI.color = Color.white;

            string expires = response.GetFirstHeaderValue("expires");
            GUI.Label(new Rect(10, Screen.height - 50, 400, 20), "Cache will expire: " + DateTime.Parse(expires).ToUniversalTime());
            GUI.Label(new Rect(10, Screen.height - 25, 400, 20), "Current time: " + DateTime.UtcNow);
        }

        if (response != null && response.IsStreamingFinished)
        {
            if (GUI.Button(new Rect(20, 500, 200, 60), "Prev Page") && from > 0)
            {
                from -= count;
                StartRefreshLeaderboard();
            }

            if (GUI.Button(new Rect(250, 500, 200, 60), "Next Page"))
            {
                from += count;
                StartRefreshLeaderboard();
            }
        }

        if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 65, 110, 60), "Back"))
            Application.LoadLevel(0);
    }

    void StartRefreshLeaderboard()
    {
        downloaded = string.Empty;
        Entries.Clear();
        response = null;

        var request = new HTTPRequest(new Uri(string.Format("http://besthttp.azurewebsites.net/api/LeaderboardTest?from={0}&count={1}", from, count)), OnLeaderboardArrived);

        // We don't want compression now.
        request.SetHeader("Accept-Encoding", "deflate");

        // Set up streaming
        request.UseStreaming = true;
        request.StreamFragmentSize = 10;

        // Send the request
        request.Send();
    }

    void OnLeaderboardArrived(HTTPRequest req, HTTPResponse resp)
    {
        this.response = resp;

        if (resp == null)
        {
            Debug.LogError(string.Format("{0}: {1}", req.Exception.Message, req.Exception.StackTrace));
            return;
        }

        List<byte[]> downloadedFragments = resp.GetStreamedFragments();

        if (downloadedFragments != null)
        {
            foreach (var buffer in downloadedFragments)
                downloaded += System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            ProcessDownloaded();
        }
    }

    void ProcessDownloaded()
    {
        int nextLineEnd = downloaded.IndexOf("\n");
        while (nextLineEnd != -1)
        {
            string line = downloaded.Substring(0, nextLineEnd);
            downloaded = downloaded.Substring(nextLineEnd + 1);

            ProcessLine(line);

            nextLineEnd = downloaded.IndexOf("\n");
        }
    }

    void ProcessLine(string line)
    {
        string[] nameAndPoints = line.Split(';');

        Entries.Add(new LeaderboardEntry(nameAndPoints[0], int.Parse(nameAndPoints[1])));
    }
}

public sealed class LeaderboardEntry
{
    public string Name { get; private set; }
    public int Points { get; private set; }

    public LeaderboardEntry(string name, int points)
    {
        this.Name = name;
        this.Points = points;
    }
}

/* The server code in ruby using the Sinatra framework:
get '/LeaderboardTest/:from/:count' do |f, c|
    from = f.to_i
    count = c.to_i

    # Set 'Expires' header to the current Time + 1 minute
    headers 'Expires' => (Time.now + 60).utc.httpdate.to_s

    # Create the leaderboard stream and write out 10 player
    stream do |out|
        count.to_i.times do |pos|
            out << "Player_Name_#{(from + pos).to_s};#{(from + count - pos) * 100}\n"

            # Here is the streaming magic. :)
            sleep 0.5
        end
    end
end
*/

/* The server code in ASP. Net Web Api 2:
public class LeaderBoardTestController : ApiController
{
    //
    // GET api/LeaderBoardTest?from={0}&count={1}
    public HttpResponseMessage Get(int from, int count)
    {
        var resp = new HttpResponseMessage(HttpStatusCode.OK);
        resp.Content = new PushStreamContent(async (outStream, content, context) =>
        {
            try
            {
                for (int i = 0; i < count; ++i)
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(string.Format("Player_Name_{0};{1}\n", from + i, from + count - i));
                    await outStream.WriteAsync(buffer, 0, buffer.Length);
                    System.Threading.Thread.Sleep(500);
                }
            }
            finally
            {
                outStream.Close();
            }
        });

        resp.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
        resp.Content.Headers.Add("Expires", DateTime.Now.AddSeconds(60).ToUniversalTime().ToString("R"));

        return resp;
    }
}
*/