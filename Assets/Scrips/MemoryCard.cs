using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CardBack;
    [SerializeField] private SpriteRenderer CardFlag;
    public Controller controller;
    [SerializeField] private GameObject Back;
    
    private int _id;
    
    public bool bIsCurrentlyFlipped => !Back.activeSelf;
    public int ID => _id;
    public Vector2 Size => CardBack.sprite.bounds.size;

    public void Flip()
    {
        Back.SetActive(false);
    }

    public void UnFlip()
    {
        Back.SetActive(true);
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        CardFlag.sprite = image;
    }

    private void OnMouseDown()
    {
        if (!bIsCurrentlyFlipped && controller.CanFlip)
        {
            Flip();
            controller.NotifyCardFlipped(this);
        }
    }
}
