using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class Controller : MonoBehaviour
{
    [SerializeField] MemoryCard cardPrefabs;
    [SerializeField] Sprite[] images;
    private int[] shuffledCardIds;
    private int[] cardIds;
    public int score = 0;
    private MemoryCard firstCard;
    private MemoryCard secondCard;
    public bool CanFlip => firstCard == null || secondCard == null;
    Stopwatch stopwatch = new Stopwatch();

    void Start()
    {
        RegisterCards();
        ShuffleCards();
        PlaceCards(3, 4);  // Puedes ajustar el tamaño del tablero si lo deseas
        stopwatch.Start();
    }

    void RegisterCards()
    {
        cardIds = new int[images.Length * 2];
        for (int i = 0; i < images.Length; i++)
        {
            cardIds[i * 2] = i;
            cardIds[i * 2 + 1] = i;
        }
    }

    T[] Shuffle<T>(T[] a)
    {
        T[] shuffled = (T[])a.Clone();
        int j;
        T aux;
        for (int i = 0; i < shuffled.Length; i++)
        {
            j = Random.Range(i, shuffled.Length);
            aux = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = aux;
        }
        return shuffled;
    }

    void ShuffleCards()
    {
        shuffledCardIds = Shuffle(cardIds);
    }

    public void PlaceCards(int nRows, int nCols, int marginX = 3, int marginY = 1)
    {
        if (nRows * nCols != shuffledCardIds.Length)
        {
            Debug.LogError("El número de filas y columnas no coincide con la cantidad de cartas.");
            return;
        }

        Vector2 cardSize = cardPrefabs.Size;
        float anchoCarta = cardSize.x;
        float altoCarta = cardSize.y;

        float anchoCamara = Camera.main.pixelWidth / 100f;
        float altoCamara = Camera.main.pixelHeight / 100f;

        float gapX = (anchoCamara - (anchoCarta * nCols) - (marginX * 2)) / (nCols - 1);
        if (gapX < 0) gapX = 0;

        float gapY = (altoCamara - (altoCarta * nRows) - (marginY * 2)) / (nRows - 1);
        if (gapY < 0) gapY = 0;

        float anchoFila = (anchoCarta * nCols) + (gapX * (nCols - 1));
        float altoColumna = (altoCarta * nRows) + (gapY * (nRows - 1));

        float x0 = -(anchoFila - anchoCarta) / 2;
        float y0 = (altoColumna - altoCarta) / 2;

        float offsetX = anchoCarta + gapX;
        float offsetY = altoCarta + gapY;

        int index = 0;
        for (int row = 0; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                int id = shuffledCardIds[index++];
                MemoryCard card = Instantiate(cardPrefabs) as MemoryCard;
                card.SetCard(id, images[id]);
                card.controller = this;

                float coordX = x0 + (col * offsetX);
                float coordY = y0 - (row * offsetY);
                card.transform.position = new Vector3(coordX, coordY, -1);
            }
        }
    }

    private IEnumerator CheckCard()
    {
        yield return new WaitForSeconds(1f);
        if (firstCard.ID == secondCard.ID)
        {
            score++;
            Destroy(firstCard.gameObject);
            Destroy(secondCard.gameObject);
        }
        else
        {
            firstCard.UnFlip();
            secondCard.UnFlip();
        }
        firstCard = null;
        secondCard = null;
        if (score == images.Length)
        {
            StartCoroutine(RestartGame());
            stopwatch.Stop();
            Debug.Log("Tiempo: " + stopwatch.Elapsed.Duration().Minutes + ":" + stopwatch.Elapsed.Duration().Seconds);
        }
    }

    public void NotifyCardFlipped(MemoryCard card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckCard());
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }
}
