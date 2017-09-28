
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {

    enum Movement {N, L, R, U, D, UL, UR, DL, DR};

    Movement chosenMovement;

    public int Speed;
    
    public StateSpace space;
    public Transform[] Checkpoints;

    private int verticalTransition;
    
    private Rigidbody2D rb;
    

    void Start () {

        rb= GetComponent<Rigidbody2D>();
        chosenMovement = Movement.N;
        verticalTransition = space.horizontalDistance; 
    }


    void Update()
    {
        if (chosenMovement == Movement.N)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (chosenMovement == Movement.L)
        {
            transform.localEulerAngles = new Vector3(0, 0, 90);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.R)
        {
            transform.localEulerAngles = new Vector3(0, 0, -90);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.U)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.D)
        {
            transform.localEulerAngles = new Vector3(0, 0, 180);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.UL)
        {
            transform.localEulerAngles = new Vector3(0, 0, 45);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.UR)
        {
            transform.localEulerAngles = new Vector3(0, 0, -45);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.DL)
        {
            transform.localEulerAngles = new Vector3(0, 0, 135);
            rb.velocity = transform.up * Speed;
        }
        if (chosenMovement == Movement.DR)
        {
            transform.localEulerAngles = new Vector3(0, 0, -135);
            rb.velocity = transform.up * Speed;
        }

        transform.position = new Vector2 (Mathf.Clamp(transform.position.x, 0.1f, space.horizontalDistance - .1f), Mathf.Clamp(transform.position.y, 0.05f, space.verticalDistance - .1f));

    }

    public void Move(string action)
    {
        //N ----> Nothing
        //L ----> Left
        //R ----> Right
        //U ----> Up
        //D ----> Down
        //UL ----> Up & Left
        //UR ----> Up & Right
        //DL ----> Down & Right
        //DR ----> Down & Right


        if (action == "L")
        {
            chosenMovement = Movement.L;
            //action = "L";
        }
        else if (action == "R")
        {
            chosenMovement = Movement.R;
            //action = "R";
        }
        else if (action == "U")
        {
            chosenMovement = Movement.U;
            //action = "U";
        }
        else if (action == "D")
        {
            chosenMovement = Movement.D;
            //action = "U";
        }
        else if (action == "UL")
        {
            chosenMovement = Movement.UL;
            //action = "U";
        }
        else if (action == "UR")
        {
            chosenMovement = Movement.UR;
            //action = "U";
        }
        else if (action == "DL")
        {
            chosenMovement = Movement.DL;
            //action = "U";
        }
        else if (action == "DR")
        {
            chosenMovement = Movement.DR;
            //action = "U";
        }

    }

    public int CurrentState()
    {

        //find state

        float possX = transform.position.x;
        float possY = transform.position.y;

        int state = 0;


        state = (int)possX + ((int)possY * verticalTransition);

        //if (possY < 1)
        //{
        //    state = (int)possX;
        //}
        //else if (possY < 2)
        //{
        //    state = (int)(possX) + verticalTransition;
        //}
        //else if (possY < 3)
        //{
        //    state = (int)(possX) + (2 * verticalTransition);
        //}
        //else if (possY < 4)
        //{
        //    state = (int)(possX) + (3 * verticalTransition);
        //}
        //else if (possY < 5)
        //{
        //    state = (int)(possX) + (4 * verticalTransition);
        //}
        //else if (possY < 6)
        //{
        //    state = (int)(possX) + (5 * verticalTransition);
        //}

        if (state < 0)
        {
            print("Possition :" + transform.position);
            Debug.LogError("Vgike Eksw Apo To StateSpace");
            return 0;
        }
        else
        {
            return state;
        }
        
    }

    public void EnterTrap(int point)
    {
        transform.position = Checkpoints[point].position;
    }


    public void ResetPoss()
    {
        transform.position = new Vector2(.1f, .1f);
    }

    public void StopMoving()
    {
        chosenMovement = Movement.N;
    }
    
}
