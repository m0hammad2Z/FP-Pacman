using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public void DestroyDot()
    {
        Destroy(this.gameObject);
        PacmanManager.score++;
    }
}
