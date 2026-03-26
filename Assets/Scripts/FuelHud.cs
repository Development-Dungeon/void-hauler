using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Top-of-screen fuel bar driven by PlayerFuel.Normalized.
/// </summary>
public class FuelHud : MonoBehaviour
{
    [SerializeField] private PlayerFuel fuel;
    private Image _fill;

    void Awake()
    {
        if (fuel == null)
            fuel = FindFirstObjectByType<PlayerFuel>();

        // Avoid stacking a second canvas if the scene already has one from a previous save.
        var existingRoot = transform.Find("FuelHudCanvas");
        if (existingRoot != null)
        {
            var fillT = existingRoot.Find("FuelTrack/FuelFill");
            if (fillT != null)
                _fill = fillT.GetComponent<Image>();
            if (_fill != null)
                return;
        }

        var canvasGo = new GameObject("FuelHudCanvas");
        canvasGo.transform.SetParent(transform, false);
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGo.AddComponent<GraphicRaycaster>();

        var track = new GameObject("FuelTrack", typeof(RectTransform));
        track.transform.SetParent(canvasGo.transform, false);
        var trt = track.GetComponent<RectTransform>();
        trt.anchorMin = new Vector2(0f, 1f);
        trt.anchorMax = new Vector2(1f, 1f);
        trt.pivot = new Vector2(0.5f, 1f);
        trt.anchoredPosition = Vector2.zero;
        trt.sizeDelta = new Vector2(-32f, 8f);
        var whiteSprite = UiWhiteSprite();
        if (whiteSprite == null)
            return;

        var trackImg = track.AddComponent<Image>();
        trackImg.sprite = whiteSprite;
        trackImg.color = new Color(0.12f, 0.12f, 0.14f, 0.75f);
        trackImg.raycastTarget = false;

        var fillGo = new GameObject("FuelFill", typeof(RectTransform));
        fillGo.transform.SetParent(track.transform, false);
        var frt = fillGo.GetComponent<RectTransform>();
        frt.anchorMin = Vector2.zero;
        frt.anchorMax = Vector2.one;
        frt.offsetMin = Vector2.zero;
        frt.offsetMax = Vector2.zero;
        _fill = fillGo.AddComponent<Image>();
        _fill.sprite = whiteSprite;
        _fill.color = new Color(1f, 0.5f, 0.12f, 0.95f);
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Horizontal;
        _fill.fillOrigin = (int)Image.OriginHorizontal.Left;
        _fill.raycastTarget = false;
        _fill.fillAmount = 1f;
    }

    void Start()
    {
        if (fuel == null)
            fuel = FindFirstObjectByType<PlayerFuel>();
    }

    static Sprite UiWhiteSprite()
    {
        var t = Texture2D.whiteTexture;
        if (t == null)
            return null;
        return Sprite.Create(t, new Rect(0f, 0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100f);
    }

    void Update()
    {
        if (fuel == null)
            fuel = FindFirstObjectByType<PlayerFuel>();
        if (fuel == null || _fill == null)
            return;
        _fill.fillAmount = fuel.Normalized;
    }
}
