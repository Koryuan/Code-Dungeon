using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open Close Object")]
    [SerializeField] private GameObject openObject;
    [SerializeField] private GameObject closeSprite;

    public void Activated(bool Open)
    {
        openObject.SetActive(Open);
        closeSprite.SetActive(!Open);
        gameObject.SetActive(true);
    }
}