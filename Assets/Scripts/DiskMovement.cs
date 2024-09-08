/**Alexander Amsenius - Junior Unity Developer - Clay AB**/
/**Kontrollerar diskarnas och pålarnas rörelser**/

using UnityEngine;

public class DiskMovement : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public GameController gameController; // Referens till GameController

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }

    void OnMouseUp()
    {
        // Hitta den närmaste staven (Pole) när musen släpps
        Transform nearestPole = FindNearestPole();
        if (nearestPole != null)
        {
            // Anropa MoveDisk-funktionen i GameController för att flytta disken
            gameController.MoveDisk(gameObject, nearestPole);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Kontrollera om disken träffar en annan disk
        if (other.gameObject.CompareTag("Disk"))
        {
            // Hantera hur diskarna staplas eller interagerar med varandra
        }
    }


    // Funktion för att hitta den närmaste staven (Pole)
    Transform FindNearestPole()
    {
        GameObject[] poles = GameObject.FindGameObjectsWithTag("Pole");
        Transform nearestPole = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject pole in poles)
        {
            float distance = Vector3.Distance(transform.position, pole.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPole = pole.transform;
            }
        }

        return nearestPole;
    }
}

