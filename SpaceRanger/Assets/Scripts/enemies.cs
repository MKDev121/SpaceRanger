using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemies : MonoBehaviour
{
    public float spawn_time;
    float spawn_timer;
    public GameObject Enemy;
    public float enemy_speed;
    public float boss_time;
    float attack_time;
    float boss_timer;
    float attack_timer;
    bool boss_mode=true;
    int count=0;
    public GameObject Boss;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        spawn_timer=spawn_time;
        attack_time=1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("player").GetComponent<player>().gameStart){
            if(!boss_mode){
                Boss.SetActive(false);
                if(spawn_timer>=0f)
                    spawn_timer-=Time.deltaTime;

                else{
                    Debug.Log("Spawn");
                    spawn_timer=spawn_time;
                    if(count<=6)
                        count++;
                    else{
                        count=0;
                        boss_mode=true;
                    }
                    Spawn();
                }
            }else{
                if(boss_timer>=0)
                    boss_timer-=Time.deltaTime;
                else{
                    boss_timer=boss_time;
                    boss_mode=false;
                    enemy_speed+=.5f;
                    spawn_time-=.5f;
                }
                BossTime(Boss);


            }
            spawn_time=Mathf.Clamp(spawn_time,.5f,3f);

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
    void BossTime(GameObject boss){
        boss.SetActive(true);
        if(attack_timer>=0)
            attack_timer-=Time.deltaTime;
        else{
            Debug.Log("Attack");
            GameObject obj=Instantiate(bullet,boss.transform.GetChild(0).position,boss.transform.rotation);
            obj.GetComponent<Rigidbody2D>().velocity=boss.transform.up*15;
            attack_timer=attack_time;
        }
    }
}
