using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

using BestHTTP;
using BestHTTP.Caching;

[RequireComponent(typeof(AudioSource))]
public class DownloadStreamingTestScript : MonoBehaviour {
    // read from wave header:
    int bytesPerSec;
    int frames;
    float lengthInSec;

    // calculated:
    int downloadedBytes;
    int downloadLength;

    // audio playing:
    AudioClip clip;
    int dataPos;
    int samplePos;

    void Update()
    {
        // Go back to the demo selector
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);
    }

    void OnGUI()
    {
        // Start download the audio file
        if (!GetComponent<UnityEngine.AudioSource>().isPlaying && GUI.Button(new Rect(10, 10, 200, 60), "Play"))
            StartDownload();

        // Display some info if an audio is playing
        if (GetComponent<UnityEngine.AudioSource>().isPlaying)
        {
            GUI.Label(new Rect(10, 100, 200, 30), string.Format("{0} / {1}", GetComponent<UnityEngine.AudioSource>().time, lengthInSec));

            if (clip != null)
                GUI.Label(new Rect(10, 140, 400, 30), string.Format("Downloaded: {0} / {1} - {2:P2}", downloadedBytes, frames, downloadedBytes / (float)downloadLength));
        }

        HTTPManager.IsCachingDisabled = GUI.Toggle(new Rect(10, 180, 400, 30), HTTPManager.IsCachingDisabled, "Caching Disabled");

        if (GUI.Button(new Rect(3, Screen.height - 200, -5 + Screen.width / 2, 195), "Clear cache"))
            HTTPCacheService.BeginClear();

        // Go back to the demo selector
        if (GUI.Button(new Rect(20 + Screen.width / 2, Screen.height - 200, -30 + Screen.width / 2, 195), "Back"))
            Application.LoadLevel(0);
    }

    void StartDownload()
    {
        GetComponent<UnityEngine.AudioSource>().clip = clip = null;
        downloadedBytes = 0;

        // wav file will be downloaded from this page: http://download.wavetlan.com/SVV/Media/HTTP/http-wav.htm
        var request = new HTTPRequest(new Uri("http://download.wavetlan.com/SVV/Media/HTTP/test_mono_44100Hz_8bit_PCM.wav"), OnfragmentDownloaded);

        // We will track the download progress
        request.OnProgress = OnDownloadProgress;

        // if UseStreaming true then, the given callback will called as soon as possible if at least one 
        //  fragment downloaded
        request.UseStreaming = true;

        // how big a fragment is. 
        request.StreamFragmentSize = 1024;

        // start proces the request
        HTTPManager.SendRequest(request);
    }

    /// <summary>
    /// Called when:
    ///     -At least one StreamFragmentSize sized fragment downloaded
    ///     -An error occured
    ///     -Download/streaming finished
    /// </summary>
    void OnfragmentDownloaded(HTTPRequest req, HTTPResponse resp)
    {
        if (resp == null)
        {
            Debug.LogError("Download Failed!");
            return;
        }

        // If streaming is used, then every time this callback function called we can use this function to
        //  retrive the downloaded and buffered data. The returned list can be null, if there is no data yet.
        List<byte[]> downloadedFragments = resp.GetStreamedFragments();

        if (downloadedFragments != null)
        {
            Debug.Log("Buffer size: " + downloadedFragments.Count);

            // First time setup:
            if (clip == null)
            {
                // read wave header data
                dataPos = ReadWavData(downloadedFragments[0]);

                // create a new audioclip
                GetComponent<UnityEngine.AudioSource>().clip = clip = AudioClip.Create("Streamed Clip", frames, 1, bytesPerSec, false
#if !UNITY_5_0
                    , false
#endif
                    );
                GetComponent<UnityEngine.AudioSource>().loop = false;

                // no audio samples added yet to our clip
                samplePos = 0;
            }

            // feed our audio clip with the newly arrived data
            FeedAudio(downloadedFragments);
        }

        if (resp.IsStreamingFinished)
            Debug.Log("Download Finished!");
    }

    void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
    {
        downloadedBytes = downloaded;
        downloadLength = length;
    }

    void FeedAudio(List<byte[]> buffer)
    {
        while (buffer.Count > 0)
        {
            byte[] sourceData = buffer[0];
            float[] data = new float[sourceData.Length - dataPos];

            // byte(0..255) to float(-1..1) conversion
            for (int i = 0; i < data.Length; ++i)
                data[i] = (127 - sourceData[i]) / 128f;

            // give the converted data to the audio clip
            clip.SetData(data, samplePos);

            // set up next time's position
            samplePos += data.Length;
            dataPos = 0;

            // remove the processed data
            buffer.RemoveAt(0);
        }

        // if not already playing, start playing
        if (GetComponent<AudioSource>() && !GetComponent<UnityEngine.AudioSource>().isPlaying)
            GetComponent<UnityEngine.AudioSource>().Play();
    }

    /// <summary>
    /// VERY simple wav header parser. For this streaming sample we are only supporting 8 bit PCMs with one channel(mono).
    /// </summary>
    /// <returns>The position, where the actual sound data starts. (Should be 44 - the length of the wav header)</returns>
    int ReadWavData(byte[] data)
    {
        using (BinaryReader br = new BinaryReader(new MemoryStream(data, false)))
        {
            br.ReadChars(4); //"RIFF"
            br.ReadInt32(); // length
            br.ReadChars(4); //"WAVE"
            string chunk = new string(br.ReadChars(4)); //"fmt "
            int chunkLength = br.ReadInt32();

            //1 == uncompressed PCM
            if (br.ReadInt16() != 1)
                throw new Exception("Not supported format");

            if (br.ReadInt16() != 1)
                throw new Exception("Too much channel! Try a mono one.");

            br.ReadInt32();
            bytesPerSec = br.ReadInt32();
            br.ReadInt16(); // block align

            // bits per sample
            if (br.ReadInt16() != 8)
                throw new Exception("Too mutch bits per sample! Try an 8 bit one.");
            br.ReadChars(chunkLength - 16);
            chunk = new string(br.ReadChars(4));
            try
            {
                while (chunk.ToLower() != "data")
                {
                    br.ReadChars(br.ReadInt32());
                    chunk = new string(br.ReadChars(4));
                }
            }
            catch
            {
                throw new Exception("No data found!");
            }

            // chunk length:
            frames = br.ReadInt32();

            lengthInSec = frames / (float)bytesPerSec;

            return (int)br.BaseStream.Position;
        }
    }
}