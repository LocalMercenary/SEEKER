using System.Collections;
using UnityEngine;

public class MonsterHide : MonoBehaviour
{
    public RectTransform[] images; // Assign multiple images in the Inspector
    public RectTransform canvasRect; // Assign the Canvas RectTransform in the Inspector
    public Transform player; // Assign the Player Transform in the Inspector
    public bool imageRestart = true;

    void Start()
    {
        if (images.Length == 0 || canvasRect == null || player == null)
        {
            Debug.LogError("Assign images, Canvas RectTransform, and Player Transform in the Inspector!");
            return;
        }

   
        
    }

    void Update()
    {
        FacePlayer();

        if (imageRestart)
        {
            foreach (RectTransform image in images)
            {
                StartCoroutine(MoveImageRoutine(image));
            }
            imageRestart = false;
        }
    }


    void FacePlayer()
    {
        if (player != null)
        {
            // Make the canvas look at the player
            canvasRect.transform.LookAt(player);

            // Rotate 180 degrees to make it face away
            canvasRect.transform.Rotate(0f, 180f, 0f);
        }
    }

    public IEnumerator MoveImageRoutine(RectTransform image)
    {
        while (true) // Loop to keep updating position & scale
        {
            SetRandomPositionAndScale(image);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f)); // Random delay for variation
        }
    }

    void SetRandomPositionAndScale(RectTransform image)
    {
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float halfWidth = image.rect.width / 2;
        float halfHeight = image.rect.height / 2;

        float randomX = Random.Range(-canvasWidth / 2 + halfWidth, canvasWidth / 2 - halfWidth);
        float randomY = Random.Range(-canvasHeight / 2 + halfHeight, canvasHeight / 2 - halfHeight);

        float randomScale = Random.Range(1f, 2.5f);
        image.localScale = new Vector3(randomScale, randomScale, 1f);

        image.anchoredPosition = new Vector2(randomX, randomY);
    }
    void OnEnable()
    {
        imageRestart = true; // Ensure images restart movement when re-enabled
    }

}
