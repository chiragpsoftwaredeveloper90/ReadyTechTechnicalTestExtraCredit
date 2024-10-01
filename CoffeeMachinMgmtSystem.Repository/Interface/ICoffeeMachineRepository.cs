namespace CoffeeMachinMgmtSystem.Repository
{
    /// <summary>
    /// Coffee Machine Repository interface
    /// </summary>
    public interface ICoffeeMachineRepository
    {
        /// <summary>
        /// Get Brew Coffee Count
        /// </summary>
        /// <returns>int</returns>
        public int GetBrewCoffeeCount();

        /// <summary>
        /// Increment Brew Coffee Count
        /// </summary>
        /// <returns>int</returns>
        public int IncrementBrewCoffeeCount();
    }
}
