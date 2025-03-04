using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject explode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,2f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="enemy"){
            Debug.Log("hit");
            Destroy(other.gameObject);
            var obj=Instantiate(explode,transform.position,transform.rotation);
            Destroy(obj,.31f);
            Destroy(gameObject);
            
        }
    }
}
