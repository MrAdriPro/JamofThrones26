using UnityEngine;

public class TerreinTextureManager : MonoBehaviour
{
    public Vector2 futureTiling = new Vector2(60, 60);
    public Vector2 originalGrassTiling = new Vector2(12,12);

    [SerializeField] TerrainLayer grassLayer;
    [SerializeField] TerrainLayer pathLayer;

    [SerializeField] Texture2D[] baseColorsGrass;
    [SerializeField] Texture2D[] normalMapsGrass;

    [SerializeField] Texture2D[] baseColorsPath;
    [SerializeField] Texture2D[] normalMapsPath;

    void Update()
    {
        TextureUpdater();
    }

    void TextureUpdater() 
    {
        if (ShopManager.shopInstance.tekLevel >= 3)
        {
            grassLayer.tileSize = futureTiling;
        }
        else 
        {
            grassLayer.tileSize = originalGrassTiling;
        }

        grassLayer.diffuseTexture = baseColorsGrass[ShopManager.shopInstance.tekLevel];
        grassLayer.normalMapTexture = normalMapsGrass[ShopManager.shopInstance.tekLevel];

        pathLayer.diffuseTexture = baseColorsPath[ShopManager.shopInstance.tekLevel];
        pathLayer.normalMapTexture = normalMapsPath[ShopManager.shopInstance.tekLevel];
    }
}
