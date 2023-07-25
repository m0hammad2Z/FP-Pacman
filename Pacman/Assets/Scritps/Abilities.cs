using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
[Header("Dash")]
    [SerializeField] float dashSpeed = 5;
    [SerializeField] float dashDuration;
    [HideInInspector] public bool isDashing;

[Header("Speed Boost")]
    [SerializeField] float boostMovementSpeed = 40;
    public bool isBoosting;
    float tempSpeed;

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

}
