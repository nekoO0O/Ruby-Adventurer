using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;

    public static UIHealthBar Instance { get; private set; }

    public bool hasTask;
    public int fixedNum;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    // 设置当前UI血条显示值
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            originalSize * fillPercent);
    }
}