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
        static string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n [4] Exit Inventory system";
        static string emptyErrorMessage = "You have to write an option.";
        static string exitMessage= "Thanks for using our Inventory System.\nHave a great day!";
        static void Main()
        {
            ProgramState currentState = ProgramState.MainMenu;
            while (currentState != ProgramState.Exit)
            {
                currentState = MenuControl(currentState);
            }
            WriteLine(exitMessage);
            ReadKey();
        }

        static ProgramState MenuControl(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.MainMenu:
                    WriteLine(welcomeMessage);
                    WriteLine(mainMenuOptions);
                    int option = GetMenuOption();
                    return ChangeStateMainMenu(option);

                default:
                    return ProgramState.Exit;
            }
        }
        
        static int GetMenuOption()
        {
            while (true)
            {
                string? userAnswer = ReadLine();
                int option = 0;
                if (userAnswer == null)
                {
                    WriteLine(Inventory.emptyErrorMessage);
                    continue;
                }
                else if (int.TryParse(userAnswer, out option))
                {
                    return option;
                }
                else
                {
                    WriteLine("Option must be a number");
                    continue;
                }
            }
        }

        static ProgramState ChangeStateMainMenu(int option)
        {
            switch (option)
            {
                case 1:
                    return ProgramState.DisplayInventory;
                case 2:
                    return ProgramState.AddProduct;
                case 3:
                    return ProgramState.DeleteProduct;
                case >= 4:
                    return ProgramState.Exit;
                default:
                    return ProgramState.MainMenu;
            }
        }
    }
}