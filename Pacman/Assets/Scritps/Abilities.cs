using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Abilities : MonoBehaviour
{
[Header("Dash")]
    [SerializeField] float dashSpeed = 5;
    [SerializeField] float dashDuration;
    [HideInInspector] public bool isDashing;

[Header("Speed Boost")]
    [SerializeField] float boostMovementSpeed = 50;
    [HideInInspector] public bool isBoosting;
    float tempSpeed;

[Header("Throug Wall")]
    [SerializeField] float radius;
    [SerializeField] float changeDuration = 5; 
    [SerializeField] Material transparentWallMat; [SerializeField] Material wallMat; [SerializeField] Material glowWallMat;
    [HideInInspector] public bool canSeeThroughWalls = false;


    private void Start()
    {
        tempSpeed = GetComponent<Movement>().movementSpeed;
    }

    public void Dash(Rigidbody rb, Vector3 dir)
    {
        if (isDashing || PacmanManager.dashes <= 0) return;
        isDashing = true;
        PacmanManager.dashes -= 1;
        Vector3 forceDir = dir.normalized == Vector3.zero ? transform.forward : dir.normalized;
        rb.AddForce(forceDir * dashSpeed + Vector3.up * 1.5f, ForceMode.VelocityChange);
        StartCoroutine(reset());
        IEnumerator reset()
        {
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
        }
    }

    public void SpeedBoost()
    {
        GetComponent<Movement>().movementSpeed = tempSpeed;
        StartCoroutine(boost());
        IEnumerator boost()
        {
            GetComponent<Movement>().movementSpeed += boostMovementSpeed;
            isBoosting = true;
            yield return new WaitForSeconds(PacmanManager.boostAbilityTime);
            GetComponent<Movement>().movementSpeed = tempSpeed;
            isBoosting = false;
        }
    }

    public void SeeThroughWalls()
    {
        StartCoroutine(ThrougWalls());
        IEnumerator ThrougWalls()
        {
            canSeeThroughWalls = true;
            yield return new WaitForSeconds(PacmanManager.throughWallsDuration);
            canSeeThroughWalls = false;
        }
    }

    public void ThroughWallsMethod()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.GetChild(1).position, radius);

        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to a wall (you can use tags or layers here)
            if (collider.CompareTag("Wall"))
            {
                // Get the wall's renderer component
                Renderer wallRenderer = collider.GetComponent<Renderer>();

                // Change the material to the new one
                wallRenderer.material = transparentWallMat;

                // Start a coroutine to revert the material after a delay
                StartCoroutine(RevertMaterialCoroutine(wallRenderer));
            }
        }

    }


    private IEnumerator RevertMaterialCoroutine(Renderer renderer)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(changeDuration);

        // Revert the material to the original one
        renderer.material = wallMat;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.GetChild(1).position, radius);
    }


    private void Update()
    {
        if(canSeeThroughWalls)
        {
            ThroughWallsMethod();
        }
    }

}
