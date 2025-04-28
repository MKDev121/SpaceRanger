using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class player : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public float bullet_speed;
    float timer;
    int score;
    public Text score_txt;
    bool paused;
    public bool gameover;
    public bool gameStart;
    GameObject gameOverScreen;
    public GameObject creditsScreen;
    public GameObject menuScreen;
    public GameObject leaderboardScreen;
    public Text HighScoreText;
    public GameObject explode;
    public InputActionAsset controls ;
    public gameStates currentState;
    
    
    public enum gameStates{
        MainMenu,
        Running,
        Paused,
        GameOver,
        Credits,
        LeaderBoards

    }
    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen=GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        currentState=gameStates.MainMenu;
        if(HighScoreText!=null)
            HighScoreText.text=PlayerPrefs.GetInt("HighScore",0).ToString();
        
        controls.Enable();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if(currentState==gameStates.Running){
            
            movement(controls.FindAction("Move").ReadValue<Vector2>());
            
            shoot(controls.FindAction("Attack").triggered);
            
            if(timer>=0f)
                timer-=Time.deltaTime;
            else{
                score+=1;
                //Color color=Color.HSVToRGB(Random.value,1,1);
                //GetComponent<SpriteRenderer>().material.SetColor("_Color",color);
                timer=1;
            }

        }
        pausing();
        score_txt.text=score.ToString();

        if(currentState==gameStates.GameOver){
            gameObject.GetComponent<SpriteRenderer>().enabled=false;     
            gameOverScreen.SetActive(true);
            if(score>PlayerPrefs.GetInt("HighScore")){
                PlayerPrefs.SetInt("HighScore",score);
                PlayerPrefs.Save();  
            }
            
        }

    }
    Vector3 Clamping(Vector3 position){
        return new Vector3(Mathf.Clamp(position.x,-8,8),Mathf.Clamp(position.y,-4.2f,4.2f),0);
    }
    void movement(Vector2 input){
        float horizontal=input.x;
        float vertical=input.y;
        //Debug.Log(input);
        Vector3 movement=new Vector3(horizontal,vertical,0)*speed*Time.deltaTime;
        transform.position+=movement;  
        transform.position=Clamping(transform.position);
    }

    void pausing(){

        if(currentState==gameStates.Paused){
            Time.timeScale=0f;
            GameObject.Find("scorePause").GetComponent<Text>().text=PlayerPrefs.GetInt("HighScore",0).ToString();
        }
        else
            Time.timeScale=1f;  
    }

    void shoot(bool mouseDown){
        if(mouseDown){
            Debug.Log("Shoot");
            GameObject obj=Instantiate(bullet,transform.GetChild(0).position,transform.GetChild(0).rotation);
            obj.GetComponent<Rigidbody2D>().velocity=transform.GetChild(0).up*bullet_speed;
            transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="enemy" ||other.name=="Boss"){
            currentState=gameStates.GameOver;
            var obj=Instantiate(explode,transform.position,transform.rotation);
            Destroy(obj,.31f);
        }

    }
    public void startGame(){
                currentState=gameStates.Running;
                GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true); 
                gameStart=true;
    }
    public void credits(bool show){
        creditsScreen.SetActive(show);
       // GameObject pauseButton=GameObject.Find("Canvas").transform.GetChild(7).gameObject;   
        //pauseButton.SetActive(!show);
        menuScreen.SetActive(!show);
    }
    public void LeaderBoard(bool show){
        leaderboardScreen.SetActive(show);
        //GameObject pauseButton=GameObject.Find("Canvas").transform.GetChild(7).gameObject;   
        //pauseButton.SetActive(!show);
        menuScreen.SetActive(!show);
    }
    public void retry(){
        SceneManager.LoadScene(0);
    }
    public void pauseGame(){
            GameObject pause_menu=GameObject.Find("Canvas").transform.GetChild(1).gameObject;
            GameObject pauseButton=GameObject.Find("Canvas").transform.GetChild(7).gameObject;        
            switch(currentState){
                case gameStates.Paused:
                pause_menu.SetActive(false);
                pauseButton.SetActive(true);
                currentState=gameStates.Running;
                break;
                case gameStates.Running:
                pause_menu.SetActive(true);
                pauseButton.SetActive(false);
                currentState=gameStates.Paused;
                break;
            }
    }

}
