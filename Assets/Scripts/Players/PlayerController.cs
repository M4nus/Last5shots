using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerType
{
    James,
    John
}

public class PlayerController : MonoBehaviour, InputMaster.IPlayerActions
{
    [SerializeField] private PlayerType playerType = new PlayerType();
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private Camera camera;
    [SerializeField] private float playerMoveSpeed = 300f;
    [SerializeField] private int maxBulletCount = 5;

    private InputMaster inputControls;
    private GameObject James;
    private GameObject John;
    private Rigidbody rbJames;
    private Rigidbody rbJohn;
    private Vector2 direction;
    private List<GameObject> bullets = new List<GameObject>();

    #region Declaration

    void OnEnable()
    {
        if(inputControls == null)
        {
            inputControls = new InputMaster();
            inputControls.Player.SetCallbacks(this);
        }
        inputControls.Player.Enable();

        James = GameObject.FindGameObjectWithTag("James");
        John = GameObject.FindGameObjectWithTag("John");

        if(James != null)
            rbJames = James.GetComponent<Rigidbody>();
        if(John != null)
            rbJohn = John.GetComponent<Rigidbody>();
    }

    void OnDisable()
    {
        inputControls.Disable();
    }

    void Update()
    {
        RotateCharacter();
    }

    #endregion

    #region Behaviours

    public void OnMovement(InputAction.CallbackContext context)
    {
        if(playerType == PlayerType.James)
        {
            StartCoroutine(MoveContinuously(context));
        }
        else if(playerType == PlayerType.John)
        {
            StartCoroutine(MoveContinuously(context));
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(playerType == PlayerType.James && maxBulletCount >= bullets.Count)
            {
                CheckForCollision();
            }
            else if(playerType == PlayerType.John)
            {
                Debug.Log("John Shoot!");
            }
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(playerType == PlayerType.James && bullets.Count >= 0)
            {
                bullets[bullets.Count - 1].SetActive(false);
                bullets.RemoveAt(bullets.Count - 1);
            }
            else if(playerType == PlayerType.John)
            {
                Debug.Log("John Rewind!");
            }
        }
    }

    #endregion

    #region Functionalities
    
    private IEnumerator MoveContinuously(InputAction.CallbackContext context)
    {
        while(context.performed || context.started || context.canceled)
        {
            direction = context.ReadValue<Vector2>();
            rbJames.velocity = new Vector3(-direction.y * playerMoveSpeed * Time.fixedDeltaTime, 0f, direction.x * playerMoveSpeed * Time.fixedDeltaTime);
            yield return null;
        }
    }

    public void RotateCharacter()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Ta Daaa
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void CheckForCollision()
    {
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        RaycastHit hit;
        if(Physics.Raycast(bulletSpawnPoint.transform.position, transform.TransformDirection(-Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            GameObject bullet = ObjectPooler.sharedInstance.GetPooledObject();
            if(bullet != null)
            {
                bullet.transform.position = (James.transform.position + hit.point) / 2;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.rotation *= Quaternion.Euler(0, 90f, 0);
                bullet.SetActive(true);
                bullets.Add(bullet);
                NewScale(bullet, hit.distance);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit ");
        }
    }

    public void NewScale(GameObject theGameObject, float newSize)
    {

        float size = theGameObject.GetComponentInChildren<Renderer>().bounds.size.x;

        Vector3 rescale = theGameObject.transform.localScale;

        rescale.x = newSize * rescale.x / size;

        theGameObject.transform.localScale = rescale;
    }

    #endregion
}
