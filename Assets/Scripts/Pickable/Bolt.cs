public class Bolt : Pickable
{
    override public void PickUp(Player player)
    {
        base.PickUp(player);
        player.IncreaseMoveSpeed();
    }
}