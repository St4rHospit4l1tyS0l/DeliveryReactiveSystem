namespace Drs.Model.Constants
{
    public class OrderStatus
    {
        public const string NEW_READY_TO_SEND = "NEW_READY_TO_SEND";
        public const string SENDING_TO_STORE = "SENDING_TO_STORE";
        public const string WAITING_FOR_FIRST_ANSWER = "WAITING_FOR_FIRST_ANSWER";
        public const string ACKNOWLEDGE_FROM_STORE = "ACKNOWLEDGE_FROM_STORE";
    }
}