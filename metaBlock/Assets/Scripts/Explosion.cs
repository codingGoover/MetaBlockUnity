using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    Material[] materials;
    public int materialCnt=10;
    int createCnt = 0;
   
    private  AudioSource musicPlayer;
   // public AudioClip clip;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    // Use this for initialization
    void Start()
    {
        materials = GetComponent<Renderer>().materials;
        musicPlayer = GetComponent<AudioSource>();


        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
        
    }

  //  float forceGravity = 50000f;
    private void FixedUpdate()
    {
       // GetComponent < Rigidbody>().AddForce(Vector3.down * forceGravity);
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Floor")
        {
            //playSound(clip, musicPlayer);

            explode();
        }

    }

    public void explode()
    {
        //make object disappear
        gameObject.SetActive(false);
       

        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }


    }

    void createPiece(int x, int y, int z)
    {

        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //set piece material
        Renderer rend = piece.GetComponent<Renderer>();
        rend.sharedMaterial = materials[createCnt%materialCnt];

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        createCnt++;
    }

    public static void playSound(AudioClip clip,AudioSource audioPlayer)
    {
        Debug.Log("오디오 나온다고");
        audioPlayer.Stop();
        audioPlayer.clip = clip;
        audioPlayer.time = 0;
        audioPlayer.Play();
    }

}