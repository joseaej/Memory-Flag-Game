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
        Debug.Log("Carta volteada, ID: " + _id);
    }

    public void UnFlip()
    {
        Back.SetActive(true);
        Debug.Log("Carta desvolteada");
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        CardFlag.sprite = image;
    }

    private void OnMouseDown()
    {
        Debug.Log("¡Carta clickeada!");
        if (!bIsCurrentlyFlipped && controller.CanFlip)
        {
            Flip();
            controller.NotifyCardFlipped(this);
        }
    }
}
