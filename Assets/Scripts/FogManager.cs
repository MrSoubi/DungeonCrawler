using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField] private GameObject fogPrefab;
    [SerializeField] private Transform fogsParent;
    [SerializeField] private bool generateFogs = true;

    public RSO_MapDefinition mapDefinition;
    public RSO_PlayerPosition playerPosition;

    private Dictionary<Vector2Int, GameObject> fogs = new Dictionary<Vector2Int, GameObject>();

    private void OnEnable()
    {
        mapDefinition.onValueChanged += Initialize;
        playerPosition.onValueChanged += HandlePlayerPositionChange;
    }

    private void OnDisable()
    {
        mapDefinition.onValueChanged -= Initialize;
        playerPosition.onValueChanged -= HandlePlayerPositionChange;
    }

    private void AddItem(Vector2Int position, GameObject fog)
    {
        fogs.Add(position, fog);
    }

    private void Initialize(MapDefinition mapDefinition)
    {
        if (generateFogs)
        {
            fogs.Clear();
            GenerateFogs(fogPrefab);
        }
    }

    private void HandlePlayerPositionChange(Vector2Int position)
    {
        if (fogs.ContainsKey(position))
        {
            Destroy(fogs[position]);
            fogs.Remove(position);
        }
    }

    private void GenerateFogs(GameObject tilePrefab)
    {
        for (int x = 0; x < mapDefinition.Value.width; x++)
        {
            for (int y = 0; y < mapDefinition.Value.height; y++)
            {
                GameObject fog = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, fogsParent);
                AddItem(new Vector2Int(x, y), fog);
            }
        }
    }
}
