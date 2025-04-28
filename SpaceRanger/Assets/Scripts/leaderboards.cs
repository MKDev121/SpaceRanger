using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using System.IO;
using System.Threading;
public class leaderboards : MonoBehaviour
{

    public GameObject nameField;
    public GameObject leaderboard;
    public GameObject playerStat;
    public BackendManager backendManager;
    public Transform content;
    
    GameObject player;
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
    void Awake()
    {
        player=GameObject.Find("player");
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        filePath=Application.persistentDataPath + "/playerData.json";

        PlayerData loadedData=LoadData();
        if(loadedData != null){
            Debug.Log("Name: " + loadedData.name);
            Debug.Log("Score: " + loadedData.score);
            Debug.Log("ID: " + loadedData.ID);
            leaderboard.SetActive(true);
            loadedData=LoadData();
            backendManager.SendScoreToBackend(loadedData.ID,loadedData.name,loadedData.score);
            
            
        }else{
            PlayerData data=new PlayerData{name="",score=PlayerPrefs.GetInt("HighScore"),ID=0}; 
            SaveData(data);
            nameField.SetActive(true);
            Debug.Log("No data found, created new data file.");

            
            //Debug.Log("Data sent to backend: " + jsonData);
        }
        //string name="Meet";

        //SendDataToBackend( apiUrl, jsonData);
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
        string playerName=nameField.GetComponent<InputField>().text;
        PlayerData newData=new PlayerData{name=playerName, score=currentData.score, ID=currentData.ID};
        SaveData(newData);
        SetScore();
        nameField.SetActive(false);
        leaderboard.SetActive(true);
    }
    public void SetScore(){
        PlayerData currentData=LoadData();
        PlayerData newData=new PlayerData{score=int.Parse(player.GetComponent<player>().HighScoreText.text), name=currentData.name, ID=currentData.ID};
        SaveData(newData);
        Debug.Log("Score: " + newData.score);
        backendManager.SendDataToBackend(currentData.name,currentData.ID,newData.score);
        backendManager.SendScoreToBackend(currentData.ID,currentData.name,newData.score);
    }
    public void SetID(int ID){
        PlayerData currentData=LoadData();
        PlayerData newData=new PlayerData{ID=ID, name=currentData.name, score=currentData.score};
        SaveData(newData);
    }
    public void ShowPlayerStats(BackendManager.ScoresResponse stats){
        int i=0;
        foreach(var stat in stats.scores){
            GameObject obj=Instantiate(playerStat,content);
            obj.transform.SetParent(content);
            obj.transform.position=content.GetChild(0).position+new Vector3(0,-(i+1)*100,0);
            obj.transform.GetChild(0).GetComponent<Text>().text=(++i).ToString();
            obj.transform.GetChild(1).GetComponent<Text>().text=stat.playerName;
            obj.transform.GetChild(2).GetComponent<Text>().text=stat.playerScore.ToString();
        }
    }
}
