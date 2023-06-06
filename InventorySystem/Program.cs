// See https://aka.ms/new-console-template for more information

/*
    TODO:
    - Create a way to store, display and delete items to the productInventory
    - Store the information as a CSV text file in the root directory to read on load time and to write changes to.
    - Edit price menu to change the price comfortably.
*/
using static System.Console;

namespace InventorySystem
{
    public class Inventory
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
        
        private static Dictionary<string, decimal> productInventory = new Dictionary<string, decimal>();
        static string welcomeMessage = "Welcome to Dynamics Inventory System.";
        static string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n [4] Exit Inventory system";
        static string optionErrorMessage = "Option must be a number";
        static string emptyErrorMessage = "You have to write a valid option.";
        static string exitMessage= "Thanks for using our Inventory System.\nHave a great day!";
        static void Main()
        {
            ProgramState currentState = ProgramState.MainMenu;
            while (currentState != ProgramState.Exit)
            {
                currentState = MenuControl(currentState);
            }
            Clear();
            WriteLine(exitMessage);
            ReadKey();
        }

        static ProgramState MenuControl(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.MainMenu:
                    Console.Clear();
                    WriteLine(welcomeMessage);
                    WriteLine(mainMenuOptions);
                    int option = GetMenuOption();
                    return ChangeStateMainMenu(option);
                case ProgramState.AddProduct:
                    AppendNewProduct();
                    return ProgramState.MainMenu;
                default:
                    return ProgramState.Exit;
            }
        }
        
        static int GetMenuOption()
        {
            int option = 0;
            while (true)
            {
                string? userAnswer = ReadLine();
                if (userAnswer == null)
                {
                    WriteLine(emptyErrorMessage);
                    continue;
                }
                if (!int.TryParse(userAnswer, out option))
                {
                    WriteLine(optionErrorMessage);
                    continue;
                }
                else
                {
                    return option;
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

        static void AppendNewProduct()
        {
            Clear();
            WriteLine("Add product mode.");
            Write("Product [Name]: ");
            string? name = null;
            while (name == null)
            {
                name = ReadLine();
                if (name != null)
                {
                    break;
                }
                else
                {
                    WriteLine(emptyErrorMessage);
                }
            }
            
            Write("Product [Price]: ");
            decimal price;
            while (!decimal.TryParse(ReadLine(), out price))
            {
                WriteLine("You must enter a number.");
            }

            productInventory.Add(name, price);
        }
    }
}