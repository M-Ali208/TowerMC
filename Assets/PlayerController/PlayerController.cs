
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

    private BlockSO blockSO;

    private LayerMaskMode currentLayerMaskMode = LayerMaskMode.Main;

    public Vector2Int mousePos;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask MouseLayer;
    [SerializeField] private LayerMask BackMainFrontLayer;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private GameObject mouseBox;
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private Vector2 gridOffset = new Vector2(0.5f, 0.5f);

    public enum LayerMaskMode
    {
        BackPlane,
        Main,
        FrontPlane
    }

    // Oyuncunun elindeki aracý ve aracýn malzeme seviyesini temsil eden deðiþkenler
    public ToolType currentToolType;
    public ToolMaterial currentToolMaterial;
    public float defaultBreakSpeed = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

            BlockSO blockData = hitObject.GetComponent<BlockSO>();
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
            BlockSO blockData = hitObject.GetComponent<BlockSO>();
            if (blockData != null)
            {
                float breakSpeedMultiplier = 1.0f;

                // Eðer oyuncunun elindeki araç bloðun BestToolType'ý ile ayný ise
                if (currentToolType == blockData.BestToolType)
                {
                    breakSpeedMultiplier = ToolMaterialBreakSpeed.BreakSpeed[blockData.ToolBreakSpeed];

                }
                else
                {
                    breakSpeedMultiplier *= 1f;
                }
                // Eðer oyuncunun elindeki aracýn malzeme seviyesi bloðun MinHarvestToolTier'i ile ayný ise
                if (currentToolMaterial == blockData.MinHarvestToolTier)
                {
                    blockData.Hardness *= 1.5f;
                }
                else
                {
                    blockData.Hardness *= 5.0f;
                }

                float breakTime = blockData.Hardness / (defaultBreakSpeed * breakSpeedMultiplier);
                StartCoroutine(BreakBlock(hitObject, breakTime));
            }
        }
    }

    private IEnumerator BreakBlock(GameObject block, float breakTime)
    {
        yield return new WaitForSeconds(breakTime);
        Destroy(block);
    }

    private void BlockPlace()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero, Mathf.Infinity, MouseLayer);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;
            BlockSO blockData = hitObject.GetComponent<BlockSO>();
            if (blockData != null)
            {

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

    private void FixedUpdate()
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
    
 