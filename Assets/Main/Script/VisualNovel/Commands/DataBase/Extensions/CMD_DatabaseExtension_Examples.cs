using COMMANDS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
    {
        //Override parent extend method
        new public static void Extend(CommandDataBase database)
        {
            //Add action with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            //Add lambda with no parameters
            // lambda is a funtion that work without using method or use reference
            database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command."); }));
            database.AddCommand("lambda_p1", new Action<string>((arg) => { Debug.Log($"Log user lamba message: '{arg}'"); }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            //Add coroutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MutliLineProcess));

            //special example
            database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));
        }

        //all of command must be static

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console.");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User message '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }
        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 1; i <= 5; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }

        }

        private static IEnumerator MutliLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: '{line}'");
                yield return new WaitForSeconds(0.5f);
            }

        }

        private static IEnumerator MoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            //Find wanted image
            Transform character = GameObject.Find("Image").transform;
            float moveSpeed = 15;

            //check with side image are on
            float targetX = left ? -8 : 8;

            //get current image position
            float currentX = character.position.x;

            //move image gradually toward taget position
            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }


    }
}