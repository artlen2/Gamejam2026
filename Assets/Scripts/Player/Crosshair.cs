using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CrosshairsUI : MonoBehaviour
{
    public LayerMask targetMask;
    public Color dotHighlightColor;
    public Image dot;

    private Color originalDotColor;

    void Start()
    {
        Cursor.visible = false;
        originalDotColor = dot.color;
    }

    void Update()
    {
        // suivre la souris
        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos;

        // lancer un ray vers la scene
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        DetectTargets(ray);
    }

    void DetectTargets(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, targetMask))
            dot.color = dotHighlightColor;
        else
            dot.color = originalDotColor;
    }
}