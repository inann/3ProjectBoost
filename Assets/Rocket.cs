using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    float rotationspeed = 45.0f;
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
      if (Input.GetKeyDown(KeyCode.Space)) {
        audiosource.Play();
      }

      if (Input.GetKeyUp(KeyCode.Space)) {
        IEnumerator fd = FadeOut(audiosource, fadeDuration);
        StartCoroutine(fd);
      }

      if (Input.GetKey(KeyCode.Space)) {
        print("Thrust");
        rigidbody.AddRelativeForce(Vector3.up);
      }

      rigidbody.freezeRotation = true;
      if (Input.GetKey(KeyCode.A)) {
        if (!Input.GetKey(KeyCode.D)) {
          transform.Rotate(rotationspeed * Vector3.forward * Time.deltaTime);
        }
      }
      else if (Input.GetKey(KeyCode.D)){
        if (!Input.GetKey(KeyCode.A)) {
          transform.Rotate(-(rotationspeed * Vector3.forward * Time.deltaTime));
        }
      }

      rigidbody.freezeRotation = false;
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
