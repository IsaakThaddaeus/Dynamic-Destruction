using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pice : MonoBehaviour
{

    public List<Pice> FixedJoints = new List<Pice>();
    public ParticleSystem Dust;


    public bool fixBottomParts;
    bool searchForNeighbors = true;
    
    void Start()
    {
        StartCoroutine(searchForNeiborsTimePeriode()); 
    }
    IEnumerator searchForNeiborsTimePeriode()
    {
        yield return new WaitForSeconds(0.1f);
        searchForNeighbors = false;
        yield return new WaitForSeconds(1f);
        fixBottomParts = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(searchForNeighbors == true)
        {
            FixedJoints.Add(collision.gameObject.GetComponent<Pice>());
        }
        if(fixBottomParts == true && collision.gameObject.layer == 7)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void OnJointBreak(float breakForce)
    {
        if(Dust != null)
        {
            Dust.transform.position = this.transform.position;
            Dust = Instantiate(Dust);
            Dust.Play();
        }
    }


}
