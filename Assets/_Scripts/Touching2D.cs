using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Use (Ground, Wall, and Ceiling) to check for current collisions.
/// </summary>
public class Touching2D : MonoBehaviour
{
    private const float WALL_RANGE = 0.1f;
    public bool Ground { private set; get; }
    public bool Wall { private set; get; }
    public bool Ceiling { private set; get; }
    private List<ContactPoint2D> _contactList;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _contactList = new List<ContactPoint2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        CheckTouches(_rigidbody);
    }

    private void CheckTouches(Rigidbody2D body)
    {
        Ground = false;
        Wall = false;
        Ceiling = false;
        body.GetContacts(_contactList);
        foreach (ContactPoint2D n in _contactList)
        {
            CalculateTouch(n.normal, true);
        }
    }

    private void CalculateTouch(Vector2 normal, bool touchSet)
    {
        float detection = Vector2.Dot(normal, Vector2.up);
        if (Mathf.Abs(detection) < WALL_RANGE)
        {
            this.Wall = touchSet;
        }
        else if (detection > 0f)
        {
            this.Ground = touchSet;
        }
        else
        {
            this.Ceiling = touchSet;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckTouches(_rigidbody);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckTouches(_rigidbody);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CheckTouches(_rigidbody);
    }
}