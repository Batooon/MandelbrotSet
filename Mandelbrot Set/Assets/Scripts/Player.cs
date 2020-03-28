using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class Player : MonoBehaviour
{
    public Vector2 Position;
    public float Scale = 3f;
    public float Angle;

    private Material mat;

    private Vector2 _smoothPos;
    private float _smoothScale;
    private float _smoothAngle;

    private void Awake()
    {
        mat = GetComponent<RawImage>().material;
    }

    private void FixedUpdate()
    {
        HandleInput();
        UpdateShader();
    }

    private void UpdateShader()
    {
        _smoothPos = Vector2.Lerp(_smoothPos, Position, .03f);
        _smoothScale = Mathf.Lerp(_smoothScale, Scale, .03f);
        _smoothAngle = Mathf.Lerp(_smoothAngle, Angle, .03f);

        float aspect = Screen.width / Screen.height;

        float scaleX = _smoothScale;
        float scaleY = _smoothScale;

        if (aspect > 1f)
            scaleY /= aspect;
        else
            scaleX *= aspect;

        mat.SetVector("_Area", new Vector4(_smoothPos.x, _smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", _smoothAngle);
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
            Scale *= 0.99f;
        if (Input.GetKey(KeyCode.KeypadMinus))
            Scale *= 1.01f;

        if (Input.GetKey(KeyCode.Q))
            Angle -= .01f;
        if (Input.GetKey(KeyCode.E))
            Angle += .01f;

        Vector2 dir = new Vector2(Scale * .01f, 0);
        float s = Mathf.Sin(Angle);
        float c = Mathf.Cos(Angle);
        dir = new Vector2(dir.x * c, dir.x * s);

        if (Input.GetKey(KeyCode.A))
            Position -= dir;
        if (Input.GetKey(KeyCode.D))
            Position += dir;

        dir = new Vector2(-dir.y, dir.x);
        if (Input.GetKey(KeyCode.S))
            Position -= dir;
        if (Input.GetKey(KeyCode.W))
            Position += dir;
    }
}
