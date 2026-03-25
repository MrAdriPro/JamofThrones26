using UnityEngine;

public class TerreinTextureManager : MonoBehaviour
{
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
        grassLayer.diffuseTexture = baseColorsGrass[ShopManager.shopInstance.tekLevel];
        //grassLayer.normalMapTexture = normalMapsGrass[ShopManager.shopInstance.tekLevel];

        //pathLayer.diffuseTexture = baseColorsPath[ShopManager.shopInstance.tekLevel];
        //pathLayer.normalMapTexture = normalMapsPath[ShopManager.shopInstance.tekLevel];
    }
}
