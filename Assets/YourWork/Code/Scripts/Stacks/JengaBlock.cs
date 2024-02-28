using System.Collections;
using System.Collections.Generic;
using ModelShark;
using UnityEngine;

public class JengaBlock : MonoBehaviour
{
    public BlockType Type;

    [SerializeField] private GameObject[] blockRenderers;
    private TooltipTrigger toolTip => _toolTip ??= GetComponent<TooltipTrigger>(); TooltipTrigger _toolTip;
    private Rigidbody rb => _rb ??= GetComponent<Rigidbody>(); Rigidbody _rb;
    private Renderer activeBlockRenderer;
    private BlockData blockData;
    private StackTemplate parentStack;

    private void Initialize() { }

    private void Awake() => Initialize();


    public void SetData(BlockData blockData, StackTemplate parentStack)
    {
        this.blockData = blockData;
        this.parentStack = parentStack;
        gameObject.SetActive(true);

        SetBlockType(blockData.Mastery);
        toolTip.SetText("TitleText1", blockData.Domain);
        toolTip.SetText("TitleText2", blockData.Grade);
        toolTip.SetText("BodyText1", blockData.Cluster);
        toolTip.SetText("BodyText2", blockData.StandardId);
        toolTip.SetText("BodyText3", blockData.StandardDescription);
    }

    public void EnablePhysics()
    {
        rb.isKinematic = false;
    }

    private void SetBlockType(int type)
    {
        type = Mathf.Clamp(type, 0, 2);

        Type = (BlockType)type;

        foreach (var br in blockRenderers)
        {
            br.SetActive(false);
        }

        activeBlockRenderer = blockRenderers[blockData.Mastery].GetComponent<Renderer>();
        activeBlockRenderer.gameObject.SetActive(true);
    }

    private void ToggleHighlight(bool isActive)
    {
        if (isActive)
        {
            activeBlockRenderer.material.EnableKeyword(Constants.STANDARD_SHADER_EMISSION_KW);
        }
        else
        {
            activeBlockRenderer.material.DisableKeyword(Constants.STANDARD_SHADER_EMISSION_KW);
        }
    }

    private void OnMouseUp()
    {
        parentStack.OnMouseUp();
    }

    private void OnMouseDown()
    {
        parentStack.OnMouseDown();
    }

    private void OnMouseEnter()
    {
        ToggleHighlight(true);
    }

    private void OnMouseExit()
    {
        ToggleHighlight(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(Constants.GROUND_TAG))
        {
            parentStack.OnBlockFallToGround(this);
        }
    }
}

public enum BlockType
{
    Glass = 0,
    Wood = 1,
    Stone = 2
}