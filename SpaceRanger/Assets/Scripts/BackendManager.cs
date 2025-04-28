using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;
public class BackendManager : MonoBehaviour
{
    int id;
    public leaderboards leaderboard;
    // URL of your backend server
    string postUrl = "https://mkdev121.pythonanywhere.com/get_ID"; // Replace with your backend's POST endpoint
    string getUrl = "https://mkdev121.pythonanywhere.com/get_scores"; // Replace with your backend's GET endpoint
    string scoreUrl="https://mkdev121.pythonanywhere.com/update_score";

    PlayerData playerData;
    // Method to send data to the backend via POST
    public void SendDataToBackend(string playerName,int playerID,int playerScore)
    {
        // Create JSON payload
        Debug.Log("Sending data to backend: " + playerName);
         playerData =new PlayerData { name = playerName ,ID=playerID,score=playerScore};
        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log("Payload being sent: " + jsonData);
        // Start the coroutine to send the POST request
        StartCoroutine(PostRequest(postUrl, jsonData,1));
        
        
    }

    public void SendScoreToBackend(int playerID,string playerName,int playerScore)
    {   
        PlayerData playerData =new PlayerData { name = playerName ,ID=playerID,score=playerScore};
        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log("Payload being sent: " + jsonData);
        // Start the coroutine to send the POST request
        StartCoroutine(PostRequest(scoreUrl, jsonData,0));
    }
    private IEnumerator PostRequest(string url, string jsonData,int idx)
    {
        // Create JSON payload as bytes
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Configure UnityWebRequest for POST method
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json"); // Set the Content-Type header

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST successful: " + request.downloadHandler.text);

            // Deserialize response if necessary
            if(idx==1)
            {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            leaderboard.SetID(response.ID);
            Debug.Log("Received ID: " + response.ID);
            
            
            }
             GetScoresFromBackend();
            
        }
        else
        {
            Debug.LogError("POST failed: " + request.error);
        }
    }


    // Method to fetch data from the backend via GET
    public void GetScoresFromBackend()
    {
        // Start the coroutine to send the GET request
        StartCoroutine(GetRequest(getUrl));
    }

    private IEnumerator GetRequest(string url)
    {
        // Configure UnityWebRequest for GET method
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GET successful: " + request.downloadHandler.text);

            // Deserialize response if necessary
            ScoresResponse response = JsonUtility.FromJson<ScoresResponse>(request.downloadHandler.text);
            leaderboard.ShowPlayerStats(response);
            foreach (var score in response.scores)
            {
                Debug.Log($"Player: {score.playerName}, Score: {score.playerScore}");
            }
        }
        else
        {
            Debug.LogError("GET failed: " + request.error);
        }
    }

    // Classes for JSON deserialization
    [System.Serializable]
    public class ServerResponse
    {
        public int ID;
    }

    [System.Serializable]
    public class ScoresResponse
    {
        public ScoreEntry[] scores;
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int playerScore;
    }
    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int ID;
        public int score;
    }
}

