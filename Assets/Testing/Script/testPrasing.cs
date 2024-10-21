using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class testPrasing : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SendFileToPrase();
        }

        void SendFileToPrase()
        {
            List<string> lines = FileManager.ReadTextAsset("testFile");

            foreach (string line in lines)
            {
                if (line == string.Empty)
                    continue;

                DIALOGUE_LINE dl = dialogueParser.Parse(line);
            }
        }
    }

}
