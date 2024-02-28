using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class StackDataFetcher
{
    public async void RequestStackData()
    {
        string uri = Constants.STACKS_DATA_URL;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
                OnDataFailed(webRequest.error);
            }
            else
            {
                OnDataReceived(webRequest.downloadHandler.text);
            }
        }
    }

    private void OnDataReceived(string jsonData)
    {
        var stackDataList = new StackDataResponse()
        {
            Data = JsonConvert.DeserializeObject<List<BlockData>>(jsonData)
        };
        StackDataFetchedEvent.Trigger(stackDataList);
    }

    private void OnDataFailed(string error)
    {
        Debug.LogError("Failed to receive data: " + error);
    }
}
