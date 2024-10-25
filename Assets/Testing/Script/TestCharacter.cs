using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Character Helena = CharacterManager.instance.CreateCharacter("Helena");
            Character Sora = CharacterManager.instance.CreateCharacter("Sora");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}