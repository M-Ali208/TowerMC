using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum CurrentToolMaterial { Hand, Wood, Gold, Stone, Iron, Diamond, Netherite }
public enum LayerMaskMode { BackPlane, Main, FrontPlane }
public enum BlockPlaceParentMode { BackPlane, Main, FrontPlane }
public enum ToolMaterialSpeed { Hand, Wood, Stone, Iron, Diamond, Netherite, Gold }
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
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
    [HideInInspector]public GameObject blockToPlace;
    [SerializeField] GameObject BlockPlaceParent;

    public LayerMaskMode currentLayerMaskMode = LayerMaskMode.Main;
    public BlockPlaceParentMode currentBlockPlaceParentMode = BlockPlaceParentMode.Main;
    public ToolMaterialSpeed currentToolMaterialSpeed;
    public CurrentToolMaterial currentToolMaterialTier;
    public ToolType currentToolType;

    public Vector2Int mousePos;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask MouseLayer;
    [SerializeField] private LayerMask BackMainFrontLayer;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private GameObject mouseBox;
    [SerializeField] private GameObject OutlineBox;
    private float gridSize = 1.0f;
    [SerializeField] private Vector2 gridOffset = new Vector2(0.5f, 0.5f);

    public float defaultBreakSpeed = 1.0f;
    public float blockPlaceDelay = 0.12f;

    private Inventory inventory;
    private Coroutine blockPlaceCoroutine;

    public float blockRotationAngle = 0f;

    public static class CurrentToolMaterialTiers
    {
        public static readonly Dictionary<CurrentToolMaterial, int> MaterialTiers = new Dictionary<CurrentToolMaterial, int>
        {
            { CurrentToolMaterial.Hand, 1 },
            { CurrentToolMaterial.Wood, 2 },
            { CurrentToolMaterial.Gold, 2 },
            { CurrentToolMaterial.Stone, 3 },
            { CurrentToolMaterial.Iron, 4 },
            { CurrentToolMaterial.Diamond, 5 },
            { CurrentToolMaterial.Netherite, 5 }
        };
    }

    public static class ToolMaterialBreakSpeed
    {
        public static readonly Dictionary<ToolMaterialSpeed, float> BreakSpeed = new Dictionary<ToolMaterialSpeed, float>
        {
            { ToolMaterialSpeed.Hand, 1f },
            { ToolMaterialSpeed.Wood, 2f },
            { ToolMaterialSpeed.Gold, 12f },
            { ToolMaterialSpeed.Stone, 4f },
            { ToolMaterialSpeed.Iron, 6f },
            { ToolMaterialSpeed.Diamond, 8f },
            { ToolMaterialSpeed.Netherite, 9f }
        };
    }
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();

        if (BlockPlaceParent == null)
            BlockPlaceParent = GameObject.Find("Main");
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
                // İşlemler burada yapılabilir
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
                float breakSpeedMultiplier = CalculateBreakSpeedMultiplier(blockData);
                float currentBlockHardness = CalculateBlockHardness(blockData);

                breakDuration = currentBlockHardness / (defaultBreakSpeed * breakSpeedMultiplier);
                Debug.Log("Break Duration: " + breakDuration);

                if (breakingBlock != hitObject)
                {
                    breakingBlock = hitObject;
                    currentBreakTime = 0f;
                }

                isBreaking = true;
                return;
            }
        }

        isBreaking = false;
        breakingBlock = null;
    }

    private float CalculateBreakSpeedMultiplier(BlockSO blockData)
    {
        if (currentToolType != blockData.BestToolType)
        {
            return 1.0f;
        }
        return ToolMaterialBreakSpeed.BreakSpeed[currentToolMaterialSpeed];
    }

    private float CalculateBlockHardness(BlockSO blockData)
    {
        float currentBlockHardness = blockData.Hardness;
        int currentToolTier = CurrentToolMaterialTiers.MaterialTiers[currentToolMaterialTier];

        if (currentToolTier >= (int)ToolMaterialTiers.MaterialTiers[blockData.MinHarvestToolTier])
        {
            currentBlockHardness *= 1.5f;
        }
        else
        {
            currentBlockHardness *= 5f;
        }
        return currentBlockHardness;
    }

    private IEnumerator BlockPlace()
    {
        while (Input.GetMouseButton(1) && inventory.isHaveBlock())
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, MouseLayer);

            if (CanPlaceBlock(hits))
            {
                PlaceBlock(mouseWorldPosition);
                inventory.inventors.Find(obj => obj.blockName == blockToPlace.name).count--;
            }
            else
            {
                Debug.Log("Block placement failed!");
            }

            yield return new WaitForSeconds(blockPlaceDelay);
        }
    }

    private bool CanPlaceBlock(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            BlockSO blockData = hitObject.GetComponent<Blocks>()?.blockSO;

            if (blockData != null)
            {
                // Geçerli yerleştirme katmanına göre kontrol et
                switch (currentLayerMaskMode)
                {
                    case LayerMaskMode.BackPlane:
                        if (hitObject.layer == LayerMask.NameToLayer("BackPlane"))
                        {
                            return false;
                        }
                        break;
                    case LayerMaskMode.Main:
                        if (hitObject.layer == LayerMask.NameToLayer("Main"))
                        {
                            return false;
                        }
                        break;
                    case LayerMaskMode.FrontPlane:
                        if (hitObject.layer == LayerMask.NameToLayer("FrontPlane"))
                        {
                            return false;
                        }
                        break;
                }
            }
        }
        return true;
    }

    public void PlaceBlock(Vector2 mouseWorldPosition)
    {
        Vector3 playerPosition = GetRoundedPosition(transform.position);
        Vector3 blockPosition = GetRoundedPosition(mouseWorldPosition);

        if (IsPlayerInTheWay(playerPosition, blockPosition))
        {
            Debug.Log("Cannot place block: Player is in the way!");
            return;
        }

        if (blockToPlace != null)
        {
            BlockSO blockSO = blockToPlace.GetComponent<Blocks>()?.blockSO;
            if (blockSO != null && blockSO.IsPlaceable)
            {
                Vector3 newBlockPosition = GetRoundedPosition(mouseWorldPosition);
                newBlockPosition.z = BlockPlaceParent.transform.position.z;

                GameObject newBlock = Instantiate(blockToPlace, newBlockPosition, Quaternion.Euler(0, 0, blockRotationAngle));
                newBlock.GetComponent<Blocks>().blockSO.IsPlaceable = true;
                newBlock.transform.SetParent(BlockPlaceParent.transform);
                SpriteRenderer newBlockRenderer = newBlock.GetComponent<SpriteRenderer>();

                if (newBlockRenderer != null)
                {
                    if (BlockPlaceParent.name == "Main")
                    {
                        newBlockRenderer.sortingOrder = 2;
                    }
                    else if (BlockPlaceParent.name == "BackPlane")
                    {
                        newBlockRenderer.sortingOrder = -1;
                    }
                    else if (BlockPlaceParent.name == "FrontPlane")
                    {
                        newBlockRenderer.sortingOrder = 3;
                    }
                }

                Debug.Log("New block placed at position: " + newBlockPosition);
            }
        }
    }

    public Vector3 GetRoundedPosition(Vector3 position)
    {
        position.x = Mathf.Round((position.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
        position.y = Mathf.Round((position.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;
        return position;
    }

    private bool IsPlayerInTheWay(Vector3 playerPosition, Vector3 blockPosition)
    {
        return playerPosition.x == blockPosition.x &&
               (playerPosition.y == blockPosition.y ||
               playerPosition.y + 1f == blockPosition.y ||
               playerPosition.y - 1f == blockPosition.y) &&
               BlockPlaceParent.name == "Main";
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
        UpdateMouseOutlineBox();
        anim.SetFloat("Horizontal", horizontal);
        anim.SetBool("hit", hit);

        if (Input.GetMouseButton(0))
        {
            BlockBreake();
        }
        else
        {
            ResetBreakingState();
        }

        if (isBreaking && breakingBlock != null)
        {
            currentBreakTime += Time.deltaTime;
            if (currentBreakTime >= breakDuration)
            {
                int currentToolTier = CurrentToolMaterialTiers.MaterialTiers[currentToolMaterialTier];
                BlockSO blockData = breakingBlock.GetComponent<Blocks>().blockSO;

                if (currentToolTier >= (int)ToolMaterialTiers.MaterialTiers[blockData.MinHarvestToolTier])
                {
                    breakingBlock.name = breakingBlock.name.Replace("(Clone)", "");
                    inventory.AddItem(breakingBlock.name);
                }

                Destroy(breakingBlock);
                Debug.Log("Break Time: " + currentBreakTime);
                ResetBreakingState();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (blockPlaceCoroutine == null)
            {
                blockPlaceCoroutine = StartCoroutine(BlockPlace());
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (blockPlaceCoroutine != null)
            {
                StopCoroutine(blockPlaceCoroutine);
                blockPlaceCoroutine = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeLayerMaskMode();
            ChangeBlockPlaceParentMode();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateBlock();
        }
    }

    private void ResetBreakingState()
    {
        isBreaking = false;
        breakingBlock = null;
        hardnessMultiplierApplied = false;
        currentBreakTime = 0f;
    }

    private void UpdateMouseBox()
    {
        if (Camera.main != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            mouseWorldPosition = GetRoundedPosition(mouseWorldPosition);
            mouseWorldPosition.z = -0.5f;
            mouseBox.transform.position = mouseWorldPosition;

            // Elde tutulan bloğun sprite'ını şeffaf bir şekilde göster
            if (blockToPlace != null)
            {
                SpriteRenderer mouseBoxRenderer = mouseBox.GetComponent<SpriteRenderer>();
                SpriteRenderer blockToPlaceRenderer = blockToPlace.GetComponent<SpriteRenderer>();

                if (mouseBoxRenderer != null && blockToPlaceRenderer != null)
                {
                    mouseBoxRenderer.sprite = blockToPlaceRenderer.sprite;
                    mouseBoxRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Şeffaflık ayarı
                    mouseBoxRenderer.transform.rotation = Quaternion.Euler(0, 0, blockRotationAngle);
                }
            }
        }
    }
    private void UpdateMouseOutlineBox()
    {
        if (Camera.main != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            mouseWorldPosition = GetRoundedPosition(mouseWorldPosition);
            mouseWorldPosition.z = -0.5f;
            OutlineBox.transform.position = mouseWorldPosition;
        }
    }


    private void RotateBlock()
    {
        blockRotationAngle += 90f;
        if (blockRotationAngle >= 360f)
        {
            blockRotationAngle = 0f;
        }
        Debug.Log("Block rotation angle: " + blockRotationAngle);
    }
}
