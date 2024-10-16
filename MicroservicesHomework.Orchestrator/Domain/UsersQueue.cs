namespace MicroservicesHomework.Orchestrator.Domain
{
    public class UsersQueue : Dictionary<string, List<long>>
    {
        public const string ProductFromServiceAName = "Молоко";
        public const string ProductFromServiceBName = "Стул";

        public UsersQueue()
        {
            Add(ProductFromServiceAName, [1, 2, 3, 4, 5]);
            Add(ProductFromServiceBName, [5, 2, 3, 1]);
        }

        public void MoveToEnd(string productName, long clientId)
        {
            if (!TryGetValue(productName, out List<long> list))
                return;

            list.Remove(clientId);
            list.Add(clientId);
        }
    }
}
