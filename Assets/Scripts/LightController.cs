using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    //public GameObject Camera;
    //[SerializeField] private VolumetricLight Light;
    //[SerializeField] private VolumetricLightRenderer Renderer;
    //private Texture Texture;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Camera = FindObjectOfType<MouseLook>().gameObject;
    //    Light = gameObject.GetComponent<VolumetricLight>();
    //    Renderer = Camera.GetComponent<VolumetricLightRenderer>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if(Camera == null)
    //    {
    //        Camera = FindObjectOfType<MouseLook>().gameObject;
    //        Renderer = Camera.GetComponent<VolumetricLightRenderer>();
    //    }
    //    var angle = Vector3.Angle(transform.forward, Camera.transform.forward);
    //    Vector3 point = -transform.forward * 1000f + Camera.transform.position;
    //    Ray ray = new Ray()
    //    {
    //        origin = point,
    //        direction = transform.forward
    //    };
    //    Debug.Log(ray);
    //    Debug.DrawRay(point, transform.forward, Color.red);
    //    bool ray1 = Physics.Raycast(ray, out RaycastHit hit);
    //    Debug.Log(hit.collider);
    //    if(ray1)
    //    {
    //        Light.enabled = true;
    //        Renderer.Resolution = VolumetricLightRenderer.VolumtericResolution.Full;
    //    } else
    //    {
    //        Light.enabled = false;
    //        Renderer.Resolution = VolumetricLightRenderer.VolumtericResolution.Disabled;
    //    }
    //}
}
