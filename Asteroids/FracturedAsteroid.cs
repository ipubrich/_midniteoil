using UnityEngine;

public class FracturedAsteroid : MonoBehaviour
{
    [SerializeField][Range(1f, 60f)] private float _duration = 10f;
    private Camera mainCamera;
    public Vector2 widthThresold;
    public Vector2 heightThresold;
 
 // destroy when off camera for some time?

   private void OnEnable()
   {
       Destroy(gameObject, _duration);
  //}
  // void Update()
  // {
  //     Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
  //     if (screenPosition.x < widthThresold.x || screenPosition.x > widthThresold.y || screenPosition.y < heightThresold.x || screenPosition.y > heightThresold.y)
  //         Destroy(gameObject);
   }

}