using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class CommandTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                CommandManager.Instance.Execute("moveCharDemo", "left");
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                CommandManager.Instance.Execute("moveCharDemo", "right");
        }

        IEnumerator Running()
        {
            /*yield return CommandManager.Instance.Execute("print");
            yield return CommandManager.Instance.Execute("print_1p", "Hellow World!");
            yield return CommandManager.Instance.Execute("print_mp", "Line1", "Line2", "Line3");*/

            /*yield return CommandManager.Instance.Execute("lambda");
            yield return CommandManager.Instance.Execute("lambda_1p", "Hello Lambda!");
            yield return CommandManager.Instance.Execute("lambda_mp", "Lambda1", "Lambda2", "Lambda3");*/

            /*yield return CommandManager.Instance.Execute("process");
            yield return CommandManager.Instance.Execute("process_1p", "3");
            yield return CommandManager.Instance.Execute("process_mp", "Process Line 1", "Process Line 2", "Process Line 3");*/

            yield return null;
        }
    }
}