using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    private bool isInwater = false;
    private float countTime = 30.0f;
    public GameObject camera;
    public int speed;
    public GameObject waterUI;
    public Image o2;
    public Transform waterTransform;
    public BoxCollider waterCollider;
    public Transform skyCollider;
    public PostProcessVolume PostProcessVolume;
    public GameObject particle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        transform.Rotate(0, X_Rotation, 0);
        camera.transform.Rotate(-Y_Rotation, 0, 0);

        //addforceバージョン
        Vector3 forword = camera.transform.forward;
        Vector3 right = camera.transform.right;
        PlayerControll(KeyCode.W, forword);
        PlayerControll(KeyCode.S, -forword);
        PlayerControll(KeyCode.D, right);
        PlayerControll(KeyCode.A, -right);

        if (isInwater)
        {
            o2.fillAmount += O2Controller(-1.0f);
        }
        else
        {
            o2.fillAmount += O2Controller(10.0f);
        }

        PostProcessVolume.enabled = isInwater;
        waterUI.SetActive(isInwater);
        waterTransform.transform.GetChild(0).gameObject.SetActive(!isInwater);
        particle.SetActive(isInwater);
        float x = rb.position.x;
        waterTransform.position = new Vector3(x, 0, rb.position.z);
        skyCollider.position = new Vector3(rb.position.x, 1.5f, rb.position.z);
        waterCollider.center = new Vector3(0, Mathf.Clamp(rb.position.y, float.MinValue, -1), 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            isInwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            isInwater = false;
        }
    }

    float O2Controller(float f)
    {
        return f / countTime * Time.deltaTime;
    }

    void PlayerControll(KeyCode key, Vector3 direction)
    {
        if (Input.GetKey(key))
        {
            rb.AddForce(direction * speed, ForceMode.Force);
        }
    }
}