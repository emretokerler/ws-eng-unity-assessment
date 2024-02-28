using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StackTemplate : MonoBehaviour
{
    private static StackTemplate lastMouseDownStack;
    private static float lastMouseDownTime;
    private static float clickTimer = 0.25f;
    private static float testTime = 10f;

    public string Title => title; private string title;

    [SerializeField] private GameObject jengaLayerPrefab;
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private TextMeshPro testTimerTxt;
    [SerializeField] private GameObject testSuccessIcon;
    [SerializeField] private GameObject testSuccessFx;
    [SerializeField] private GameObject testFailIcon;
    [SerializeField] private GameObject testFailFx;
    [SerializeField] private GameObject highlightParticles;

    private List<BlockData> stackData;
    private List<JengaBlock> availableBlocks;
    private List<JengaBlock> spawnedBlocks;
    private float lastSpawnedLayerAngle = 0;
    public bool IsTested;
    private bool isTesting;

    public void TestMyStack()
    {
        IsTested = true;

        var glassBlocks = spawnedBlocks.FindAll(b => b.Type == BlockType.Glass);
        var nonGlassBlocks = spawnedBlocks.FindAll(b => b.Type != BlockType.Glass);

        foreach (var b in glassBlocks)
        {
            b.gameObject.SetActive(false);
        }

        foreach (var b in nonGlassBlocks)
        {
            b.EnablePhysics();
        }

        titleText.gameObject.SetActive(false);

        StartCoroutine(CR_Test());
    }

    private IEnumerator CR_Test()
    {
        isTesting = true;
        float timer = testTime;
        testTimerTxt.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        while (timer > 0)
        {
            UpdateTestTimer(timer);
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        testTimerTxt.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        isTesting = false;
        OnTestSuccess();
    }

    private void UpdateTestTimer(float time)
    {
        testTimerTxt.SetText($"{time:0.0}s");
    }

    private void OnTestSuccess()
    {
        isTesting = false;
        StartCoroutine(CR_TestSuccess());
    }

    private IEnumerator CR_TestSuccess()
    {
        testSuccessFx.SetActive(true);
        yield return new WaitForSeconds(1f);
        testSuccessIcon.SetActive(true);
    }

    private void OnTestFail()
    {
        isTesting = false;
        StopAllCoroutines();
        testTimerTxt.gameObject.SetActive(false);
        StartCoroutine(CR_TestFail());
    }

    private IEnumerator CR_TestFail()
    {
        testFailFx.SetActive(true);
        yield return new WaitForSeconds(1f);
        testFailIcon.SetActive(true);
    }

    public void Fill(List<BlockData> stackData, string title)
    {
        this.stackData = stackData;
        this.title = title;
        availableBlocks = new List<JengaBlock>();
        spawnedBlocks = new List<JengaBlock>();

        for (int i = 0; i < stackData.Count; i++)
        {
            if (availableBlocks.Count < 1)
            {
                SpawnNewLayer();
            }

            var block = availableBlocks[0];
            availableBlocks.RemoveAt(0);

            block.SetData(stackData[i], this);
            spawnedBlocks.Add(block);
        }

        titleText.SetText(title);
        HighlightStack();
    }

    private void SpawnNewLayer()
    {
        var currentStackHeight = transform.GetMaxBounds().size.y;

        var newLayer = Instantiate(jengaLayerPrefab, transform);
        var layerHeight = newLayer.transform.GetMaxBounds().size.y;
        newLayer.transform.localPosition = Vector3.up * (currentStackHeight + layerHeight / 2);
        newLayer.transform.localRotation = Quaternion.Euler(0, lastSpawnedLayerAngle, 0);
        lastSpawnedLayerAngle = Mathf.Abs(lastSpawnedLayerAngle - 90);
        availableBlocks.AddRange(newLayer.GetComponentsInChildren<JengaBlock>());
        foreach (var b in availableBlocks)
        {
            b.gameObject.SetActive(false);
        }
    }

    private void HighlightStack()
    {
        StackHighlightedEvent.Trigger(this);
    }

    public void OnMouseUp()
    {
        if (lastMouseDownStack == this && Time.time < lastMouseDownTime + clickTimer)
        {
            HighlightStack();
        }
    }

    public void OnMouseDown()
    {
        lastMouseDownTime = Time.time;
        lastMouseDownStack = this;
    }

    public void OnBlockFallToGround(JengaBlock block)
    {
        if (!isTesting) return;
        int blockIndex = spawnedBlocks.FindIndex(b => b == block);
        if (blockIndex > 2)
        {
            OnTestFail();
        }
    }

    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnregisterEvents();
    void RegisterEvents()
    {
        GameEvents.On<StackHighlightedEvent>(HandleStackHighlighted);
    }
    void UnregisterEvents()
    {
        GameEvents.Off<StackHighlightedEvent>(HandleStackHighlighted);
    }

    private void HandleStackHighlighted(StackHighlightedEvent e)
    {
        highlightParticles.SetActive(e.StackTemplate == this);
    }
}