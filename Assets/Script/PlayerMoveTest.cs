using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    [SerializeField] float jumpPwoer = 1.0f;
    float x, z;
    float run = 1;


    public GameObject cam;
    Quaternion cameraRot, characterRot;
    float Xsensityvity = 3f, Ysensityvity = 3f;
    Rigidbody rb;

    bool cursorLock = true;

    bool _canMove = true;

    float minX = -90f, maxX = 90f;
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
        if (_canMove)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot, 0);

            //Updateの中で作成した関数を呼ぶ
            cameraRot = ClampRotation(cameraRot);

            cam.transform.localRotation = cameraRot;
            transform.localRotation = characterRot;


            UpdateCursorLock();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jumpPwoer, 0);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            run = 2;
        }
        else
        {
            run = 1;
        }
    }

    private void FixedUpdate()
    {
        x = 0;
        z = 0;

        if (_canMove)
        {
            x = Input.GetAxisRaw("Horizontal") * speed;
            z = Input.GetAxisRaw("Vertical") * speed;
        }

        rb.velocity += cam.transform.forward * z * run;
        rb.velocity += cam.transform.right * x * run;
        //rb.velocity -= cam.transform.up * z * x * run;
        //rb.velocity -= new Vector3(0, cam.transform.forward.y + cam.transform.right.y, 0);
        rb.AddForce(0, -9.8f * 5, 0);
    }


    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if (Input.GetMouseButton(0))
        {
            cursorLock = true;
        }


        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //角度制限関数の作成
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }


}
