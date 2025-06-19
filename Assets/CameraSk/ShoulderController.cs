using UnityEngine;

public class ShoulderController : MonoBehaviour
{

    public Transform puntoHombroIzquierdo;
    public Transform puntoHombroDerecho;

    void Update()
    {
        if (puntoHombroIzquierdo == null || puntoHombroDerecho == null)
        {
            return; 
        }

        Vector3 posicionCentral = (puntoHombroIzquierdo.position + puntoHombroDerecho.position) / 2f;
        transform.position = posicionCentral;


        Vector3 direccionHombros = puntoHombroDerecho.position - puntoHombroIzquierdo.position;

 
        if (direccionHombros != Vector3.zero)
        {
            Quaternion rotacionDeInclinacion = Quaternion.LookRotation(Vector3.forward, direccionHombros);
            transform.rotation = rotacionDeInclinacion;
        }
    }
}
