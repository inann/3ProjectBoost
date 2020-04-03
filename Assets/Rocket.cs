using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1f;
    float fadeDuration = 0.75f;
    AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
      rigidbody = GetComponent<Rigidbody>();
      audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      ProcessInput();
    }

    private void ProcessInput() {
      Thrust();
      Rotate();
    }

    private void Thrust() {
      if (Input.GetKeyDown(KeyCode.Space)) {
        audiosource.Play();
      }

      if (Input.GetKeyUp(KeyCode.Space)) {
        IEnumerator fd = FadeOut(audiosource, fadeDuration);
        StartCoroutine(fd);
      }

      if (Input.GetKey(KeyCode.Space)) {
        print("Thrust");
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
      }
    }

    private void Rotate() {
      rigidbody.freezeRotation = true;
      float rotationThisFrame = rcsThrust * Time.deltaTime;

      if (Input.GetKey(KeyCode.A)) {
        if (!Input.GetKey(KeyCode.D)) {
          transform.Rotate(Vector3.forward * rotationThisFrame);
        }
      }
      else if (Input.GetKey(KeyCode.D)){
        if (!Input.GetKey(KeyCode.A)) {
          transform.Rotate(-(Vector3.forward * rotationThisFrame));
        }
      }

      rigidbody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision) {
      // Check the collision object for tags
      switch (collision.gameObject.tag) {
        case "Friendly":
          {
            print("Friendly");
            break;
          }
        default:
          {
            print("Death");
            break;
          }
      }
    }

    public static IEnumerator FadeOut (AudioSource ad, float fadetime) {
      float startVol = ad.volume;

      while (ad.volume > 0) {
        ad.volume -= startVol * Time.deltaTime / fadetime;

        yield return null;
      }

      ad.Stop();
      ad.volume = startVol;
    }
  }
