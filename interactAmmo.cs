using System.Collections;
using UnityEngine;

public class interactAmmo : MonoBehaviour, IInteractable
{
    public int ammoAmount = 10;
    public AudioSource ammoOpen;
    void IInteractable.Interact()
    {
        Debug.Log("Ammo pickup . interact called baby");
        StartCoroutine(HandleInteraction());
    }

    private IEnumerator HandleInteraction()
    {
        //find gun system attached to player (avoids affecting enemy gun system)
        GunSystem gunSystem = GameObject.FindWithTag("Player")?.GetComponentInChildren<GunSystem>();
        
        if (gunSystem != null)
        {
            gunSystem.AddAmmo(ammoAmount);
            ammoOpen.Play();
            Debug.Log($"Picked up {ammoAmount} ammo");
        }

        yield return new WaitForSeconds(0.1f);
        InteractionManager.Instance.EndInteraction();
        Destroy(gameObject);
    }
}
