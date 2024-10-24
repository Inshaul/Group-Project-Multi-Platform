using UnityEngine;
using TMPro;
//using EZCameraShake;


public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


    //bools 
    bool shooting, readyToShoot, reloading;


    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //Graphics
    public GameObject bulletHoleGraphic;

    public ParticleSystem muzzleFlash;
    //public CameraShaker camShake; // To be done later
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        //MyInput();


        //SetText
        text.SetText(bulletsLeft + "/" + magazineSize);
    }
    public void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        //if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();


        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;


        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);


            if (rayHit.collider.CompareTag("Enemy"))
            {
                //rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage); // To be done later
                GameObject obj = rayHit.collider.gameObject;
                Destroy(obj);

            }
        }


        //ShakeCamera
        //camShake.Shake(camShakeDuration, camShakeMagnitude); // To be done later


        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        muzzleFlash.Play();


        bulletsLeft--;
        bulletsShot--;


        Invoke("ResetShot", timeBetweenShooting);


        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    public void Reload()
    {

        if(bulletsLeft < magazineSize && !reloading){
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
