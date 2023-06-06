// See https://aka.ms/new-console-template for more information

/*
    TODO:
    [x] Create a way to store, display and delete items to the productInventory
    [x] Store the information as a CSV text file in the root directory to read on load time and to write changes to.
    [ ] Edit price menu to change the price comfortably.
    [ ] Bugfixes to catch exceptions
*/
using System;
using System.Globalization;
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

        static CultureInfo culture = new CultureInfo("es-ES");

        private static Dictionary<string, decimal> productInventory = new Dictionary<string, decimal>();
        static string welcomeMessage = "Welcome to Dynamics Inventory System.";
        static string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n [4] Exit Inventory system";
        static string optionErrorMessage = "Option must be a number";
        static string emptyErrorMessage = "You have to write a valid option.";
        static string exitMessage= "Thanks for using our Inventory System.\nHave a great day!";
        static void Main()
        {
            ProgramState currentState = ProgramState.MainMenu;
            LoadFile();
            while (currentState != ProgramState.Exit)
            {
                currentState = MenuControl(currentState);
            }
            Clear();
            WriteFile();
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
                case ProgramState.DisplayInventory:
                    DisplayInventory();
                    return ProgramState.MainMenu;
                case ProgramState.DeleteProduct:
                    DeleteProduct();
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

        static void DisplayInventory()
        {
            Clear();
            WriteLine("List of products and prices.\n");
            WriteLine("|       Product name       |  Price  |");
            WriteLine("|--------------------------|---------|");
            foreach (var product in productInventory)
            {
                WriteLine("|{0,-26}|${1,8:N2}|", product.Key, productInventory[product.Key]);
                WriteLine("|--------------------------|---------|");
            }
            ReadKey();
        }

        static void DeleteProduct()
        {
            Clear();
            WriteLine("Delete product mode.");
            Write("Product to delete: ");
            string? product = null;
            while (product == null)
            {
                product = ReadLine();
                if (product == null)
                {
                    WriteLine(emptyErrorMessage);
                    continue;
                }
                break;
            }
            
            if (!productInventory.ContainsKey(product))
            {
                WriteLine("Product is not in the inventory.");
                ReadKey();
                return;
            }
            else
            {
                WriteLine("You're about to delete {0}.\nAre you sure you want to continue?\n[1] Yes\n[2] No", product);
                int option = GetMenuOption();
                switch (option)
                {
                    case 1:
                        break;
                    case >= 2:
                    default:
                        WriteLine("Nothing was deleted.");
                        ReadKey();
                        return;
                }
                productInventory.Remove(product);
                WriteLine("{0} has been removed from inventory.", product);
                ReadKey();
                return;
            }
        }

        static void LoadFile(string filePath = "inventory.csv")
        {
            if (File.Exists(filePath))
            {
                using (StreamReader inventoryFile = new StreamReader(filePath, true))
                {
                    while (inventoryFile.Peek() >= 0)
                    {
                        string? line = inventoryFile.ReadLine();
                        if (line != null)
                        {
                            string[] pair = line.Split(";");
                            productInventory.Add(pair[0], decimal.Parse(pair[1], NumberStyles.AllowDecimalPoint, culture));
                        }
                        else
                        {
                            break;
                        }
                    }
                    return;
                }
            }
            else
            {
                File.CreateText(filePath);
                return;
            }
        }

        static void WriteFile(string filePath = "inventory.csv")
        {
            using (StreamWriter inventoryFile = new StreamWriter(filePath))
            {
                foreach (var product in productInventory)
                {
                    inventoryFile.WriteLine("{0};{1}", product.Key, product.Value.ToString(culture));
                }
            }
            return;
        }
    }
}