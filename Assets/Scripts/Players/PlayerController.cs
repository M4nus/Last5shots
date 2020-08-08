using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public enum PlayerType
{
    James,
    John
}

public class PlayerController : MonoBehaviour, InputMaster.IPlayerActions
{
    public static PlayerController instance;

    [SerializeField] private PlayerType playerType = new PlayerType();
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private Camera camera;
    [SerializeField] private float playerMoveSpeed = 300f;
    [SerializeField] private int maxBulletCount = 5;

    private InputMaster inputControls;
    private ParticleContainer pc;
    private GameObject James;
    private GameObject John;
    private Rigidbody rbJames;
    private Rigidbody rbJohn;
    private Vector2 direction;
    public List<GameObject> bullets = new List<GameObject>();
    public UnityAction onRewind;
    public UnityAction onJamesDeath;
    public CameraShake cameraShake;

    ParticleSystem electricity;
    ParticleSystem laserSpawn;

     AudioSource audioClip;
    AudioSource rewindClip;

    #region Declaration

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        audioClip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>();
        rewindClip = GetComponent<AudioSource>();
    }

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
        pc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParticleContainer>();

        if(James != null)
            rbJames = James.GetComponent<Rigidbody>();
        if(John != null)
            rbJohn = John.GetComponent<Rigidbody>();

        onRewind += null;
        onJamesDeath += null;
        
    }

    void OnDisable()
    {
        pc.rewind.Play();
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
                audioClip.Play();
                StartCoroutine(cameraShake.Shake(0.1f, 0.1f));
                if(LaserIndicator.instance.onLaserUsed != null)
                    LaserIndicator.instance.onLaserUsed.Invoke();
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
            if(playerType == PlayerType.James && bullets.Count >= 1)
            {
                GameObject rewind = ObjectPooler.sharedInstance.GetPooledObject("Rewind");
                if(rewind != null)
                {
                    rewind.transform.position = bullets[bullets.Count - 1].transform.position;
                    rewind.transform.rotation = bullets[bullets.Count - 1].transform.rotation;
                    rewind.SetActive(true);
                    var shRewind = rewind.GetComponent<ParticleSystem>().shape;
                    shRewind.scale = new Vector3(bullets[bullets.Count - 1].transform.localScale.x / 10f, 0.1f, 0.1f);
                }

                if(GameObject.FindGameObjectsWithTag("RedBullet").Length > 0 && onRewind != null)
                    onRewind.Invoke();

                bullets[bullets.Count - 1].SetActive(false);
                bullets.RemoveAt(bullets.Count - 1);
            }
            else if(playerType == PlayerType.John)
            {
                Debug.Log("John Rewind!");
            }
            rewindClip.Play();
            StartCoroutine(cameraShake.Shake(0.04f, 0.04f));
            if(LaserIndicator.instance.onLaserRestored != null)
                LaserIndicator.instance.onLaserRestored.Invoke();
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
            GameObject bullet = ObjectPooler.sharedInstance.GetPooledObject("BlueBullet");
            if(bullet != null)
            {
                bullet.transform.position = (bulletSpawnPoint.transform.position + hit.point) / 2;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.rotation *= Quaternion.Euler(0, 90f, 0);
                bullet.SetActive(true);
                bullets.Add(bullet);
                NewScale(bullet, hit.distance);
                ParticleShape(bullet);
            }
            GameObject hitExplosion = ObjectPooler.sharedInstance.GetPooledObject("HitExplosion");
            if(hitExplosion != null)
            {
                hitExplosion.transform.position = hit.point;
                hitExplosion.transform.rotation = Quaternion.identity;
                hitExplosion.SetActive(true);
            }
            if(hit.collider.gameObject.tag != "BlueBullet")
            {
                GameObject wallMark = ObjectPooler.sharedInstance.GetPooledObject("WallMark");
                if(wallMark != null)
                {
                    wallMark.transform.position = hit.point + Vector3.forward * 0f;
                    wallMark.transform.rotation = Quaternion.LookRotation(hit.normal);
                    wallMark.SetActive(true);
                }
            }
        }
    }

    public void NewScale(GameObject theGameObject, float newSize)
    {
        theGameObject.transform.localScale = new Vector3((newSize + 0.1f) * 10f, 1f, 1f);
    }

    public void ParticleShape(GameObject parent)
    {
        electricity = parent.GetComponentsInChildren<ParticleSystem>()[0];
        laserSpawn = parent.GetComponentsInChildren<ParticleSystem>()[1];
        var shElectricity = electricity.shape;
        var shLaser = laserSpawn.shape;

        shElectricity.scale = new Vector3(parent.transform.localScale.x / 10f, 0.2f, 0.2f);
        shLaser.scale = new Vector3(parent.transform.localScale.x / 10f, 0.2f, 0.2f);
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
