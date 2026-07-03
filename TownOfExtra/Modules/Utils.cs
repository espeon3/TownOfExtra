using UnityEngine;

namespace TownOfExtra.Modules;

public static class Utils
{
    public static GameObject CreateObstructedIcon(this ActionButton button)
    {
        var obstructedOverlay = new GameObject("ObstructedSprite");
        obstructedOverlay.transform.SetParent(button.transform);
        obstructedOverlay.transform.localPosition = new Vector3(0, 0, -10f);
        obstructedOverlay.gameObject.layer = button.gameObject.layer;

        var render = obstructedOverlay.AddComponent<SpriteRenderer>();
        render.sprite = TownOfExtraAssets.ObstructedButtonOverlay.LoadAsset();

        obstructedOverlay.SetObstructActive(false);

        return obstructedOverlay;
    }
    
    public static void SetObstructActive(this GameObject obstructedOverlay, bool isActive)
    {
        if (obstructedOverlay && obstructedOverlay.gameObject)
        {
            obstructedOverlay.gameObject.SetActive(isActive);
            obstructedOverlay.GetComponent<SpriteRenderer>().enabled = isActive;
        }
    }
}