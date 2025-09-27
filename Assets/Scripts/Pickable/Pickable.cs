using UnityEngine;



public abstract class Pickable : MonoBehaviour
{
    public virtual void PickUp(Player player)
    {
        Destroy(this.gameObject);
    }

}


