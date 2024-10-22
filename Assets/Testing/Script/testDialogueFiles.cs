using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class testDialogueFiles : MonoBehaviour
{
    [SerializeField] private TextAsset fileToRead = null;

    void Start()
    {
        StartConversation();
    }

    //Read text asset and send to Dialogue System
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(fileToRead);

        DialogueSystem.instance.Say(lines);

        //DialogueSystem.instance.Say(lines);
    }
}
