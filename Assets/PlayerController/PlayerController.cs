
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

    private BlockSO blockSO;
    [SerializeField] GameObject blockToPlace;

    private LayerMaskMode currentLayerMaskMode = LayerMaskMode.Main;
    private BlockPlaceParentMode currentBlockPlaceParentMode = BlockPlaceParentMode.Main;

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
                float CurrentBlockHardness = blockData.Hardness; // If bloğunun dışına taşındı
                 // Her blok için ayrı değişken
                //float previousBlockHardness = 0.0f;
                // Eğer oyuncunun elindeki araç bloğun BestToolType'ı ile aynı ise
                if (currentToolType == blockData.BestToolType)
                {
                    breakSpeedMultiplier = ToolMaterialBreakSpeed.BreakSpeed[blockData.ToolBreakSpeed];
                }

                // Eğer hardnessMultiplier daha önce uygulanmadıysa
                if (!hardnessMultiplierApplied)
                {
                    // Eğer oyuncunun elindeki aracın malzeme seviyesi bloğun MinHarvestToolTier'i ile aynı ise
                    if (currentToolMaterial == blockData.MinHarvestToolTier)
                    {
                        CurrentBlockHardness *= 1.5f;
                    }
                    else
                    {
                        CurrentBlockHardness *= 5.0f;
                    }
                    hardnessMultiplierApplied = true;
                    Debug.Log("Current Block Hardness: " + CurrentBlockHardness);
                }

                float breakTime = CurrentBlockHardness / (defaultBreakSpeed * breakSpeedMultiplier);
                StartCoroutine(BreakBlock(hitObject, breakTime));
            }
        }
    }

    private IEnumerator BreakBlock(GameObject block, float breakTime)
    {
        yield return new WaitForSeconds(breakTime);
        hardnessMultiplierApplied = false;
        GetComponent<Inventory>().AddItem(block.name);
        Destroy(block);
    }


    private void BlockPlace()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, MouseLayer);

        bool canPlaceBlock = true;

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            BlockSO blockData = hitObject.GetComponent<Blocks>().blockSO;
            if (blockData != null)
            {
                canPlaceBlock = false;
                break;
            }
        }

        if (canPlaceBlock)
        {
            // Elindeki bloktan bir blok al
            //GameObject blockToPlace = inventory.GetBlockFromHand();

            if (blockToPlace != null)
            {
                BlockSO blockSO = blockToPlace.GetComponent<Blocks>().blockSO;
                if (blockSO != null && blockSO.IsPlaceable)
                {
                    // Blok yerleştirme pozisyonunu ayarla
                    mouseWorldPosition.x = Mathf.Round((mouseWorldPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
                    mouseWorldPosition.y = Mathf.Round((mouseWorldPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;

                    // Yeni bloğu oluştur ve pozisyonunu ayarla
                    GameObject newBlock = Instantiate(blockToPlace, mouseWorldPosition, Quaternion.identity);
                    newBlock.GetComponent<Blocks>().blockSO.IsPlaceable = true;

                    // Bloğu BlockPlaceParent nesnesinin altına yerleştir
                    newBlock.transform.SetParent(BlockPlaceParent.transform);

                    // Elindeki bloktan bloğu çıkar
                    //inventory.RemoveItemFromHand(blockToPlace.name);
                }
            }
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

        if (Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
        {
            //mouseBlockCheck();
            BlockBreake();
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

/*
 private float currentBreakTime = 0f;
private bool isBreaking = false;
private GameObject breakingBlock;
private float breakDuration;

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
        currentBreakTime += Time.deltaTime;
        if (currentBreakTime >= breakDuration)
        {
            GetComponent<Inventory>().AddItem(breakingBlock.name);
            Destroy(breakingBlock);
            isBreaking = false;
            breakingBlock = null;
            hardnessMultiplierApplied = false;
        }
    }

    if (Input.GetMouseButton(1))
    {
        BlockPlace();
    }

    if (Input.GetKeyDown(KeyCode.T))
    {
        ChangeLayerMaskMode();
        ChangeBlockPlaceParentMode();
    }
}
