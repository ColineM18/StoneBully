using UnityEngine;
using UnityEngine.EventSystems;

public class TapManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerCharacter player;
    public float longPress;
    public float pressTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        pressTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float startPress = pressTime;
        pressTime = Time.time - startPress;
        if (pressTime < longPress)
            player.ShootTo(eventData.position);

        //Debug.Log((pressTime < longPress ? "shoot " : "move ") + pressTime);
    }

}
