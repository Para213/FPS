using UnityEngine;


public class SemiAutomatic : Gun
{

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            if (Time.time - timeOfLastShot >= 1 / fireRate)
            {

                Fire();

                timeOfLastShot = Time.time;
            }
        }
    }
}