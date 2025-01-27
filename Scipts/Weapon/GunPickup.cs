using HCID274._Weapons;
using UnityEngine;


public class GunPickup : MonoBehaviour, ILootable
{
    [SerializeField] private Gun gun; 

    
    public void OnStartLook()
    {
    //Debug.Log($"Started looking at {gun.gunName}!");
    }

    
    public void OnInteract()
    {
        //Debug.Log($"Trying to pick up {gun.gunName}!");
        WeaponHandler.instance.PickUpGun(gun);
        Destroy(gameObject); 
    }

    
    public void OnEndLook()
    {
        //Debug.Log($"Stopped looking at {gun.gunName}!");
    }
}
