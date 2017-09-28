using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security.Permissions;

public class Policy : MonoBehaviour{

    public static Policy Instance { get; private set; }

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
        }
        
        Instance = this;
        
    }


    public PolicyStates policyState;


    public void SavePolicy()
    {
        string filePath = Application.dataPath + "/StreamingAssets/XML/Policy.xml";

        //overWritten file//
        File.SetAttributes(filePath, FileAttributes.Normal);
        FileIOPermission filePermission =
                 new FileIOPermission(FileIOPermissionAccess.AllAccess, filePath);
        //****//

        XmlSerializer serializer = new XmlSerializer(typeof(PolicyStates));

        FileStream stream = new FileStream(filePath, FileMode.Create);

        serializer.Serialize(stream, policyState);

        stream.Close();
    }
    public void LoadPolicy()
    {
        string filePath = Application.dataPath + "/StreamingAssets/XML/Policy.xml";

        XmlSerializer serializer = new XmlSerializer(typeof(PolicyStates));

        FileStream stream = new FileStream(filePath, FileMode.Open);

        policyState =  serializer.Deserialize(stream) as PolicyStates;

        stream.Close();
    }
}

[System.Serializable]
public class PolicyStates
{
    public List<PolicyValues> state = new List<PolicyValues>();
}

[System.Serializable]
public class PolicyValues
{
    public int value;
    public string action;
}