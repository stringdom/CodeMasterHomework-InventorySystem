// See https://aka.ms/new-console-template for more information
using static System.Console;

namespace InventorySystem
{
    static class Inventory
    {
        enum ProgramState
        {
            MainMenu,
            AddProduct,
            DeleteProduct,
            DisplayInventory,
            Exit,
            EditProduct
        }
        
        static string welcomeMessage = "Welcome to Dynamics Inventory System.";
        static string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n[4] Exit Inventory system";
        static void Main()
        {
            ProgramState currentState = ProgramState.MainMenu;
            while (currentState != ProgramState.Exit)
            {
                currentState = MenuControl(currentState);
            }
        }

        static ProgramState MenuControl(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.MainMenu:
                    WriteLine(Inventory.welcomeMessage);
                    int option = int.Parse(ReadLine());
                    return ProgramState.MainMenu;
                
                default:
                    return ProgramState.Exit;
            }
        }
    }
}