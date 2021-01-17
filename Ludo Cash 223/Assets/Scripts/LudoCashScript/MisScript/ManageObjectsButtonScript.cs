using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageObjectsButtonScript : MonoBehaviour
{
    [Header("Image Attribute")]

    public Image[] buttons;
    public Sprite selectImage;
    public Sprite unselectImage;
    private int ButtonLength;

    private void Start()
    {
        ButtonLength = buttons.Length;
        AccessButton(0);
    }

    void DisableAllButton()
    {
        for (int i = 0; i < ButtonLength; i++)
        {
            buttons[i].sprite = unselectImage ;
        }
    }
   public void AccessButton( int buttonnumber)
    {
        DisableAllButton();
        buttons[buttonnumber].sprite = selectImage ;

    }
    
}
