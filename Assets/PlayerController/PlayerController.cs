
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    public bool hit;
    public bool hardnessMultiplierApplied = false;

    private float currentBreakTime = 0f;
    private bool isBreaking = false;
    private GameObject breakingBlock;
    private float breakDuration;
   
    private BlockSO blockSO;
    [SerializeField] GameObject blockToPlace;

    public LayerMaskMode currentLayerMaskMode = LayerMaskMode.Main;
    public BlockPlaceParentMode currentBlockPlaceParentMode = BlockPlaceParentMode.Main;

    public Vector2Int mousePos;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask MouseLayer;
    [SerializeField] private GameObject BlockPlaceParent;
    [SerializeField] private LayerMask BackMainFrontLayer;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private GameObject mouseBox;
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private Vector2 gridOffset = new Vector2(0.5f, 0.5f);

    private Inventory inventory;

    public enum LayerMaskMode
    {
        BackPlane,
        Main,
        FrontPlane
    }
    public enum BlockPlaceParentMode
    {
        BackPlane,
        Main,
        FrontPlane
    }

    // Oyuncunun elindeki aracı ve aracın malzeme seviyesini temsil eden değişkenler
    public ToolType currentToolType;
    public ToolMaterial currentToolMaterial;
    public float defaultBreakSpeed = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

        if (anim == null)
            Debug.LogError("Animator component is missing on PlayerController!");

        if (inventory == null)
            Debug.LogError("Inventory component is missing on PlayerController!");

        if (BlockPlaceParent == null)
        {
            BlockPlaceParent = GameObject.Find("Main"); // Varsayılan olarak "Main" isimli GameObject'i ata
 
        }
    }


    private bool onGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void mouseBlockCheck()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, BackMainFrontLayer);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Hit object name: " + hitObject.name);
            Debug.Log("Hit object transform: " + hitObject.transform.position);
            Debug.Log("Hit object layer: " + LayerMask.LayerToName(hitObject.layer));

            BlockSO blockData = hitObject.GetComponent<Blocks>().blockSO;
            if (blockData != null)
            {

            }

            if (hitObject.transform.parent != null)
            {
                Debug.Log("Parent object name: " + hitObject.transform.parent.name);
            }
        }
    }

    private void BlockBreake()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, MouseLayer);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            BlockSO blockData = hitObject.GetComponent<Blocks>().blockSO;

            if (blockData != null)
            {
                float breakSpeedMultiplier = 1.0f;
                float CurrentBlockHardness = blockData.Hardness;

                if (currentToolType == blockData.BestToolType)
                {
                    breakSpeedMultiplier = ToolMaterialBreakSpeed.BreakSpeed[blockData.ToolBreakSpeed];
                }

                if (!hardnessMultiplierApplied)
                {
                    if (currentToolMaterial == blockData.MinHarvestToolTier)
                    {
                        CurrentBlockHardness *= 1.5f;
                    }
                    else
                    {
                        CurrentBlockHardness *= 5.0f;
                    }
                    hardnessMultiplierApplied = true;
                }

                breakDuration = CurrentBlockHardness / (defaultBreakSpeed * breakSpeedMultiplier);

                if (breakingBlock != hitObject)
                {
                    breakingBlock = hitObject;
                    currentBreakTime = 0f; // Yeni bloğa geçtiyse süre sıfırlansın
                }

                isBreaking = true;
                return;
            }
        }

        isBreaking = false;
        breakingBlock = null;
    }


    private void BlockPlace()
        
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, MouseLayer);

        bool canPlaceBlock = true;
        bool canPlaceBlockMain = true;

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            BlockSO blockData = hitObject.GetComponent<Blocks>()?.blockSO;
            if (blockData != null)
            {
                canPlaceBlock = false;
                break;
            }
        }

        Vector3 playerPosition = transform.position;
        playerPosition.x = Mathf.Round((playerPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
        playerPosition.y = Mathf.Round((playerPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;

        Vector3 blockPosition = new Vector3(
            Mathf.Round((mouseWorldPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x,
            Mathf.Round((mouseWorldPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y,
            BlockPlaceParent.transform.position.z
        );

        if (playerPosition.x == blockPosition.x && 
            (playerPosition.y == blockPosition.y || playerPosition.y + 1 == blockPosition.y) && BlockPlaceParent.name == "Main")
        {
            Debug.Log("Cannot place block: Player is in the way!");
            canPlaceBlockMain = false;
        }


        if (canPlaceBlock && canPlaceBlockMain)
        {
            if (blockToPlace != null)
            {
                BlockSO blockSO = blockToPlace.GetComponent<Blocks>()?.blockSO;
                if (blockSO != null && blockSO.IsPlaceable)
                {
                    Vector3 NewBlockPosition = Vector3.zero;

                    // Blok yerleştirme pozisyonunu ızgaraya hizala
                    NewBlockPosition.x = Mathf.Round((mouseWorldPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
                    NewBlockPosition.y = Mathf.Round((mouseWorldPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;
                    NewBlockPosition.z = BlockPlaceParent.transform.position.z;
                    

                    // Yeni bloğu oluştur ve pozisyonunu ayarla
                    GameObject newBlock = Instantiate(blockToPlace, NewBlockPosition, Quaternion.identity);
                    newBlock.GetComponent<Blocks>().blockSO.IsPlaceable = true;

                    // Bloğu BlockPlaceParent nesnesinin altına yerleştir
                    newBlock.transform.SetParent(BlockPlaceParent.transform);

                    Debug.Log("Block placed successfully at: " + NewBlockPosition);
                }
            }
        }
        else
        {
            Debug.Log("Block placement failed!");
        }
    }


    private void ChangeLayerMaskMode()
    {
        switch (currentLayerMaskMode)
        {
            case LayerMaskMode.BackPlane:
                currentLayerMaskMode = LayerMaskMode.Main;
                MouseLayer = LayerMask.GetMask("Main");
                break;
            case LayerMaskMode.Main:
                currentLayerMaskMode = LayerMaskMode.FrontPlane;
                MouseLayer = LayerMask.GetMask("FrontPlane");
                break;
            case LayerMaskMode.FrontPlane:
                currentLayerMaskMode = LayerMaskMode.BackPlane;
                MouseLayer = LayerMask.GetMask("BackPlane");
                break;
        }
        Debug.Log("Current LayerMask Mode: " + currentLayerMaskMode);
    }
    private void ChangeBlockPlaceParentMode()
    {
        switch (currentBlockPlaceParentMode)
        {
            case BlockPlaceParentMode.BackPlane:
                currentBlockPlaceParentMode = BlockPlaceParentMode.Main;
                BlockPlaceParent = GameObject.Find("Main");
                break;
            case BlockPlaceParentMode.Main:
                currentBlockPlaceParentMode = BlockPlaceParentMode.FrontPlane;
                BlockPlaceParent = GameObject.Find("FrontPlane");
                break;
            case BlockPlaceParentMode.FrontPlane:
                currentBlockPlaceParentMode = BlockPlaceParentMode.BackPlane;
                BlockPlaceParent = GameObject.Find("BackPlane");
                break;
        }
        Debug.Log("Current BlockPlaceParent Mode: " + currentBlockPlaceParentMode);
    }
    private void MovePlayer()
    {
        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        hit = Input.GetMouseButton(0);

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);

        if ((vertical > 0.1f || jump > 0.1f) && onGrounded())
        {
            movement.y = jumpForce;
        }

        rb.velocity = movement;
    }
    

    private void FixedUpdate()
    {

        hit = Input.GetMouseButton(0);

        MovePlayer();
    }

    private void Update()
    {
        UpdateMouseBox();
        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool("hit", hit);

        if (Input.GetMouseButton(0))
        {
            BlockBreake();
        }
        else
        {
            isBreaking = false;
            breakingBlock = null;
            currentBreakTime = 0f; // Tuş bırakıldığında süre sıfırlansın
        }

        if (isBreaking && breakingBlock != null)
        {
            currentBreakTime += 2 * Time.deltaTime;
            //Debug.Log("Current Break Time: " + currentBreakTime);
            if (currentBreakTime >= breakDuration)
            {
                GetComponent<Inventory>().AddItem(breakingBlock.name);
                Destroy(breakingBlock);
                isBreaking = false;
                breakingBlock = null;
                hardnessMultiplierApplied = false;
                Inventory inventory = GetComponent<Inventory>();


            }
            
            
            
        }

        if (Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
        {
            //mouseBlockCheck();
            BlockPlace();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeLayerMaskMode();
            ChangeBlockPlaceParentMode();
        }

    }

    private void UpdateMouseBox()
    {
        if (Camera.main != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Z konumunu 0 yap

            mouseWorldPosition.x = Mathf.Round((mouseWorldPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
            mouseWorldPosition.y = Mathf.Round((mouseWorldPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;
            mouseWorldPosition.z = -0.5f;
            mouseBox.transform.position = mouseWorldPosition;
        }
    }
}
