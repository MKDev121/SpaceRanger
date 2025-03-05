using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public float bullet_speed;
    public float bg_speed;
    Camera cam;
    public GameObject background;
    public List<GameObject> bgs;
    List<bool> truth_values;
    float timer;
    int score;
    public Text score_txt;
    bool paused;
    public bool gameover;
    public bool gameStart;
    GameObject gameOverScreen;
    public Text HighScoreText;
    public GameObject explode;
    // Start is called before the first frame update
    void Start()
    {
        cam=GameObject.Find("Main Camera").GetComponent<Camera>();
        bgs.Add(GameObject.Find("space1"));
        gameOverScreen=GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        if(HighScoreText!=null)
         HighScoreText.text=PlayerPrefs.GetInt("HighScore",0).ToString();
    
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameover&&gameStart){
            float horizontal=Input.GetAxis("Horizontal");
            float vertical=Input.GetAxis("Vertical");
            
            Vector3 movement=new Vector3(horizontal,vertical,0)*speed*Time.deltaTime;
            transform.position+=movement;  
            transform.position=Clamping(transform.position);

            shoot();

            if(timer>=0f)
                timer-=Time.deltaTime;
            else{
                score+=1;
                timer=1;
            }

        }
        score_txt.text=score.ToString();
            if(Input.GetKeyDown("space")){
                GameObject pause_menu=GameObject.Find("Canvas").transform.GetChild(1).gameObject;
                if(!gameover&&gameStart){
                    if(!paused){
                        pause_menu.SetActive(true);

                        paused=true;
                    }
                    else{
                        pause_menu.SetActive(false);
                        paused=false;
                    }
                }
            }
        if(gameover){
            gameObject.GetComponent<SpriteRenderer>().enabled=false;     
            gameOverScreen.SetActive(true);
            if(score>PlayerPrefs.GetInt("HighScore")){
                PlayerPrefs.SetInt("HighScore",score);
                PlayerPrefs.Save();
            }
        }
        if(paused){
            Time.timeScale=0f;
            GameObject.Find("scorePause").GetComponent<Text>().text=PlayerPrefs.GetInt("HighScore",0).ToString();
        }
        else
            Time.timeScale=1f;  
    }
    Vector3 Clamping(Vector3 position){
        return new Vector3(Mathf.Clamp(position.x,-8,8),Mathf.Clamp(position.y,-4.2f,4.2f),0);
    }
    void shoot(){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Shoot");
            GameObject obj=Instantiate(bullet,transform.GetChild(0).position,transform.GetChild(0).rotation);
            obj.GetComponent<Rigidbody2D>().velocity=transform.GetChild(0).up*bullet_speed;
            GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="enemy" ||other.name=="Boss"){
            
            gameover=true;
            var obj=Instantiate(explode,transform.position,transform.rotation);
            Destroy(obj,.31f);
        }

    }
    public void startGame(){
                GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
                gameStart=true;
    }
    public void retry(){
        SceneManager.LoadScene(0);
    }

}
