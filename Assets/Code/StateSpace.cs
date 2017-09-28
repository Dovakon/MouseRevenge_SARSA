using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class StateSpace : MonoBehaviour {

    public int verticalDistance;
    public int horizontalDistance;

    [HideInInspector]public int states;

    string st;
    int xPoss, yPoss;
    int horizontalTransisions;
    int count;

    public GameObject Traps1, Traps2, Cheese;

    [HideInInspector]public int[] traps1, traps2, cheese;

    void Start() {

        states = verticalDistance * horizontalDistance;

        traps1 = new int[Traps1.transform.childCount];
        traps2 = new int[Traps2.transform.childCount];
        cheese = new int[Cheese.transform.childCount];

        for (int i = 0; i < traps1.Length; i++)
        {
            traps1[i] = TakeState(Traps1.transform.GetChild(i).position);
            
        }
        for (int i = 0; i < traps2.Length; i++)
        {
            traps2[i] = TakeState(Traps2.transform.GetChild(i).position);
            
        }
        for (int i = 0; i < cheese.Length; i++)
        {
            cheese[i] = TakeState(Cheese.transform.GetChild(i).position);

        }


        horizontalTransisions = horizontalDistance;
        xPoss = 0;
        yPoss = 0;
        count = 0;
       
    }

    private int TakeState(Vector2 trapPoss)
    {
        //find state

        float possX = trapPoss.x;
        float possY = trapPoss.y;

        int state;

        state = (int)possX + ((int)possY * horizontalDistance);

        return state;
    }

    //void OnDrawGizmos()
    //{

    //    horizontalTransisions = horizontalDistance;
    //    xPoss = 0;
    //    yPoss = 0;
    //    count = 0;

    //    for (int i = 0; i < states; i++)
    //    {

    //        st = i.ToString();

    //        if (i < horizontalTransisions)
    //        {
    //            Handles.Label(new Vector3(xPoss + .3f, yPoss + .3f, 0), st);
    //        }
    //        else if (i >= horizontalTransisions)
    //        {
    //            yPoss++;
    //            xPoss = 0;
    //            Handles.Label(new Vector3(xPoss + .3f , yPoss + .3f, 0), st);
    //            horizontalTransisions += horizontalDistance;
    //        }

    //        xPoss++;
    //        //count++;
    //        //if (count > 66)
    //        //{
    //        //    count = 0;
    //        //}
    //    }

    //}

    //static void drawString(string text, Vector3 worldPos, Color? colour = null)
    //{
    //    UnityEditor.Handles.BeginGUI();
    //    if (colour.HasValue) GUI.color = colour.Value;
    //    var view = UnityEditor.SceneView.currentDrawingSceneView;
    //    Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
    //    Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
    //    GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
    //    UnityEditor.Handles.EndGUI();
    //}

}
