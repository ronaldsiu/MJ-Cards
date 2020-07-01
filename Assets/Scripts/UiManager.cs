using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public Text instruction;
    public static UiManager instance = null;

    void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
