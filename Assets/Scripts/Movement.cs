using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float mainThrusters = 500f;
    public float rotateThrusters = 110f;
    public AudioClip audioMainThrusters;
    public ParticleSystem particleMainThrusters;
    public ParticleSystem particleRotateThrustersLeft;
    public ParticleSystem particleRotateThrustersRight;

    public GameObject destinationObject;

    public int lengthOfLineRenderer = 2;
    public Color c1 = Color.yellow;
    public Color c2 = Color.green;

    Rigidbody rb;
    Transform tr;

    LineRenderer lineRenderer;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, .5f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, .5f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust(); 
        ProcessRotate();
        var points = new Vector3[lengthOfLineRenderer];
        points[0] = tr.position;
        points[1] = destinationObject.transform.position;
        lineRenderer.SetPositions(points);
            
    }

    void ProcessThrust(){

        if(Input.GetKey(KeyCode.Space)){
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotate(){

        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D)){
            RotateRight();
        }else{
            StopRotation();
        }
    }

    private void RotateThrusters(float rotateThrusters)
    {
        rb.freezeRotation = true; //Freeze rotation so that we can manually rotate is
        tr.Rotate(Vector3.forward * rotateThrusters * Time.deltaTime);
        rb.freezeRotation = false; //Un - Freeze rotation so that physics system can continue calculating the rotation
    }

    private void StartThrusting()
    {

            rb.AddRelativeForce(Vector3.up * mainThrusters * Time.deltaTime);
            if(!audioSource.isPlaying){
                audioSource.PlayOneShot(audioMainThrusters);
            }
            if(!particleMainThrusters.isPlaying){
                particleMainThrusters.Play();
            }
    }

    private void StopThrusting()
    {

            particleMainThrusters.Stop();
            audioSource.Stop();
    }

    private void RotateLeft(){
            RotateThrusters(rotateThrusters);
            if(!particleRotateThrustersRight.isPlaying){
                particleRotateThrustersRight.Play();
            }
    }

    private void RotateRight(){
            RotateThrusters(-rotateThrusters);
            if(!particleRotateThrustersLeft.isPlaying){
                particleRotateThrustersLeft.Play();
            }
    }

    private void StopRotation(){
        particleRotateThrustersRight.Stop();
        particleRotateThrustersLeft.Stop();
    }
}
