using UnityEngine;
using System.Collections;

public class UI_TweenRotation : MonoBehaviour {
     public enum rotationAxial
    {
        x,y,z
    }
    public rotationAxial axial;
    public float speed = 150f;
    Vector3 angle;
    // Use this for initialization
    void Start()
    {
        angle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (axial==rotationAxial.x)
        {
            angle.x += Time.deltaTime * speed;
        }
        else if (axial == rotationAxial.y)
        {
            angle.y += Time.deltaTime * speed;
        }
        else if (axial == rotationAxial.z)
        {
            angle.z += Time.deltaTime * speed;
        }
        transform.eulerAngles = angle;
    }
}
