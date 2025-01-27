using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HCID274._UI
{
    public class AmmunitionUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ammunitionTypeText; 
        [SerializeField] TextMeshProUGUI ammunitionCountText; 

        private CanvasGroup canvasGroup; 

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>(); 
        }

        
        public void UpdateAmmunitionType(Gun gun)
        {
            if (gun == null)
            {
                canvasGroup.alpha = 0; 
                return;
            }

            canvasGroup.alpha = 1; 

            
            UpdateAmmunitionCount(AmmunitionManager.instance.GetAmmunitionCount(gun.ammunitionType));

            
            ammunitionTypeText.text = gun.ammunitionType.ToString();
        }

        
        public void UpdateAmmunitionCount(int newCount)
        {
            ammunitionCountText.text = newCount.ToString();
        }
    }
}

