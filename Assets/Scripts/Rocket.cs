using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1f;
    // Audio Clip Fields
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip winJingle;

    // Particle System Fields
    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;

    float fadeDuration = 0.75f;
    AudioSource audiosource;
    public float delayTime = 3f;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start() {
      rigidbody = GetComponent<Rigidbody>();
      audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
      ProcessInput();
    }

    private void ProcessInput() {
      if (state == State.Alive) {
        Thrust();
        Rotate();
      }
    }

    private void Thrust() {
      if (Input.GetKey(KeyCode.Space)) {
        print("Thrust");
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        playThrustSound();
        thrustParticles.Play();
      } else {
        audiosource.Stop();
        thrustParticles.Stop();
      }
    }

    private void playThrustSound() {
      if (!audiosource.isPlaying) {
        audiosource.PlayOneShot(mainEngine);
      }
    }

    private void playWinCondition() {
      audiosource.Stop();
      audiosource.PlayOneShot(winJingle);
      thrustParticles.Stop();
      winParticles.Play();
    }

    private void playDeathsplosion() {
      audiosource.Stop();
      audiosource.PlayOneShot(explosion);
      thrustParticles.Stop();
      deathParticles.Play();
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
      if (state != State.Alive) {
        return;
      }
      // Check the collision object for tags
      switch (collision.gameObject.tag) {
        case "Friendly":
          {
            print("Friendly");
            break;
          }
        case "Finish":
          {
            state = State.Transcending;
            playWinCondition();
            Invoke("LoadNextScene", delayTime);
            break;
          }
        default:
          {
            state = State.Dying;
            playDeathsplosion();
            Invoke("LoadDeathScene", delayTime);
            break;
          }
      }
    }

    private void LoadDeathScene() {
      SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
      SceneManager.LoadScene(1);
    }

  }
