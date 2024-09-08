
namespace DastgyrAPI.Entity.HelperModels
{
    public enum OrderStatus
    {
        Pending = 0,
        InPreparation = 1,
        ReadyToShip = 2,
        InTransit = 3,
        Delivered = 4,
        Closed = 5,
        Cancelled = 6,
    }

    public enum OrderSellerStatus
    {
        Pending = 1,
        AwaitingPickup = 2,
        Shipped__ = 3,
        Ready = 4,
        Returned = 5,
        ReadyToPick = 6,
        PickUp = 7,
    }
}
