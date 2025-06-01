using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class movementTracker : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;

    private terrainMorpher tm;

    private float currX;
    private float currY;
    private float currZ;

    [SerializeField]
    private bool flag = false; //true = stretch, false = multiplier

    private void Awake()
    {
        tm = terrain.GetComponent<terrainMorpher>();    
    }
    void Start()
    {
        currX = transform.position.x; currY = transform.position.y; currZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        { 
            tm.morphWithMovement(transform.position.x - currX, transform.position.y - currY);
            /*if (tm.getFlag())
            {
                tm.updatePerlinStretch();
            }
            else
            {
                tm.decreasePerlinStretch();
            }*/
        }
        
    }
}
