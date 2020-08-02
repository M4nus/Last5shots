using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    James,
    John
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType = new PlayerType();
    private GameObject James;
    private GameObject John;

    void Start()
    {
        James = GameObject.FindGameObjectWithTag("James");
        John = GameObject.FindGameObjectWithTag("John");
    }

    public void MoveCharacter()
    {
        if(playerType == PlayerType.James)
        {

        }
        else if(playerType == PlayerType.John)
        {

        }
    }
}
