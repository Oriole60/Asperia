using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseUI : MonoBehaviour
{
    [SerializeField] GameObject uiContainer = null;

    // Start is called before the first frame update

    void Start()
    {
        uiContainer.SetActive(false);
    }
    //Event Signal Call
    public void Toggle()
    {
        uiContainer.SetActive(!uiContainer.activeSelf);
    }
}
