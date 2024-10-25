using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class testFiles : MonoBehaviour
    {
        [SerializeField] private TextAsset fileName;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            //Pass file name and include blank line to file Manager to read


            List<string> lines = FileManager.ReadTextAsset(fileName, true);

            foreach (string line in lines)
            {
                Debug.Log(line);
            }

            yield return null;
        }
    }
}