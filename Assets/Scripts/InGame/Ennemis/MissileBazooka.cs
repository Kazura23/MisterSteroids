using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBazooka : MonoBehaviour {

    private Rigidbody rigiSelf;
    private Vector3 direction, save;
    public int vitesse = 7, tempsExistance = 10;
    private bool active;
    private float facteurVite;
    private bool isReturn;
    private Collider coll;
    private Transform SapwnPos;
    public float projection;

	// Use this for initialization
	void Start () {
        active = false; isReturn = false;
        StartCoroutine("TempsDeVie");
    }
	

    public void ActiveTir(Vector3 p_dir, float p_facteur, bool p_Return)
    {
        if (!isReturn)
        {
            isReturn = p_Return;
            direction = p_dir;
            facteurVite = p_facteur;
            if (rigiSelf == null)
            {
                rigiSelf = GetComponent<Rigidbody>();
            }
            if (coll == null)
            {
                coll = GetComponent<Collider>();
            }
            rigiSelf.velocity = direction * vitesse * facteurVite;
            save = rigiSelf.velocity;
            active = true;
        }
    }

    private IEnumerator TempsDeVie()
    {
        yield return new WaitForSeconds(tempsExistance);
        Explosion();
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void Explosion()
    {
        //particule explosion
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Untagged" || collision.gameObject.tag == Constants._MissileBazoo)
        {
            Explosion();
        }else if(collision.gameObject.tag == Constants._EnnemisTag && isReturn)
        {
            Debug.Log("explo");
            collision.gameObject.GetComponentInChildren<AbstractEnnemis>().Degat(transform.forward * projection, 1);
            Explosion();
        }else if (collision.gameObject.tag == Constants._EnnemisTag && !isReturn)
        {
            coll.isTrigger = true;
            rigiSelf.velocity = save;
        }
    }

    void OnTriggerExit(Collider other)
    {
        coll.isTrigger = false;
    }

}
