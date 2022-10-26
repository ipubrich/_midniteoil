using UnityEngine;

public class MaterialScroll : MonoBehaviour
{
    [SerializeField] private float scrollX = 0.05f;
    [SerializeField] private float scrollY = 0.05f;
    [SerializeField] private Renderer _renderer;
    private void Awake()
    {

        _renderer = GetComponent<Renderer>();
    }

        void FixedUpdate()
    {
        float offsetX = Time.time * scrollX;
        float offsetY = Time.time * scrollY;
        _renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);

    }
}
