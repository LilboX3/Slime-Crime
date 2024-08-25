using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashController : MonoBehaviour
{
    [SerializeField]
    private int scoreAmount;
    private ParticleSystem particles;
    private SlimeController slimeController;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPoints()
    {
        return scoreAmount;
    }

    void OnTriggerEnter(UnityEngine.Collider other)
    {
        Debug.Log("collided a: "+other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            slimeController = other.gameObject.GetComponentInParent<SlimeController>();
            slimeController.AddPoints(scoreAmount);
            particles.Play();
            StartCoroutine(DestroyAfterSeconds(0.35f));
        }
    }

    IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
