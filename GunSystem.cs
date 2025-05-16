using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public bool isPlayer = false; //set to true for player and left false for enemy

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public int ammoReserve = 20; //reserve ammo player starts with


    //bools 
    bool shooting = false;
    bool readyToShoot = true;
    bool reloading = false;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Start()
    {
        // Debug.Log("GunSystem script is running!");
    }
    private void Update()
    {
        //skips the update code for enemies
        if (!isPlayer) return;

        //for player only        
        if(ammoText) ammoText.SetText(bulletsLeft + "/" + ammoReserve);
        
        if (allowButtonHold && Input.GetKey(KeyCode.Mouse0))
        {
            shooting = true;
            Debug.Log("Shooting: " + shooting);
        }
        else if(!allowButtonHold && Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
        }
        else shooting = false;

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && ammoReserve > 0 && !reloading) Reload();

        //shoot triggered
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    //Triggered in enemy code
    public void TriggerShot(){
        if (readyToShoot && !reloading && bulletsLeft > 0){
            //Shot triggered
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        //enemy auto reloads (with finite ammo)
        else if(bulletsLeft <= 0 && ammoReserve > 0 && !reloading) Reload();
    }

    private void Shoot()
    {
        readyToShoot = false;

        Debug.Log("Shooting: " + shooting + ", (Bullets left = " + bulletsLeft); 

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate Direction with Spread
        Vector3 direction;
        if (fpsCam){
            direction = fpsCam.transform.forward + new Vector3(x, y, 0); //for player use
        }
        //determines if enemy is shooting
        else if (GameObject.FindWithTag("Player"))
        {
            //gets player's position
            Transform player = GameObject.FindWithTag("Player").transform;
            Vector3 targetDirection = (player.position + Vector3.up* 1.5f) - attackPoint.position;
            direction = targetDirection.normalized + new Vector3(x,y,0);
        }
        else direction = transform.forward;
        
        // RayCast for bullets
        if (Physics.Raycast(attackPoint.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log($"{gameObject.name} Hit: + {rayHit.collider.name}");
            
            //if enemy is hit, enemy take damage function is triggered
            if (rayHit.collider.TryGetComponent<EnemyAI>(out var enemy)){
                enemy.TakeDamage(damage);
            }

            //if player is hit, player take damage function is triggered
            if (rayHit.collider.CompareTag("Player")){
                if (rayHit.collider.TryGetComponent<PlayerHealth>(out var playerHealth)){
                    playerHealth.TakeDamage(damage);
                }
            }

            //instantiates bullet hole effect
            if(bulletHoleGraphic) Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        }

        //instantiates muzzle flash effect
        if (muzzleFlash != null)
        {
            var flash = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            flash.transform.SetParent(attackPoint);
            Destroy(flash, 0.1f);
        }
        else
        {
            Debug.LogError("Muzzle Flash Prefab is not assigned!");
        }

        bulletsLeft--;
        bulletsShot--;

        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        if(ammoReserve <=0 || bulletsLeft == magazineSize)
        {
            Debug.Log("Cannot reload. No reserve ammo or magazine size full");
            return;
        }

        reloading = true;
        Debug.Log("Reloading...");
        Invoke(nameof(ReloadFinished), reloadTime);
    }
    private void ReloadFinished()
    {
        int bulletsNeeded = magazineSize - bulletsLeft;
        int bulletsToReload = Mathf.Min(bulletsNeeded, ammoReserve);

        bulletsLeft += bulletsToReload;
        ammoReserve -= bulletsToReload;

        reloading = false;

        if(ammoText){
            ammoText.SetText(bulletsLeft + "/" + ammoReserve);
        }
    }
    public void AddAmmo(int amount){
        ammoReserve += amount;

        if (ammoText != null){
            ammoText.SetText(bulletsLeft + "/" + ammoReserve);
        }
    }
}

