using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTableConfiguration : MonoBehaviour
{
    #region Singleton

    private static GameTableConfiguration _instance;
    public static GameTableConfiguration Instance => _instance;

    private void Awake()
    {
        if (!_instance == null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public string Key;

    public void SetTableValue(string name)
    {
        Key =name ;
    }
}
