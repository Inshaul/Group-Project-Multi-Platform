using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private Sprite alien;
    [SerializeField] private Sprite alienHit;
    [SerializeField] private GameManager gameManager;

    // Alien hiding
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    // Time to show an alien
    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    private bool hittable = true;
    private int alienIndex = 0;

    private IEnumerator ShowHide(Vector2 start, Vector2 end) {
        transform.localPosition = start;

        //Show the alien
        float elapsed = 0f;
        while (elapsed < showDuration) {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        //Hide the alien
        elapsed = 0f;
        while (elapsed < showDuration) {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;

        if (hittable) {
            hittable = false;
            gameManager.Missed(alienIndex);
        }
    }

    public void Hide() {
        transform.localPosition = startPosition;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }

    private IEnumerator QuickHide() {
        yield return new WaitForSeconds(0.25f);
        if (!hittable) {
            Hide(); 
        }
    }

    private void OnMouseDown() {
        if (hittable) {
            spriteRenderer.sprite = alienHit;
            gameManager.AddScore(alienIndex);
            StopAllCoroutines();
            StartCoroutine(QuickHide());
            hittable = false;
        }
    }

    private void CreateNext() {
        float random = Random.Range(0f, 1f);
        spriteRenderer.sprite = alien;
        hittable = true;
    }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);

    }

    public void Activate() {
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    public void SetIndex(int index) {
        alienIndex = index;
    }

    public void StopGame() {
        hittable = false;
        StopAllCoroutines();
    }
}
