using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public NpcData data;


    // Start is called before the first frame update
    void Initialize(NpcData data)
    {
        this.data = data;
        transform.GetChild(0).GetComponent<PolygonGenerator>().GenerateFullPolygon(data.edges, data.scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class NpcData
{
    public string npcName;
    public int health;
    public float speed;
    public int edges;
    public float scale;
}

[System.Serializable]
public class NpcDialogueData
{
    public string npcName;
    public List<DialogueEntry> entries;
}

[System.Serializable]
public class DialogueEntry
{
    public string title;
    [TextArea(15,20)]
    public string entry;
}

