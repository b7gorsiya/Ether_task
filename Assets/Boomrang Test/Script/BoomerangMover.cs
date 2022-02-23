using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangMover : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    float rotateSpeed = 1.5f;

    //store the refrence for rigidbody
    Rigidbody _rigidbody;
    //store init position of player
    Vector3 _player_position;
    // used for boomerang
    Vector3 playerPos;
    //store init rotation of player
    Vector3 _player_rotation;

    // line renderer
    LineRenderer _lineRnd;

    //initial of boomerang
    float heigtY;
    //store postions for line to draw
    List<Vector3> linePos;


    private void Start()
    {
        //inti list
        linePos = new List<Vector3>();
        _rigidbody = this.GetComponent<Rigidbody>();

        //save palyer pos and rot
        _player_position = this.transform.position;
        playerPos = _player_position;
        playerPos.y = 0;
        _player_rotation = this.transform.rotation.eulerAngles;

        //store line renderer
        _lineRnd = this.gameObject.GetComponent<LineRenderer>();

        //default player height
        heigtY = transform.localPosition.y;
    }

    private void OnEnable()
    {
        //suscribes event
        TouchController.DrawProjectile += PredicateProjectiles;
        TouchController.Throw_Boomer += Throw;
    }

    void PredicateProjectiles(float _width, float _farValue, float _time)
    {
        // clear the list
        linePos.Clear();

        float timer = 0.0f;
        while (timer < _time)
        {
            //applying the ellipse equation
            float t = Mathf.PI * 2.0f * timer / _time - Mathf.PI / 2.0f;
            float x = _width * Mathf.Cos(t);
            float z = _farValue * Mathf.Sin(t);

            //predict the position
            Vector3 v = new Vector3(x, heigtY, z + _farValue);
            //store in list
            linePos.Add(v);
            timer += Time.deltaTime;
        }
        DrawProjectile(linePos);
    }



    void Throw(float _width, float _farValue, float _time)
    {
        StartCoroutine(Throw(_farValue, _width, _player.transform.forward, _time));
    }

    IEnumerator Throw(float dist, float width, Vector3 direction, float time)
    {
        // geting moving direction of boomrang based on players rotation
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, direction);
        float timer = 0.0f;
        Quaternion r = _rigidbody.rotation;
        while (timer < time)
        {
            // the PI ellipse equation
            float t = Mathf.PI * 2.0f * timer / time - Mathf.PI / 2.0f;
            float x = width * Mathf.Cos(t);
            float z = dist * Mathf.Sin(t);
            Vector3 v = new Vector3(x, heigtY, z + dist);
            //give spin effect on boomerang
            _rigidbody.MoveRotation(Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y * (v.z * rotateSpeed), r.eulerAngles.z));
            // move boomerang towords the predicted point
            _rigidbody.MovePosition(playerPos + (q * v));
            timer += Time.deltaTime;
            yield return null;
        }
        //set default value on boomerang come back
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.rotation = Quaternion.Euler(_player_rotation);
        _rigidbody.MovePosition(_player_position);

        //hide the linerandrer
        HideProjectiles();
    }

    private void HideProjectiles()
    {
        _lineRnd.positionCount = 0;
        linePos.Clear();

    }
    private void DrawProjectile(List<Vector3> pos)
    {
        _lineRnd.positionCount = 0;
        _lineRnd.startWidth = .2f;
        _lineRnd.endWidth = .2f;
        _lineRnd.positionCount = pos.Count;
        _lineRnd.SetPositions(pos.ToArray());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="others")
        {
            collision.gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        }
    }
    private void OnDisable()
    {
        //unsuscribe events
        TouchController.DrawProjectile -= PredicateProjectiles;
        TouchController.Throw_Boomer -= Throw;
    }
}

