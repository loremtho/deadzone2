using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{

    public GameObject gmaePannel;
    public GameObject storePannel;
    public AudioClip button;

    // Start is called before the first frame update
    void Start()
    {
        storePannel.GetComponent<RectTransform>().localScale = Vector3.zero;

    }

    public void OnclickStore()
    {
        SoundManager.instance.SoundPlay("Button", button);
        storePannel.GetComponent<RectTransform>().localScale = Vector3.one;
        gmaePannel.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void Onclickback()
    {
        SoundManager.instance.SoundPlay("Button", button);
        storePannel.GetComponent<RectTransform>().localScale = Vector3.zero;
        gmaePannel.GetComponent<RectTransform>().localScale = Vector3.one;
    }

}
