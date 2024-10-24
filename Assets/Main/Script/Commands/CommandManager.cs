using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }

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

            foreach(Type extension in extensionTypes)
            {
                //extcute method that name "Extend" in each of assembly
                MethodInfo extendMethod = extension.GetMethod("Extend");
                extendMethod.Invoke(null, new object[] { dataBase });
            }
        } else
            DestroyImmediate(gameObject);
        

    }

    public void Execute(string commandName)
    {
        Delegate command = dataBase.GetCommand(commandName);

        if (command != null)
            command.DynamicInvoke();
    }
}
