using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardScript : MonoBehaviour
{
    public TMP_InputField TextField, NextTextField;
    public GameObject RusLayoutSml, RusLayoutBig, EngLayoutSml, EngLayoutBig, SymbLayout;
    public GameObject openedLayout;
    public GameObject closeButton;

    public void alphabetFunction(string alphabet)
    {
        UIFlowHandler.uihandler.isInputField = true;
        UIFlowHandler.uihandler.CancelInvoke();
        TextField.text = TextField.text + alphabet;
        UIFlowHandler.uihandler.SetCaret();
    }

    public void BackSpace()
    {
        UIFlowHandler.uihandler.isInputField = true;
        UIFlowHandler.uihandler.CancelInvoke();
        if (TextField.text.Length > 0) TextField.text = TextField.text.Remove(TextField.text.Length - 1);
        UIFlowHandler.uihandler.SetCaret();
    }

    public void CloseAllLayouts()
    {
        closeButton.SetActive(false);
        RusLayoutSml.SetActive(false);
        RusLayoutBig.SetActive(false);
        EngLayoutSml.SetActive(false);
        EngLayoutBig.SetActive(false);
        SymbLayout.SetActive(false);

    }

    public GameObject IsAnyLayoutOpened()
    {
        if (EngLayoutBig.activeInHierarchy)
        {
            return EngLayoutBig;
        }
        else if (EngLayoutSml.activeInHierarchy)
        {
            return EngLayoutSml;
        }
        else if (SymbLayout.activeInHierarchy)
        {
            return SymbLayout;
        }
        else return null;
    }

    public void ShowLayout(GameObject SetLayout)
    {
        CloseAllLayouts();
        SetLayout.SetActive(true);
        closeButton.SetActive(true);
        openedLayout = SetLayout;
    }

}