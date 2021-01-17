using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScalerAnimationHandler : MonoBehaviour
{

    readonly float reducingValue = 0.25f;
    readonly float reducingValueForUI = 0.25f;
    static ScalerAnimationHandler _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            return;
        }

        Destroy(this.gameObject);
    }

    public static ScalerAnimationHandler Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpScale(GameObject targetObj)
    {
        GameObject ButtonUIObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        targetObj.transform.localScale = new Vector2(0.5f, 0.5f);
        IEnumerator coroutine = UpScalingAnimation(targetObj, new Vector2(1, 1), reducingValue);
        StartCoroutine(ScalingEffect(targetObj, ButtonUIObj, coroutine, true));
    }

    public void DeScale(GameObject targetObj)
    {
        GameObject ButtonUIObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        IEnumerator coroutine = DeScalingAnimation(targetObj, new Vector2(0.5f, 0.5f), reducingValue);
        StartCoroutine(ScalingEffect(targetObj, ButtonUIObj, coroutine, false));
    }

    IEnumerator ScalingEffect(GameObject targetObj, GameObject ButtonUIObj, IEnumerator coroutine, bool toOpen)
    {
        if (ButtonUIObj != null)
        {
            yield return StartCoroutine(DeScalingAnimation(ButtonUIObj, new Vector2(0.95f, 0.95f), reducingValueForUI));
            yield return StartCoroutine(UpScalingAnimation(ButtonUIObj, new Vector2(1f, 1f), reducingValueForUI));
        }
        if (ButtonUIObj != null)
            ButtonUIObj.transform.localScale = new Vector3(1, 1, 1);

        targetObj.GetComponent<Canvas>().enabled = true;
        yield return StartCoroutine(coroutine);
        if (!toOpen)
            targetObj.GetComponent<Canvas>().enabled = false;
    }

    IEnumerator UpScalingAnimation(GameObject obj, Vector2 targetScale, float value)
    {

        while (obj.transform.localScale.x < targetScale.x)
        {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x + value, obj.transform.localScale.y + value, 1);
            yield return null;
        }
        obj.transform.localScale = Vector3.one;
    }

    IEnumerator DeScalingAnimation(GameObject obj, Vector2 targetScale, float value)
    {
        while (obj.transform.localScale.x > targetScale.x)
        {
            obj.transform.localScale = new Vector3(obj.transform.localScale.x - value, obj.transform.localScale.y - value, 1);
            yield return null;
        }
        obj.transform.localScale = Vector3.zero;

    }
}
