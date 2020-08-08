using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    TextMeshProUGUI counter;
    float time;

    // Start is called before the first frame update
    void Awake()
    {
        counter = GetComponent<TextMeshProUGUI>();
        counter.text = "0.0";
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0.8f)
        {
            time += Time.fixedDeltaTime / 4f;
            counter.text = time.ToString("f1");
        }
    }
}
