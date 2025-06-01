using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Animations;

public class mover : MonoBehaviour
{

    private Animator animator;
    private bool flag;
    public Terrain terrain;
    private TerrainMorpher tm;

    /*public float xOff = 0;
    public float yOff = 0;
    public int depth = 0;*/

    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        flag = false;
        tm = terrain.transform.GetComponent<TerrainMorpher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoveRig(6);
        }
        if (flag == true)
           tm.moveTerrainWithModel();
    }

    public void MoveRig(int direction)
    {
        switch(direction)
        {
            case 6: //animate
                animator.SetBool("IsMoving", !flag);
                flag = !flag;
                break;  
        }
    }

}
