using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.BGM.Chapter1);
    }

}
