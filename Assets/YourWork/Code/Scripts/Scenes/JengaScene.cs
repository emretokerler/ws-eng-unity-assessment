using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JengaScene : MonoBehaviour
{
    [SerializeField] CanvasGroup loadingBlocker;
    [SerializeField] TextMeshProUGUI highlightedStackTxt;
    [SerializeField] GameObject testMyStackButton;
    private StackTemplate highlightedStack;
    void Initialize()
    {
        loadingBlocker.alpha = 1;

        var stackDataFetcher = new StackDataFetcher();
        stackDataFetcher.RequestStackData();
    }

    private void Awake() => Initialize();

    public void OnTestMyStackButtonClicked()
    {
        testMyStackButton.SetActive(false);
        highlightedStack.TestMyStack();
    }

    public void OnRestartButtonClicked()
    {
        loadingBlocker.blocksRaycasts = true;
        DOTween.Kill(loadingBlocker);
        DOTween.To(() => loadingBlocker.alpha, (x) => loadingBlocker.alpha = x, 1, 0.3f).SetEase(Ease.OutSine).SetId(loadingBlocker).OnComplete(() =>
        {
            RestartScene();
        });
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnregisterEvents();
    void RegisterEvents()
    {
        GameEvents.On<StackDataFetchedEvent>(HandleStackDataFetched);
        GameEvents.On<StackHighlightedEvent>(HandleStackHighlighted);
    }
    void UnregisterEvents()
    {
        GameEvents.Off<StackDataFetchedEvent>(HandleStackDataFetched);
        GameEvents.Off<StackHighlightedEvent>(HandleStackHighlighted);
    }

    void HandleStackDataFetched(StackDataFetchedEvent e)
    {
        DOTween.Kill(loadingBlocker);
        DOTween.To(() => loadingBlocker.alpha, (x) => loadingBlocker.alpha = x, 0, 0.3f).SetEase(Ease.OutSine).SetId(loadingBlocker).OnComplete(() =>
        {
            loadingBlocker.blocksRaycasts = false;
        });
    }

    void HandleStackHighlighted(StackHighlightedEvent e)
    {
        highlightedStack = e.StackTemplate;
        highlightedStackTxt.SetText(highlightedStack.Title);
        testMyStackButton.SetActive(!highlightedStack.IsTested);
    }
}