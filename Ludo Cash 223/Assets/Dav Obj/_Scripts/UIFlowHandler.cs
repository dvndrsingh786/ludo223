using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UIFlowHandler : MonoBehaviour
{
    [SerializeField] EventSystem system;
    InputField old;
    TouchScreenKeyboard keyboard;
    [SerializeField] GameObject dateOfBirthIP;
    [SerializeField] ScrollRect scroll;
    public static UIFlowHandler uihandler;
    public KeyboardScript keyboard1;
    [SerializeField] GameObject smallKeyboard, symbolKeyboard;
    [SerializeField] int currentIndex = 0;
    [SerializeField] TMP_InputField[] theFields;
    public bool isInputField = false;
    public GameObject updateAppPopup;
    public GameObject loadingPanel;

    public Texture THETEXTURE;

    public Hashtable FieldType = new Hashtable() {
        {3,"0"},  {4,"1"},{5,"1"},{6,"1"}
    };

    void Start()
    {
        if (uihandler == null) uihandler = this;
        else if (uihandler != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerImage()
    {
        GameManager.Instance.initMenuScript.playerAvatarDav.GetComponent<RawImage>().texture = THETEXTURE;
    }

    public Sprite TextureToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void UpdateApp()
    {
        updateAppPopup.SetActive(true);
    }

    /// <summary>
    /// Opens Provided URL/Link
    /// </summary>
    public void OpenLink(string url)
    {
        Application.OpenURL(url);
        //Game link is in GameManager.appLink
    }

    public void IsInputField()
    {
        isInputField = true;
        CancelInvoke();
    }

    void Update()
    {
        if (GameManager.Instance!=null)
        {
            //Debug.LogError("1"+GameManager.playerName);
            //Debug.LogError("2"+GameManager.Instance.nameMy);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (keyboard1.IsAnyLayoutOpened() != null)
            {
                keyboard1.closeButton.SetActive(false);
                keyboard1.openedLayout.SetActive(false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Invoke(nameof(CustomUpdate), 0.2f);
        }
    }

    void CustomUpdate()
    {
        if (!isInputField)
        {
            keyboard1.CloseAllLayouts();
        }
        else isInputField = false;
    }

    public void SetCurrentInputField(TMP_InputField currentField)
    {
        isInputField = true;
        keyboard1.TextField = currentField;
    }
    public void OpenSpecificKeyboard(int code)
    {
        if (code == 0) keyboard1.ShowLayout(smallKeyboard);
        else keyboard1.ShowLayout(symbolKeyboard);
    }
    public void SetNextInputField(TMP_InputField nextField)
    {
        if (nextField != null)
            keyboard1.NextTextField = nextField;
    }

    public void SetTheIndex(int currentInd)
    {
        currentIndex = currentInd;
        if (currentIndex == 3 || currentIndex == 4 || currentIndex == 5 || currentIndex == 6 || currentIndex == 7)
        {
            scroll.verticalNormalizedPosition = 0;
        }
        else scroll.verticalNormalizedPosition = 1;
        StartCoroutine(CaretSetter());
    }

    public void OnDonee()
    {
        isInputField = true;
        if (theFields[currentIndex + 1] != null)
        {
            currentIndex++;
            if (FieldType.ContainsKey(currentIndex)) keyboard1.ShowLayout(symbolKeyboard);
            else keyboard1.ShowLayout(smallKeyboard);
            keyboard1.TextField = theFields[currentIndex];
            theFields[currentIndex].ActivateInputField();
            StartCoroutine(CaretSetter());
        }
        else
        {
            keyboard1.CloseAllLayouts();
        }
    }

    public void SetCaret()
    {
        StartCoroutine(CaretSetter());
    }

    IEnumerator CaretSetter()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            theFields[currentIndex].ActivateInputField();
            theFields[currentIndex].caretPosition = theFields[currentIndex].text.Length;
            theFields[currentIndex].ForceLabelUpdate();
        }
        catch
        {
            Debug.LogError("Caret Catch");
        }
    }


    public void OnDone(InputField field)
    {
        if (keyboard.status == TouchScreenKeyboard.Status.Done)
        {
            if (old) old.DeactivateInputField();
            field.GetComponent<Selectable>().Select();
            field.ActivateInputField();
            system.SetSelectedGameObject(field.gameObject);
            old = field;
        }
    }

    public void OnSelect(int code)
    {
        if (system.currentSelectedGameObject == dateOfBirthIP)
        {
            scroll.verticalNormalizedPosition = 0;
        }
        if (code == 0)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
        else if (code == 1)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        }
        else if (code == 2)
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);
        }
    }
}