using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadService : MonoBehaviour
{
    public static IEnumerator DownloadFileAsync(string url, Action<float> onProgress, Action<string> onDownloadComplete)
    {
        const string fileName = "temporary-pointCloud";
        Debug.Log($"Downloading point cloud, please wait...");  
        
        using (var request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (!request.isDone)
            {
                onProgress?.Invoke(request.downloadProgress);
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var savePath = $"{Application.persistentDataPath}/{fileName}.ply";
                System.IO.File.WriteAllBytes(savePath, request.downloadHandler.data);
                
                
                Debug.Log($"Downloading point completed successfully.");  
                onDownloadComplete?.Invoke(savePath);
            }
        }
    }
}
