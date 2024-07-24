using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Timer clocktimer;

    // Start is called before the first frame update
    void Start()
    {
        clocktimer = GetComponent<Timer>();
    }

    // [SerializeField] float speed = 0.0f;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, clocktimer.clockspeed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
