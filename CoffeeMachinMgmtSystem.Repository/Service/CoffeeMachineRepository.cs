namespace CoffeeMachinMgmtSystem.Repository
{
    /// <summary>
    /// Coffee Machine Repository Class
    /// </summary>
    public class CoffeeMachineRepository : ICoffeeMachineRepository
    {
        private int count;

        /// <summary>
        /// Coffee Machine Repository
        /// </summary>
        public CoffeeMachineRepository() {
            count = 1;
        }

        /// <summary>
        /// Get Brew Coffee Count
        /// </summary>
        /// <returns>int</returns>
        public int GetBrewCoffeeCount()
        {
            return count;
        }

        /// <summary>
        /// Increment Brew Coffee Count
        /// </summary>
        /// <returns>int</returns>
        public int IncrementBrewCoffeeCount()
        {
            return count++;
        }
    }
}
