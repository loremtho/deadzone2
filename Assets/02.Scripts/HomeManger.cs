using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManger : MonoBehaviour
{

    public GameObject gmaePannel;
    public GameObject storePannel;

    // Start is called before the first frame update
    void Start()
    {
        storePannel.GetComponent<RectTransform>().localScale = Vector3.zero;

    }

    public void OnclickStore()
    {
        storePannel.GetComponent<RectTransform>().localScale = Vector3.one;
        gmaePannel.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void Onclickback()
    {
        storePannel.GetComponent<RectTransform>().localScale = Vector3.zero;
        gmaePannel.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
