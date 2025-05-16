using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BombCodePickup : MonoBehaviour
{
    public TextMeshProUGUI codeStatusText;

    private void OnTriggerEnter(Collider other)
    {
        //if player walks over the book, a trigger event takes place that destroys the book and updates bomb code
        if (other.CompareTag("Player"))
        {
            if (codeStatusText != null)
            {
                codeStatusText.SetText("Bomb Code Found: " + GameManager.bombCode);
            }
            Destroy(gameObject);
        }
    }
}
