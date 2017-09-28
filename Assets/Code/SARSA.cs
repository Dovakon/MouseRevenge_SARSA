
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SARSA : MonoBehaviour
{
    
    public CharacterMovement character;

    public Text episodeUI;
    public Text currentStateUI;
    public Text time;
    public GameObject arrow;

    public float Episodes;

    public StateSpace space;

    private List<State> state;
    private List<Action> action;
    
    //RL Settings
    int currentState;
    int nextState;
    
    void Start()
    {
        state = new List<State>();
        action = new List<Action>();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(RunPolicy());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            WriteQtable();
            WriteVtable();
            //WriteActions();
            SavePolicy();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InstantiateArrows();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartLearning();
        }

    }
    
    public void StartLearning()
    {
        int states = space.states;

        
       // state.Add(new State());
        

        for (int i = 0; i < states; i++)
        {
            
            state.Add(new State());
            state[i].StateValue = i;

            if (i == 8)
            {
                state[i].Reward = 1000;
                state[i].StateType = "G";
            }
            else
            {
                state[i].Reward = -100;
                state[i].StateType = "B";
            }


            for (int t1 = 0; t1 < space.traps1.Length; t1++)
            {
                if(i == space.traps1[t1])
                {
                    state[i].Reward = -500;
                    state[i].StateType = "T1";
                    
                }
            }
            for (int t2 = 0; t2 < space.traps2.Length; t2++)
            {
                if (i == space.traps2[t2])
                {
                    state[i].Reward = -500;
                    state[i].StateType = "T2";
                }
            }
            for (int c = 0; c < space.cheese.Length; c++)
            {
                if (i == space.cheese[c])
                {
                    state[i].Reward = -70;
                    state[i].StateType = "C";
                    
                }
            }
            


        }

        StartCoroutine(Learning());
    }

    IEnumerator Learning()
    {

        float Alpha = .1f;
        float Gamma = .9f;
        float eGreddy;
        int currentEpisode = 0;
        int currentAction = 0;
        //float startTime;
        

        float QValue;
        float qValue;

        string firstAction;
        string nextAction;

        int reachTree = 0;

        

        yield return new WaitForSeconds(1f);

        //First Action 
        //Move Right
        currentState = 0;
        firstAction = DoRandomAction();
        character.Move(firstAction);
        

        //action.Add(new Action(currentAction, Time.time, currentState, firstAction, state[currentState].Reward));


        yield return new WaitForSeconds(.2f);

        episodeUI.text = currentEpisode.ToString();

        while (currentEpisode < Episodes)
        {

            nextState = character.CurrentState();

            currentStateUI.text = nextState.ToString();

            
            string minutes = ((int)Time.time / 60).ToString();
            string seconds = (Time.time % 60).ToString("f2");


            time.text = minutes + ":" + seconds;

            eGreddy = ((currentEpisode) / Episodes);
            eGreddy = Mathf.Clamp(eGreddy, .4f, 0.7f);
            
            if (Random.value >= eGreddy)
            {
                print("random");
                nextAction = DoRandomAction();
            }
            else
            {
                nextAction = GetMaxQ(nextState);
                print("Max");
            }
            

            QValue = state[currentState].Action[firstAction];
            qValue = state[nextState].Action[nextAction];

            float rw = state[nextState].Reward; //- Time.time;
            QValue = QValue + Alpha * (rw + (Gamma * qValue) - QValue);

            
            state[currentState].Action[firstAction] = QValue;

            //print("Current State" + currentState);
            //print("first Action "+ firstAction);
            //print("Next State" + nextState);
            //print("Next Action " + nextAction);



            if (state[currentState].StateType == "G")
            {
                currentEpisode++;
                episodeUI.text = currentEpisode.ToString();
                
                currentState = 0;
                character.ResetPoss();
                firstAction = DoRandomAction();
                character.Move(firstAction);
               
            }
            else if (state[currentState].StateType == "B" || state[currentState].StateType == "C")
            {
                currentState = nextState;
                firstAction = nextAction;
                character.Move(firstAction);
                
            }
            else
            {
                if (state[currentState].StateType == "T1")
                {
                    //character.EnterTrap(0);
                    //currentState = character.CurrentState();

                    currentState = nextState;

                    firstAction = DoRandomAction();
                    character.Move(firstAction);
                }
                
                if (state[currentState].StateType == "T2")
                {
                    //character.EnterTrap(1);
                    //currentState = character.CurrentState();

                    currentState = nextState;

                    firstAction = DoRandomAction();
                    character.Move(firstAction);
                }
                
            }

            yield return new WaitForSeconds(.2f);


            currentAction++;
            //action.Add(new Action(currentAction, Time.time, currentState, firstAction, state[currentState].Reward));

        }
        print(reachTree);

        character.StopMoving();
        WriteQtable();
        WriteVtable();
        //WriteActions();
        SavePolicy();
    }

    string DoRandomAction()
    {
        int action = Random.Range(0, 8);

        if (action == 0)
        {
            return "L";
        }
        else if (action == 1)
        {
            return "R";
        }
        else if (action == 2)
        {
            return "U";
        }
        else if (action == 3)
        {
            return "D";
        }
        else if (action == 4)
        {
            return "UL";
        }
        else if (action == 5)
        {
            return "UR";
        }
        else if (action == 6)
        {
            return "DL";
        }
        else if (action == 7)
        {
            return "DR";
        }
        return null;
    }

    string GetMaxQ(int st)
    {
        string action = "L";

        
        if (state[st].Action[action] < state[st].Action["R"])
        {
            action = "R";
        }
        if (state[st].Action[action] < state[st].Action["U"])
        {
            action = "U";
        }
        if (state[st].Action[action] < state[st].Action["D"])
        {
            action = "D";
        }
        if (state[st].Action[action] < state[st].Action["UL"])
        {
            action = "UL";
        }
        if (state[st].Action[action] < state[st].Action["UR"])
        {
            action = "UR";
        }
        if (state[st].Action[action] < state[st].Action["DL"])
        {
            action = "DL";
        }
        if (state[st].Action[action] < state[st].Action["DR"])
        {
            action = "DR";
        }

        return action;
    }




    IEnumerator RunPolicy()
    {

        character.ResetPoss();
        int currentState = 0;
        Policy.Instance.LoadPolicy();

        string nextMove = Policy.Instance.policyState.state[currentState].action;
        character.Move(nextMove);
        yield return new WaitForSeconds(.2f);

        while (true)
        {
            if (character.CurrentState() == 39 || character.CurrentState() == 79 || character.CurrentState() == 119)
            {
                break;
            }
            else
            {
                currentState = character.CurrentState();
                print(currentState);
            }

            nextMove = Policy.Instance.policyState.state[currentState].action;
            character.Move(nextMove);
            yield return new WaitForSeconds(.2f);
        }
    }

    void SavePolicy()
    {
        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);

            PolicyValues pol = new PolicyValues();
            pol.action = MaxAction;
            pol.value = i;

            Policy.Instance.policyState.state.Add(pol);
            Policy.Instance.SavePolicy();

        }
    }

    void InstantiateArrows()
    {
        Policy.Instance.LoadPolicy();

        int verticalDist = space.verticalDistance;
        int horizontalDist = space.horizontalDistance;


        
        for (int i = 0; i < state.Count; i++)
        {
            string action = Policy.Instance.policyState.state[i].action;
            int Zrotation = 0;
            int Xposs, Yposs;

            Xposs = i;
            Yposs = 0;


            Yposs = Xposs / horizontalDist;
            Xposs -= Yposs * horizontalDist;

           

            if (action == "L")
            {
                Zrotation = 180;
            }
            else if (action == "R")
            {
                Zrotation = 0;
            }
            else if (action == "U")
            {
                Zrotation = 90;
            }
            else if (action == "D")
            {
                Zrotation = -90;
            }
            else if (action == "UL")
            {
                Zrotation = 135;
            }
            else if (action == "UR")
            {
                Zrotation = 45;
            }
            else if (action == "DL")
            {
                Zrotation = 225;
            }
            else if (action == "DR")
            {
                Zrotation = -45;
            }


            Instantiate(arrow, new Vector3(Xposs + .5f, Yposs + .5f, 0), Quaternion.Euler(new Vector3(0, 0, Zrotation)));

        }
        
    }
    

    public void WriteVtable()
    {

        string filePath = Application.dataPath + "/StreamingAssets/Vtable.txt";

        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath)) ;
        }

        StreamWriter writer;
        writer = new StreamWriter(filePath, append: false);
        
        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);
            writer.WriteLine("V: " + state[i].StateValue + "   " + state[i].Action[MaxAction] + "   " + MaxAction);

        }
        
        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void WriteActions()
    {
        
        string filePath = Application.dataPath + "/StreamingAssets/Actions.txt";

        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath)) ;
        }

        StreamWriter writer;
        
        writer = new StreamWriter(filePath, append: false);

        for (int i = 0; i < action.Count; i++)
        {

            writer.WriteLine(action[i].value + "  " + action[i].time + "  " + action[i].state + "  " + action[i].action + "  " + action[i].reward);

        }


        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }

    public void WriteQtable()
    {

        string filePath = Application.dataPath + "/StreamingAssets/Qtable.txt";

        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath)) ;
        }

        StreamWriter writer;
        writer = new StreamWriter(filePath, append: false);


        for (int i = 0; i < state.Count; i++)
        {
            string MaxAction = GetMaxQ(i);

            writer.WriteLine("V: " + state[i].StateValue +"  Left: " + state[i].Action["L"] + "   Right: " + state[i].Action["R"]
                 + "   Up: " + state[i].Action["U"] + "   Down: " + state[i].Action["D"] + "   Up Left: " + state[i].Action["UL"]
                 + "   Up Right: " + state[i].Action["UR"] + "   Down Left: " + state[i].Action["DL"] + "   Down Right: " + state[i].Action["DR"]);
        }

        writer.WriteLine("");
        writer.Flush();
        writer.Close();

    }
}


[System.Serializable]
public class State
{

    public Dictionary<string, float> Action;
    public float Reward;
    public int StateValue;
    public string StateType;

    public State()
    {
        Action = new Dictionary<string, float>();
        
       
        Action.Add("L", 0);
        Action.Add("R", 0);
        Action.Add("U", 0);
        Action.Add("D", 0);
        Action.Add("UL", 0);
        Action.Add("UR", 0);
        Action.Add("DL", 0);
        Action.Add("DR", 0);
        
    }
}

[System.Serializable]
public class Action
{
    public int value;
    public double time;
    public int state;
    public string action;
    public float reward;


    public Action(int val, float t, int st, string act, float r)
    {
        value = val;
        time = System.Math.Round(t, 2);
        state = st;
        action = act;
        reward = r;
    }
}