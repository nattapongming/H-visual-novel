using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class testDialogueFiles : MonoBehaviour
{
    
    void Start()
    {
        StartConversation();
    }

    //Read text asset and send to Dialogue System
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("testFile");

        DialogueSystem.instance.Say(lines);
    }
}
