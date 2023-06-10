using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallBehaviour : MonoBehaviour
{
    public int wallHealth; // number of hits any tile can take before being destroyed
    public int damageToBullet; // bounces taken away from bullet on impact
    public bool indestructible; // makes all tiles immune to projectile damage

    private Tilemap tilemap;
    private List<Vector3Int> tileCellLocations = new List<Vector3Int>();
    private List<int> tileHps = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogWarning("Failed to fetch tilemap for wall behaviour");
            return;
        }

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(tilePos))
            {
                tileCellLocations.Add(tilePos); // Take only x and y coordinates
                tileHps.Add(wallHealth); // Add a health tracker for each tile in the map
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile" && !indestructible && other.contactCount > 0)
        {
            ContactPoint2D contactPoint = other.GetContact(0);
            Vector3 point = new Vector3(contactPoint.point.x, contactPoint.point.y, 0); // This requires that our tilemaps all be at z=0

            Vector3Int cell = tilemap.WorldToCell(point);

            int tileIndex = tileCellLocations.IndexOf(cell);

            if (tileIndex == -1)
            {
                Debug.LogWarning($"Failed to retreieve tile at {cell} from impact at {point}");
                return;
            }

            tileHps[tileIndex]--;

            if (tileHps[tileIndex] == 0)
            {
                tilemap.SetTile(tileCellLocations[tileIndex], null); // Remove the tile from the tilemap
            }
        }
    }
}
