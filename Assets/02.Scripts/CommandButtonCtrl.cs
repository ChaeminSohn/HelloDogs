using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandButtonCtrl : MonoBehaviour
{
    enum Command
    {
        SIT, LIE, STAND
    }

    private GameObject mainDog;
    public int commandIdx = 0;
    private Command[] commandType = { Command.SIT , Command.LIE};
    [SerializeField]
    private TextMeshProUGUI commandText;


    private string[] commandTexts = { "Sit Down", "Lie Down"};

    void Update()
    {
        if(mainDog == null)
        {
            mainDog = GameObject.FindGameObjectWithTag("modelObject_Script");
        }
    }

    public void SwitchCommand(int num)
    {
        if(num == 0)
        {
            if (commandIdx == 0)
            {
                commandIdx = GameManager.friendLv - 1;
            }
            else
                commandIdx -= 1;
        }
        else if(num == 1)
        {
            if(commandIdx == GameManager.friendLv - 1)
            {
                commandIdx = 0;
            }
            else
                commandIdx = commandIdx + 1;
        }
        commandText.text = commandTexts[commandIdx];
    }

    public void GiveCommand()
    {
        if (mainDog != null)
        {
            switch (commandType[commandIdx])
            {
                case Command.SIT:
                    mainDog.GetComponentInChildren<DogCtrl>()?.GiveOrder("SitDown");
                    commandType[commandIdx] = Command.STAND;
                    commandTexts[commandIdx] = "Get Up";
                    break;
                case Command.LIE:
                    mainDog.GetComponentInChildren<DogCtrl>()?.GiveOrder("Lie");
                    commandType[commandIdx] = Command.STAND;
                    commandTexts[commandIdx] = "Get Up";
                    break;
                case Command.STAND:
                    mainDog.GetComponentInChildren<DogCtrl>()?.GiveOrder("Stand");
                    if (commandIdx == 0)
                    {
                        commandType[commandIdx] = Command.SIT;
                        commandTexts[commandIdx] = "Sit Down";
                    }
                    else if (commandIdx == 1)
                    {
                        commandType[commandIdx] = Command.LIE;
                        commandTexts[commandIdx] = "Lie Down";
                    }
                    break;
            }
            commandText.text = commandTexts[commandIdx];
        }

    }
}
