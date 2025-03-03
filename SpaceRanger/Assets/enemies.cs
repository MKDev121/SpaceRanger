using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemies : MonoBehaviour
{
    public float spawn_time;
    float spawn_timer;
    public GameObject Enemy;
    public float enemy_speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("player").GetComponent<player>().gameStart){
            if(spawn_timer>=0f)
                spawn_timer-=Time.deltaTime;
            else{
                Debug.Log("Spawn");
                spawn_timer=spawn_time;
                
                Spawn();
            }
        }
    }
    void Spawn(){
        GameObject obj=Instantiate(Enemy,transform.position+new Vector3(Random.Range(0,7),0f,0f),transform.rotation);
        obj.GetComponent<Rigidbody2D>().velocity=-transform.up*enemy_speed/Random.Range(1,1.5f);
        Destroy(obj,5f);
        obj=Instantiate(Enemy,transform.position+new Vector3(Random.Range(-7,0),0f,0f),transform.rotation);
        obj.GetComponent<Rigidbody2D>().velocity=-transform.up*enemy_speed/Random.Range(1,1.5f);
        Destroy(obj,5f);
    }
}
