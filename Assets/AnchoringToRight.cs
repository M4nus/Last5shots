using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchoringToRight : MonoBehaviour
{
    Camera gui;

    private void Awake()
    {
        gui = transform.parent.gameObject.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var v3Pos = new Vector3(0.1f, -12.5f, 0.1f);
        transform.position = gui.GetComponent<Camera>().ViewportToWorldPoint(v3Pos);
    }
}
