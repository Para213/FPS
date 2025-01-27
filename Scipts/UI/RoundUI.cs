using System.Collections;
using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundText; 
    [SerializeField] AudioClip roundAudioClip; 
    [SerializeField] float roundTextDisplayTime = 1.0f; 
    [SerializeField] float roundTextFadeDuration = 2.0f;

    private CanvasGroup canvasGroup; 

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); 
    }

    public IEnumerator ShowRoundText(int currentRound)
    {
        roundText.text = "Round " + currentRound; 
        AudioSource.PlayClipAtPoint(roundAudioClip, Camera.main.transform.position); 

        canvasGroup.alpha = 1; 
        yield return new WaitForSeconds(roundTextDisplayTime); 

        float elapsedTime = 0;
        while (elapsedTime < roundTextFadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / roundTextFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0; 
    }
}
