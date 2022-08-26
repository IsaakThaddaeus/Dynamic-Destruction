using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    List<Pice> Pices = new List<Pice>();

    public float Breakeforce;
    public float BreakeTorque;
    public float partMass;

    public bool gravity;
    public bool fixBottomParts;

    public ParticleSystem Dust;
    public PhysicMaterial physicMaterial;
    void Awake()
    {
        instanciateParts();
        StartCoroutine(waitForPices());
    }

    void instanciateParts()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.AddComponent<Rigidbody>().useGravity = false;
            child.gameObject.GetComponent<Rigidbody>().mass = partMass;
            child.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            child.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            child.gameObject.AddComponent<MeshCollider>().convex = true;
            child.gameObject.GetComponent<MeshCollider>().material = physicMaterial;

            child.gameObject.AddComponent<Pice>().fixBottomParts = this.fixBottomParts;
            child.gameObject.AddComponent<Pice>().Dust = this.Dust;
            Pices.Add(child.gameObject.GetComponent<Pice>());
        }
    }

    IEnumerator waitForPices()
    {
        yield return new WaitForSeconds(0.15f);

        EvaluateFjs();
        makeFjs();

        if(gravity == true)
        {
            activateGravity();
        }
    }
    void EvaluateFjs()
    {
        foreach(Pice pice in Pices)
        {
            foreach(Pice fj in pice.FixedJoints)
            {
                fj.FixedJoints.Remove(pice);
            }
        }
    }
    void makeFjs()
    {
            foreach (Pice pice in Pices)
            {
                foreach (Pice joint in pice.FixedJoints)
                {
                    ConfigurableJoint cj = pice.gameObject.AddComponent<ConfigurableJoint>();
                    cj.connectedBody = joint.gameObject.GetComponent<Rigidbody>();

                    cj.breakForce = Breakeforce;
                    cj.breakTorque = BreakeTorque;
                    cj.enablePreprocessing = false;

                    cj.xMotion = ConfigurableJointMotion.Locked;
                    cj.yMotion = ConfigurableJointMotion.Locked;
                    cj.zMotion = ConfigurableJointMotion.Locked;

                    cj.angularXMotion = ConfigurableJointMotion.Locked;
                    cj.angularYMotion = ConfigurableJointMotion.Locked;
                    cj.angularZMotion = ConfigurableJointMotion.Locked;

                    cj.projectionMode = JointProjectionMode.PositionAndRotation;
                    cj.projectionDistance = 0;
                    cj.projectionAngle = 0;
                    
                }
            }
        }
    
    void activateGravity()
    {
        foreach (Pice pice in Pices)
        {
            pice.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
