using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg : MonoBehaviour
{
    bool spawned=false;
    public float speed=4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down*speed*Time.deltaTime);
        if(transform.position.y<4.9f && !spawned){
            GameObject obj=Instantiate(gameObject,transform.position+new Vector3(0f,9f,0f),transform.rotation);
            spawned=true;
        }
        if(transform.position.y<-10f)
            Destroy(gameObject);
    }
}
