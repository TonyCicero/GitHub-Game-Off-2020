using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pcontroller : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    Rigidbody rb;

    [SerializeField]
    float camSens = 0.25f; //camera sensitivity
    private Vector3 lastMouse = new Vector3(0, 0, 0);

    [SerializeField]
    console con;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (con.UsingTerminal) //if using terminal, dont allow movement
        {
            return;
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }


        Vector3 movement = Vector3.zero;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        movement += transform.forward * v * playerSpeed * Time.deltaTime;
        movement += transform.right * h * playerSpeed * Time.deltaTime;
        movement += Physics.gravity;

        controller.Move(movement);


        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;
    }
}
