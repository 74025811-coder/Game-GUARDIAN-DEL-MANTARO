using UnityEngine;
using UnityEngine.UI;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private PlayerInteractor playerInteractor;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float verticalOffset = 0.8f;

    private void Update()
    {
        Transform target = playerInteractor.CurrentTargetTransform;

        if (target != null)
        {
            promptRoot.SetActive(true);
            promptRoot.transform.position = target.position + Vector3.up * verticalOffset;
            progressBar.value = playerInteractor.CurrentProgress01;
        }
        else
        {
            promptRoot.SetActive(false);
        }
    }
}