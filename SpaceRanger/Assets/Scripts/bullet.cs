using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject explode;
    public bool enemy_bullet;
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,2f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!enemy_bullet){//bullet of player
            if(other.tag=="enemy"){
                Debug.Log("hit");
                Destroy(other.gameObject);
                var obj=Instantiate(explode,transform.position,transform.rotation);
                Destroy(obj,.31f);
                Destroy(gameObject);
                
            }
        }else{//bullet of boss
            if(other.name=="player"){
            var obj=Instantiate(explode,other.transform.position,other.transform.rotation);
            Destroy(obj,.31f);
            other.GetComponent<player>().currentState=player.gameStates.GameOver;
            
            }
        }
    }
}
