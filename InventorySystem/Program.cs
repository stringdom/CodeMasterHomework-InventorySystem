// See https://aka.ms/new-console-template for more information

/*
    TODO: [ ] Refactor this program to use the OOP patterns.
    [ ] CRUD: Create, Read, Update, Delete.
    [ ] Store the information as a CSV text file in the root directory to read on load time and to write changes to.
    [ ] Console based user inteface
    [ ] Unit testing.
    [ ] Bugfixing.
*/
using System.Globalization;
using static System.Console;

namespace InventorySystem
{
    public class Product
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; } = 0;
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public void ChangePrice(decimal price) => Price = price;
    }

    public class Inventory
    {
        public Dictionary<Product,int>? Contents { get; private set; }

        public void Add(Product product, int stock = 0)
        {
            try
            {
                Contents.Add(product, stock);
                WriteLine("Product: {0} added to inventory.", product.Name);
                return;
            }
            catch (ArgumentException)
            {
                WriteLine("This product is already in the inventory.\nTry editing its price or stock instead.");
                ReadKey();
                return;
            }
        }
        public void Remove(Product product)
        {
            try
            {
                Contents.Remove(product);
                WriteLine("Product: {0} deleted.", product.Name);
            }
            catch (ArgumentException)
            {
                WriteLine("This product doesn't exist in the inventory.");
                ReadKey();
                return;

            }
        }
        public void ChangeStock(Product product, int stock)
        {
            Contents[product] = stock;
        }
    }

    public class FileOperator
    {
        public static CultureInfo Culture { get; set; } = new("es-ES");
        public static string PathName { get; set; } = "inventory.csv";
        public static Inventory LoadInventory()
        {
            if (string.IsNullOrWhiteSpace(PathName))
            {
                throw new ArgumentException($"'{nameof(PathName)}' cannot be null or whitespace.", nameof(PathName));
            }

            Inventory inventoryInFile = new();
            if (File.Exists(PathName))
            {
                using StreamReader inventoryFile = new(PathName, true);
                while (inventoryFile.Peek() >= 0)
                {
                    string? line = inventoryFile.ReadLine();
                    if (line != null)
                    {
                        string[] pair = line.Split(";");
                        Product product = new(pair[0], decimal.Parse(pair[1], NumberStyles.AllowDecimalPoint, Culture));
                        inventoryInFile.Add(product);
                    }
                    else
                    {
                        break;
                    }
                }
                return inventoryInFile;
            }
            else
            {
                File.CreateText(PathName);
                return inventoryInFile;
            }
        }
        public static void WriteInventory(Inventory inventory)
        {
            if (string.IsNullOrWhiteSpace(PathName))
            {
                throw new ArgumentException($"'{nameof(PathName)}' cannot be null or whitespace.", nameof(PathName));
            }
            using StreamWriter inventoryFile = new(PathName);
            foreach (var product in inventory) // TODO how do I get an enumerator for Inventory?
            {
                inventoryFile.WriteLine("{0};{1}", product.Contents.Name, product.Contents.Price.ToString(Culture));
            }
            return;
        }
    }
    public class MenuControl
    {
        enum ProgramState
        {
            MainMenu,
            DisplayInventory,
            AddProduct,
            DeleteProduct,
            EditProductPrice,
            EditProductStock,
            Exit
        }

        private static readonly string welcomeMessage = "Welcome to Dynamics Inventory System.";
        private static readonly string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n [4] Edit price of product.\n [5] Exit Inventory system";
        private static readonly string optionErrorMessage = "Option must be a number";
        private static readonly string emptyErrorMessage = "You have to write a valid option.";
        private static readonly string exitMessage= "Thanks for using our Inventory System.\nHave a great day!";
        private static readonly string numberErrorMessage = "You must enter a number.";

        private ProgramState CurrentState { get; set; } = ProgramState.MainMenu;
        public Inventory CurrentInventory { get; set; }
        public MenuControl(Inventory inventory)
        {
            CurrentInventory = inventory;
            CurrentState = MenuUI(CurrentState);
        }

        public ProgramState MenuUI(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.MainMenu:
                    Clear();
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
                case ProgramState.EditProduct:
                    EditPrice();
                    return ProgramState.MainMenu;
                default:
                    return ProgramState.Exit;
            }

        }
        private ProgramState ChangeStateMainMenu(int option)
        {
            return option switch
            {
                1 => ProgramState.DisplayInventory,
                2 => ProgramState.AddProduct,
                3 => ProgramState.DeleteProduct,
                4 => ProgramState.EditProductPrice,
                5 => ProgramState.EditProductStock,
                >= 6 => ProgramState.Exit,
                _ => ProgramState.MainMenu,
            };
        }
        private static string GetAText()
        {
            string? text = null;
            while (text == null)
            {
                text = ReadLine();
                if (text != null)
                {
                    break;
                }
                else
                {
                    WriteLine(emptyErrorMessage);
                }
            }           
            return text;
        }
        private static decimal GetPrice()
        {
            decimal price;
            while (!decimal.TryParse(ReadLine(), out price))
            {
                WriteLine(numberErrorMessage);
            }
            return price;
        }
        private static int GetMenuOption()
        {
            while (true)
            {
                string? userAnswer = ReadLine();
                if (userAnswer == null)
                {
                    WriteLine(emptyErrorMessage);
                    continue;
                }
                if (!int.TryParse(userAnswer, out int option))
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
        static void AppendNewProduct(Inventory inventory)
        {
            Clear();
            WriteLine("Add product mode.");
            string name;
            Write("Product [Name]: ");
            name = GetAText();
            
            Write("Product [Price]: ");
            decimal price = GetPrice();

            try
            {
                productInventory.Add(name, price);
            }
            catch (ArgumentException)
            {
                WriteLine("This product is already in the inventory.\nTry a different option instead.");
                ReadKey();
                return;
            }


    }

    }
    public class InventoryUI
    {
        static void Main()
        {
            Inventory inventory = FileOperator.LoadInventory();
            MenuControl inventoryMenu = new(inventory);
            Clear();
            WriteFile();
            WriteLine(exitMessage);
            ReadKey();
        }

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
            
            if (CheckProduct(product))
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


        static void EditPrice()
        {
            Clear();
            WriteLine("Edit Price mode.");
            Write("Product: ");
            string product = GetAText();
            if (CheckProduct(product))
            {
                Write("New price: ");
                decimal price = GetPrice();
                productInventory[product] = price;
            }

            return;
        }

        static bool CheckProduct(string product)
        {
            if (!productInventory.ContainsKey(product))
            {
                WriteLine("Product is not in the inventory.");
                ReadKey();
                return false;
            }
            else
            {
                return true;
            }
        }
}