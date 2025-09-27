



public class Hourglass : Pickable
{
    override public void PickUp(Player player)
    {
        base.PickUp(player);
        FindAnyObjectByType<LevelManager>().IncreaseTime();
    }
}
