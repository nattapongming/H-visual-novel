using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using DIALOGUE;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //Character Sora = CharacterManager.instance.CreateCharacter("Sora");*/
            StartCoroutine(Test());
            //Test2();
            
        }

        IEnumerator Test()
        {
            Character Helena = CharacterManager.instance.CreateCharacter("Helena");

            List<string> lines = new List<string>()
            {
                "Hi",
                "Sup"
            };
            //yield return DialogueSystem.instance.Say(lines);
            yield return Helena.Say(lines);
            //yield return null;

        }
        private void Test2()
        {
            Character Helena = CharacterManager.instance.CreateCharacter("Helena");

            List<string> lines = new List<string>()
            {
                "Hi",
                "Sup"
            };
            //Helena.Say(lines);
            Debug.Log("Complete");
        }
    }
}