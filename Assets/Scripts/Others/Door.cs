using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject OpenObject;
    public GameObject CloseSprite;

    public void Activated(bool Open)
    {
        OpenObject.SetActive(Open);
        CloseSprite.SetActive(!Open);
        gameObject.SetActive(true);
    }
}