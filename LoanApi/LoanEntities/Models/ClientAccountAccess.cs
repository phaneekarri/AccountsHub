namespace LoanEntities.Models
{
    public class ClientAccountAccess
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Account Account { get; set; }
        public bool isActive { get; set; }
        public bool ClientAccessType { get; set; }
    }
}
