using Pustok.Database;

namespace Pustok.Services
{
    public class OrderIdentifierGenerator
    {
        private readonly PustokDbContext _pustokDbContext;

        public OrderIdentifierGenerator(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
        }

        public string GenerateOrderIdentifier()
        {

            Random random = new Random();

            int randomNumber = random.Next(10000, 99999);

            string orderIdentifier = $"OR{randomNumber}";

            while (_pustokDbContext.Orders.Any(e => e.TracingCode == orderIdentifier))
            {
                orderIdentifier = orderIdentifier.ToString();
            }

            return orderIdentifier;
        }


    }
}



