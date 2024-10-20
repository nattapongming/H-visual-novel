using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingArchitect : MonoBehaviour
{
    DialogueSystem ds;
    TextArchitect architect;

    string[] lines = new string[]
    {
        "This is a random line of dialogue.",
        "I want to say somethings, come over here.",
        "The world is crazy place sometime.",
        "Don't lose hope, Things will get better!",
        "Tt's a bird? It's a plane? No - ! It's an Ender Dragon!"
    };

    // Start is called before the first frame update
    void Start()
    {
        //Get dialogue sytem
        ds = DialogueSystem.instance;
        //Create new architext to customize buildmethod and speed etc.
        architect = new TextArchitect(ds.dialogueContainer.dialogueText);
        architect.buildMethod = TextArchitect.BuildMethod.fade;
        //architect.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // press to create text
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(architect.isBuilding)
            {
                if (!architect.hurryUp) { architect.hurryUp = true; }
                else { architect.ForceComplete(); }
            }
            else
            {
                architect.Build(lines[Random.Range(0, lines.Length)]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            architect.Append(lines[Random.Range(0, lines.Length)]);
        }
    }
}
