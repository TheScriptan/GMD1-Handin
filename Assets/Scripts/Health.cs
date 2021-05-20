using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthbar; 
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        healthbar.maxValue = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = health;
        if(health <= 0)
            Destroy(gameObject);
    }
}
