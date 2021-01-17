using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardDragHandler : MonoBehaviour, IDragHandler
{



    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        //not needed in ludo cash win project so disabled!
        /*transform.position = eventData.position; */
    }

    #endregion





}