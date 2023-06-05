// See https://aka.ms/new-console-template for more information
namespace InventorySystem
{
    static class Inventory
    {
        enum ProgramState
        {
            MainMenu,
            AddProduct,
            DeleteProduct,
            DisplayInventory
        }
        
        static void Main()
        {
            Console.WriteLine("This runs!");
        }
    }
}