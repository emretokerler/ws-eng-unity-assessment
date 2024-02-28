using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackSpawner : MonoBehaviour
{
    [SerializeField] private string gradeTitle;
    [SerializeField] private StackTemplate stackTemplatePrefab;
    private StackTemplate stackTemplate;
    private List<BlockData> gradeStack;

    private void SpawnStack()
    {
        stackTemplate = Instantiate(stackTemplatePrefab, transform);
        stackTemplate.Fill(gradeStack, gradeTitle);
    }

    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnregisterEvents();
    void RegisterEvents()
    {
        GameEvents.On<StackDataFetchedEvent>(HandleStackDataFetched);
    }
    void UnregisterEvents()
    {
        GameEvents.Off<StackDataFetchedEvent>(HandleStackDataFetched);
    }

    void HandleStackDataFetched(StackDataFetchedEvent e)
    {
        gradeStack = e.StackDataResponse.Data.FindAll(s => s.Grade.Equals(gradeTitle)).OrderBy(s => s.Domain).OrderBy(s => s.Cluster).OrderBy(s => s.StandardId).ToList();
        SpawnStack();
    }
}
