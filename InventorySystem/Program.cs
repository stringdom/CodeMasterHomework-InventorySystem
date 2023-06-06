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
        public decimal Price { get; private set; }
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

    public class InventoryUI
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

        public static CultureInfo culture = new CultureInfo("es-ES");

        private static Dictionary<string, decimal> productInventory = new Dictionary<string, decimal>();
        static string welcomeMessage = "Welcome to Dynamics Inventory System.";
        static string mainMenuOptions = "Choose an option and press [Enter]:\n [1] Display all products in inventory.\n [2] Add a new product.\n [3] Delete a product.\n [4] Edit price of product.\n [5] Exit Inventory system";
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
                case ProgramState.EditProduct:
                    EditPrice();
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
                case 4:
                    return ProgramState.EditProduct;
                case >= 5:
                    return ProgramState.Exit;
                default:
                    return ProgramState.MainMenu;
            }
        }

        static void AppendNewProduct()
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
                WriteLine("This product is already in the inventory.\nTry editing its price instead.");
                ReadKey();
                return;
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

        static string GetAText()
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

        static decimal GetPrice()
        {
            decimal price;
            while (!decimal.TryParse(ReadLine(), out price))
            {
                WriteLine("You must enter a number.");
            }
            return price;
        }
    }
}