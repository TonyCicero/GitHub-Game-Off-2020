using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class console : MonoBehaviour
{
    [SerializeField]
    Text Monitor;
    private int monitorSize = 7; // number of lines

    public consoleVars cVars;
    public bool UsingTerminal;

    computerAudio audioMngr;

    [System.Serializable]
    public class commands
    {
        public string inputCmd; //cmd keyword
        public int reqArgs; // number of required arguments
        public string output; //optional output text to be displayed
    }

    public class LaunchChecklist
    {
        public readonly string req_sysState = "11101";
        public bool passDiag;
        // binary representation of activated systems
        //1=active;0=inactive; order= FuelPump>Engine>Navigation>Solar>Communication
        //example: 11111 = All systems active
        public bool sys_State; // true if state = 11101
        public bool Fuel;
        public bool Batt;
        public bool Passed; //is ready for launch
        
        
    }
    LaunchChecklist checklist;

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
            case "CHECKLIST":
                output = CHECKLISTcmd();
                break;
            case "DIAGNOSTIC":
                output = DIAGNOSTICcmd();
                break;
            case "HELP":
                output = HELPcmd(args);
                break;
            default:
                return "Invalid Command! for help type 'HELP'\n";
        }


        return output;
    }

    string SETcmd(string[] args)
    {
        if (args.Length > 1)
        {
            switch (args[1])
            {
                case "Throttle":
                    int val = int.Parse(args[2]);
                    if (val >= 0 && val <= 100)
                    {
                        cVars.Throttle = val;
                        return "Throttle set to " + val + "%\n";
                    }
                    else
                    {
                        return "Invalid Value. Must be between 0 and 100\n";
                    }
                case "FuelPump":
                    if (args[2] == "ON")
                    {
                        cVars.FuelPump = true;
                        return "Fuel Pump set to ON\n";
                    }else if (args[2] == "OFF")
                    {
                        cVars.FuelPump = false;
                        return "Fuel Pump set to OFF\n";
                    }
                    else
                    {
                        return "Invalid Input. Expecting 'ON' or 'OFF'";
                    }
                case "Engine":
                    if (args[2] == "ON")
                    {
                        cVars.Engine = true;
                        return "Engine set to ON\n";
                    }
                    else if (args[2] == "OFF")
                    {
                        cVars.Engine = false;
                        return "Engine set to OFF\n";
                    }
                    else
                    {
                        return "Invalid Input. Expecting 'ON' or 'OFF'";
                    }
                case "Navigation":
                    if (args[2] == "ON")
                    {
                        cVars.Navigation = true;
                        return "Navigation system set to ON\n";
                    }
                    else if (args[2] == "OFF")
                    {
                        cVars.Navigation = false;
                        return "Navigation system set to OFF\n";
                    }
                    else
                    {
                        return "Invalid Input. Expecting 'ON' or 'OFF'";
                    }
                case "Solar":
                    if (args[2] == "ON")
                    {
                        cVars.Solar = true;
                        return "Solar set to ON\n";
                    }
                    else if (args[2] == "OFF")
                    {
                        cVars.Solar = false;
                        return "Solar set to OFF\n";
                    }
                    else
                    {
                        return "Invalid Input. Expecting 'ON' or 'OFF'";
                    }
                    case "Communication":
                    if (args[2] == "ON")
                    {
                        cVars.Communication = true;
                        return "Communication system set to ON\n";
                    }else if (args[2] == "OFF")
                    {
                        cVars.Communication = false;
                        return "Communication system set to OFF\n";
                    }
                    else
                    {
                        return "Invalid Input. Expecting 'ON' or 'OFF'";
                    }
                default:
                    return "Invalid Variable\n";
            }
        }
        else
        {
            return "<b>No Variable specified!</b>\n";
        }
    }

    string GETcmd(string[] args)
    {
        if (args.Length > 1)
        {
            switch (args[1])
            {
                case "Throttle":
                    return "Throttle is currently at " + cVars.Throttle + "%\n";
                case "Health":
                    return "Ship Health is currently at " + cVars.Health + "\n";
                case "Fuel":
                    return "Fuel level is currently at " + cVars.Fuel + "\n";
                case "Battery":
                    return "Battery level is currently at " + cVars.Battery + "\n";
                case "gForce":
                    return "Gravitational Force is currently " + cVars.gForce + "N\n";

                default:
                    return "Invalid Variable\n";
            }
        }
        else
        {
            return "<b>No Variable specified!</b>\n";
        }
    }

    string STATUScmd()
    {
        string sys_state = string.Format("{0}{1}{2}{4}", cVars.FuelPump, cVars.Engine, cVars.Navigation, cVars.Communication, cVars.Solar);
        sys_state = sys_state.Replace("True", "1");
        sys_state = sys_state.Replace("False", "0");
        if (sys_state == checklist.req_sysState) //check if state meet requirement for Launch
        {
            checklist.sys_State = true;
        }
        string output = string.Format("System Status:\nFuel Pump: {0} \nEngine: {1} \nNavigation: {2} \nCommunication: {3} \nSolar: {4} \nState = {5}",cVars.FuelPump ,cVars.Engine,cVars.Navigation,cVars.Communication,cVars.Solar,sys_state);
        output = output.Replace("True", "<color=green>Active</color>");
        output = output.Replace("False", "<color=red>Inactive</color>");
        return output +"\n";
    }

    void CLEARcmd() //clears console monitor
    {
       
        curMonitor = "";
        Monitor.text = curMonitor;
    }

    string CHECKLISTcmd() // displays laucnch checklist
    {
        string output = string.Format("Launch Checklist:\nDiagnostics: {0} \nFuel: {1} \nBattery: {2} \nSystems Status: {3} \n", checklist.passDiag,checklist.Fuel,checklist.Batt,checklist.sys_State);
        output = output.Replace("True", "<color=green>Passed</color>");
        output = output.Replace("False", "<color=red>Failed</color>");
        if (checklist.passDiag && checklist.Fuel && checklist.Batt && checklist.sys_State)
        {
            checklist.Passed = true;
            output += "<color=green>Ready for launch</color>\n";
        }
        else
        {
            audioMngr.playClip(0);
            checklist.Passed = false;
            output += "<color=red>Not Ready for Launch</color>\n";
            if (!checklist.sys_State)
            {
                output += "<color=red>System State must equal " + checklist.req_sysState + "</color>\n";
            }
        }
        return output;
    }

    string DIAGNOSTICcmd()
    {
        checklist.passDiag = true;
        return "<color=green>No Problems were detected</color>\n";
    }

    string HELPcmd(string[] args)
    {
        if (args.Length > 1)
        {
            switch (args[1])
            {
                case "SET":
                    return "<b>SET Command:</b> sets value for a given variable\n" +
                        "Syntax: SET <var> <value>\n" +
                        "Variables: 'Throttle','FuelPump','Engine','Navigation','Solar','Communication'\n";

                case "GET":
                    return "<b>GET Command:</b> gets value for a given variable\n" +
                        "Syntax: SET <var> \n" +
                        "Variables: 'Throttle','Health','Fuel','Battery','gForce'\n";

                case "STATUS":
                    return "<b>STATUS Command:</b> gets statuses of ship systems \n" +
                        "Syntax: STATUS\n";

                case "CLEAR":
                    return "<b>CLEAR Command:</b> clears all contents on screen\n" +
                        "Syntax: CLEAR\n";
                case "DIAGNOSTIC":
                    return "<b>DIAGNOSTIC Command:</b> runs system diagnostics test\n" +
                        "Syntax: DIAGNOSTIC\n";
                case "CHECKLIST":
                    return "<b>CHECKLIST Command:</b> runs Launch Checklist\n" +
                        "Syntax: CHECKLIST\n";
                default:
                    return "<b>Command not found!</b> Available commands:'SET','GET','STATUS','CLEAR','DIAGNOSTIC','CHECKLIST'\n";
            }
        }
        else
        {
            return "<b>HELP Command:</b> find how to use commands\n" +
                "Syntax: HELP <command> \n" +
                "Commands: 'SET','GET','STATUS','CLEAR','DIAGNOSTIC','CHECKLIST'\n";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checklist = new LaunchChecklist();
        audioMngr = GetComponent<computerAudio>();
    }

    string fit2Screen(string s)
    {
        string[] Lines = s.Split('\n');
        int numLines = Lines.Length;
        if (numLines< monitorSize) 
        {
            return s; //make no changes
        }
        int startIndex = numLines - monitorSize;
        string result = "";
        for (int i = startIndex; i < numLines; i++)
        {
            result += Lines[i] + "\n";
        }

        return result;
    }

    IEnumerator readInput(char c)
    {

        if (c == '\b') //backspace key
        {
            Debug.Log("backspace");
            if (curMonitor.Length > 0) { curMonitor = curMonitor.Substring(0, curMonitor.Length - 1); }
            if (UserInput.Length > 0) { UserInput = UserInput.Substring(0, UserInput.Length - 1); }
        }
        else if (c == '\n' || c == '\r')//enter or return key hit
        {
            string result = "\n" + execute(UserInput); // execute cmd & display output
            curMonitor += result;
            curMonitor = fit2Screen(curMonitor);
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
