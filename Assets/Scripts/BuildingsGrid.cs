using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    [SerializeField] private static int _radiusToBuild = 10;
    [SerializeField] private Buildings _mainBuild;
    [SerializeField] private GameObject buildPref;
    
    private Vector2Int GridSize = new Vector2Int(_radiusToBuild, _radiusToBuild);

    private Buildings[,] grid;
    private Buildings flyingBuilding;

    private Camera mainCamera;

    private List<GameObject> tilesForBuild; 
    private void Awake()
    {
        grid = new Buildings[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
    }

    private void BuildingZone()
    {
        Instantiate(_mainBuild, new Vector3(0, 1, 0), Quaternion.identity);
        for (int x = -_radiusToBuild; x < GridSize.x; x++)
        {
            for (int z = -_radiusToBuild; z < GridSize.y; z++)
            {
                Instantiate(buildPref, new Vector3(x, 2, z), Quaternion.identity);
            }
        }
        
    }

    public void StartPlacingBuilding(Buildings buildPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildPrefab);
    }
    private void Update()
    {
        if (flyingBuilding != null)
        {
            BuildingZone();
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

                if (available && IsPlaceTaken(x, y)) available = false;
                    
                flyingBuilding.transform.position = new Vector3(x, 1.5f, y);
                
                flyingBuilding.SetTransparent(available);

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    private bool IsPlaceTaken(int PlaceX, int PlaceY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int z = 0; z < flyingBuilding.Size.y; z++)
            {
                if (grid[PlaceX + x, PlaceY + z] != null) return true;
            }
        }

        return false;
    }
    private void PlaceFlyingBuilding(int PlaceX, int PlaceY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int z = 0; z < flyingBuilding.Size.y; z++)
            {
                grid[PlaceX + x, PlaceY + z] = flyingBuilding;
            }
        }
        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }
}
