using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineGameStartScript : MonoBehaviour
{
    [SerializeField] Button oldPlayButton;

    public void CheckValidation()
    {
        if (!ReferenceManager.refMngr.isOnlineBidSelected)
        {
            ReferenceManager.refMngr.ShowError("Please select bid amount first");
            return;
        }
        ReferenceManager.refMngr.isOnlineBidSelected = false;
        oldPlayButton.onClick.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
