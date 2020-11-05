using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class console : MonoBehaviour
{
    [SerializeField]
    Text Monitor;

    public consoleVars cVars;
    public bool UsingTerminal;

    [System.Serializable]
    public class commands
    {
        public string inputCmd; //cmd keyword
        public int reqArgs; // number of required arguments
        public string output; //optional output text to be displayed
    }

    public commands[] cmds;

    [SerializeField]
    string UserInput = "";
    [TextArea]
    [SerializeField]
    string curMonitor = ""; //current contents of terminal monitor

    string execute(string cmd)
    {
        string output = "";
        string[] args = cmd.Split(' ');


        switch (args[0])
        {
            case "SET":
                output = SETcmd(args);
                break;
            case "GET":
                output = GETcmd(args);
                break;
            case "STATUS":
                output = STATUScmd();
                break;
            case "CLEAR":
                CLEARcmd();
                break;
            case "HELP":
                break;
            default:
                return "Invalid Command! for help type 'HELP'\n";
        }


        return output;
    }

    string SETcmd(string[] args)
    {
        return null;
    }

    string GETcmd(string[] args)
    {
        return null;
    }

    string STATUScmd()
    {
        string output = string.Format("System Status: \nEngine: {0} \nNavigation: {1} \nCommunication: {2} \nSolar: {3}",cVars.Engine,cVars.Navigation,cVars.Communication,cVars.Solar);
        output = output.Replace("True", "<color=green>Active</color>");
        output = output.Replace("False", "<color=red>Inactive</color>");
        return output +"\n";
    }

    void CLEARcmd() //clears console monitor
    {
       
        curMonitor = "";
        Debug.Log(curMonitor);
        Monitor.text = curMonitor;
    }

    string HELPcmd()
    {
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator readInput(char c)
    {

        if (c == '\b') //backspace key
        {
            curMonitor = curMonitor.Substring(0, curMonitor.Length - 1);
            UserInput = UserInput.Substring(0, UserInput.Length - 1);
        }
        else if (c == '\n' || c == '\r')//enter or return key hit
        {
            string result = "\n" + execute(UserInput); // execute cmd & display output
            curMonitor += result;
            UserInput = "";
        }
        else
        {
            UserInput += c;
        }
        curMonitor += c;
        Monitor.text = curMonitor; //update the monitor

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (UsingTerminal) //check if user is using the terminal
        {
            foreach (char c in Input.inputString) //get user input
            {
                StartCoroutine(readInput(c));
               
            }
        }

    }

}
