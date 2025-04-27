using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

using System.IO;
public class leaderboards : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1:5000:5000/get_ID"; // Replace with your API URL
        // Send data to the backend via POST
    public void SendDataToBackend(string url, string jsonData,int idx)
    {
        StartCoroutine(PostRequest(url, jsonData,idx));
    }

    private IEnumerator PostRequest(string url, string jsonData,int idx)
    {
        // Create a UnityWebRequest with the JSON data
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
        if(idx==1){
            IDResponse response=JsonUtility.FromJson<IDResponse>(request.downloadHandler.text);
            SetID(response.ID);
        }
    }

    

    // Fetch data from the backend via GET
    public void FetchDataFromBackend(string url)
    {
        StartCoroutine(GetRequest(url));
    }

    private IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data received: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }
  
    class IDResponse{
        public int ID;
    }

    public class PlayerData{
        public string name;
        public int score=0;
        public int ID=0;
    }
    string filePath;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        filePath=Application.persistentDataPath + "/playerData.json";

        PlayerData loadedData=LoadData();
        if(loadedData != null){
            Debug.Log("Name: " + loadedData.name);
            Debug.Log("Score: " + loadedData.score);
            Debug.Log("ID: " + loadedData.ID);
        }else{
            PlayerData data=new PlayerData();
            SaveData(data);
            Debug.Log("No data found, created new data file.");
            SetName();
            string jsonData = "{\"name\": \"PlayerName\"}";
            SendDataToBackend( apiUrl, jsonData,1);
            //Debug.Log("Data sent to backend: " + jsonData);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData(PlayerData data){
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
    }

    public PlayerData LoadData(){
        if(File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Data loaded from " + filePath);
            return data;
        }else{
            //Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
    public void SetName(){
        PlayerData currentData=LoadData();
        PlayerData newData=new PlayerData{name="Player1", score=currentData.score, ID=currentData.ID};
        SaveData(newData);
    }
    public void SetScore(){
        PlayerData currentData=LoadData();
        PlayerData newData=new PlayerData{score=100, name=currentData.name, ID=currentData.ID};
        SaveData(newData);
    }
    void SetID(int ID){
        PlayerData currentData=LoadData();
        PlayerData newData=new PlayerData{ID=ID, name=currentData.name, score=currentData.score};
        SaveData(newData);
    }

}
