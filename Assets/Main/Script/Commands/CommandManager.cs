using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.Xml.Serialization;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set; }
        private static Coroutine process = null;
        public static bool isRunningProcess => process != null;

        private CommandDataBase dataBase;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                dataBase = new CommandDataBase();

                //get infomation of current executing code
                Assembly assembly = Assembly.GetExecutingAssembly();

                //Get command of child class of CMD_DatabaseExtension
                Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray();

                foreach (Type extension in extensionTypes)
                {
                    //extcute method that name "Extend" in each of assembly
                    MethodInfo extendMethod = extension.GetMethod("Extend");
                    extendMethod.Invoke(null, new object[] { dataBase });
                }
            }
            else
                DestroyImmediate(gameObject);


        }

        //any other string beside commandName will count as arguments array
        public Coroutine Execute(string commandName, params string[] args)
        {
            Delegate command = dataBase.GetCommand(commandName);

            if (command == null)
                return null;

            return StartProcess(commandName, command, args);
        }

        public Coroutine StartProcess(string commandName, Delegate command, string[] args)
        {
            StopCurrentProcess();

            process = StartCoroutine(RunnigProcess(command, args));

            return process;
        }

        private void StopCurrentProcess()
        {
            if (process != null)
                StopCoroutine(process);

            process = null;
        }

        private IEnumerator RunnigProcess(Delegate command, string[] args)
        {
            yield return WaitingForProcessToComplete(command, args);

            process = null;
        }

        //check for type of command
        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
                command.DynamicInvoke();

            else if (command is Action<string>)
                command.DynamicInvoke(args[0]);

            else if (command is Action<string[]>)
                command.DynamicInvoke((object)args);

            //if it's func convert too
            else if (command is Func<IEnumerator>)
                yield return ((Func<IEnumerator>)command)();

            else if (command is Func<string, IEnumerator>)
                yield return ((Func<string, IEnumerator>)command)(args[0]);

            else if (command is Func<string[], IEnumerator>)
                yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }
}