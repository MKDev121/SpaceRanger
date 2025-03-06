using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.U2D.IK;

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
    int count=4;
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
                Boss.transform.position=new Vector3(0f,6f,0f);
                if(spawn_timer>=0f)
                    spawn_timer-=Time.deltaTime;
                else{
                    Debug.Log("Spawn");
                    spawn_timer=spawn_time;
                    if(count<=3)
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

        int spawcount=0;
        while(spawcount<4){
            Vector3 spaw_pos=new Vector3(Random.Range(-7,7),Random.Range(5f,10f),0f);
            Collider2D[] colliders=Physics2D.OverlapCircleAll(spaw_pos,1.5f);
            if(colliders.Length==0){
                spawcount+=1;
                 var obj=Instantiate(Enemy,spaw_pos,transform.rotation);
                 obj.GetComponent<Rigidbody2D>().velocity=-transform.up*enemy_speed/Random.Range(1,1.5f);
                 Destroy(obj,5f);
            }
        }
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
        float x=7*Mathf.Sin(2f*Time.time);
        boss.transform.position=transform.position+ new Vector3(x,-3f,0f);
    }


}
