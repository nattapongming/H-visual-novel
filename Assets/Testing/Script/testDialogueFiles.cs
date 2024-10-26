using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using CHARACTER;

namespace TESTING
{
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
            //Character Helena = CharacterManager.instance.CreateCharacter("Helena");
            List<string> lines = FileManager.ReadTextAsset(fileToRead);
            //Helena.Say(lines);
            DialogueSystem.instance.Say(lines);
        }
    }
}