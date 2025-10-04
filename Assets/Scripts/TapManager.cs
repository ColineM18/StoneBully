using UnityEngine;
using UnityEngine.EventSystems;

public class TapManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerCharacter player;
    public float deadZone;
    public float dragThreshold;

    private Canvas canvas;
    private Vector2 startPos;
    Vector2 radius = new Vector2(128, 128);

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragThreshold = ((eventData.position - startPos) / (radius * canvas.scaleFactor)).magnitude;

        if (dragThreshold < deadZone)
            player.ShootTo(eventData.position);
        else
            Debug.Log(dragThreshold);
    }

}
