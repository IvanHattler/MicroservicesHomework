namespace MicroservicesHomework.Orchestrator.Domain
{
    public class UsersQueue : Dictionary<string, List<long>>
    {
        public UsersQueue()
        {
            Add("Product1", [1, 2, 3, 4, 5]);
            Add("Product2", [4, 3]);
            Add("Product3", [5, 2, 3, 1]);
            Add("Product4", [3, 1, 5]);
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
