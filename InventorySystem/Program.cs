// See https://aka.ms/new-console-template for more information

/*
    TODO: [x] Refactor this program to use the OOP patterns.
    [x] CRUD: Create, Read, Update, Delete.
    [x] Store the information as a CSV text file in the root directory to read on load time and to write changes to.
    [x] Console based user inteface
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
            if(!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            else
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            if (price < 0)
            {
                throw new ArgumentException("Price can't be negative.");
            }
            else
            {
                Price = price;
            }
        }
        public void ChangePrice(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Price can't be negative.");
            }
            else
            {
                Price = value;
            }
        }

        public bool IsEqual(string name)
        {
            if (name == Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEqual(decimal value)
        {
            if (value == Price)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Inventory
    {
        public Dictionary<Product,int> Catalog { get; set; }
        public Inventory()
        {
            if (Catalog is not null)
            {
                return;
            }
            else
            {
                Catalog = new();
            }
        }
        public void AddProduct(Product product, int stock = 1, bool userInteraction = false)
        {
                try
                {
                    Catalog.Add(product, stock);
                    WriteLine("Product: {0} added to inventory.", product.Name);
                }
                catch (ArgumentException)
                {
                    WriteLine("This product is already in the inventory.\nTry editing its price or stock instead.");
                }
                if (userInteraction)
                {
                    ReadKey();
                }
                return;
        }
        public void RemoveProduct(Product product, bool userInteraction = false)
        {
            try
            {
                Catalog.Remove(product);
                WriteLine("Product {0} was removed from the inventory.", product.Name);
            }
            catch (ArgumentException)
            {
                WriteLine("This product doesn't exist in the inventory.");
            }
            if (userInteraction)
            {
                ReadKey();
            }
            return;
        }
        public void RemoveProduct(string name, bool userInteraction = false)
        {
            RemoveProduct(GetProduct(name), userInteraction);
            return;
        }
        public void ChangeStock(Product product, int stock)
        {
                Catalog[product] = stock;
                return;
        }
        public void ChangeStock(string name, int stock)
        {
            try
            {
                Product product = GetProduct(name);
                ChangeStock(product, stock);
                WriteLine("Product {0} stock changed to {1:N0}", name, stock);
                ReadKey();
                return;
            }
            catch (ArgumentException)
            {
                WriteLine("Product is not in the inventory.");
                ReadKey();
                return;
            }
        }
        public Product GetProduct(string name)
        {
            foreach (var item in Catalog.Keys)
            {
                if (item.IsEqual(name))
                {
                    return item;
                }
                else
                {
                    continue;
                }
            }
            throw new ArgumentException("Product is not in inventory.");
        }
        public void ChangePrice(Product product, decimal price) // It's not possible to change the key, for both name and price constitute the dictionary key. What we can do is to delete the old product and add it again, with the new price.
        {
            int preserveStockValue = Catalog[product];
            Catalog.Remove(product);
            product.ChangePrice(price);
            Catalog.Add(product, preserveStockValue);
            return;
        }
    }

    public class FileOperator
    {
        public static CultureInfo Culture { get; set; } = new("es-ES");
        public static string PathName { get; set; } = "inventory.csv";
        public static Inventory LoadInventory(Inventory inventory)
        {
            if (string.IsNullOrWhiteSpace(PathName))
            {
                throw new ArgumentException($"'{nameof(PathName)}' cannot be null or whitespace.", nameof(PathName));
            }

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
                        inventory.AddProduct(product, int.Parse(pair[2]));
                    }
                    else
                    {
                        break;
                    }
                }
                return inventory;
            }
            else
            {
                File.CreateText(PathName);
                return inventory;
            }
        }
        public static void WriteInventory(Inventory inventory)
        {
            if (string.IsNullOrWhiteSpace(PathName))
            {
                throw new ArgumentException($"'{nameof(PathName)}' cannot be null or whitespace.", nameof(PathName));
            }
                using StreamWriter inventoryFile = new(PathName);
                foreach (var product in inventory.Catalog)
                {
                    inventoryFile.WriteLine("{0};{1};{2}", product.Key.Name, product.Key.Price.ToString(Culture), inventory.Catalog[product.Key]);
                }
            return;
        }
    }
    public class MenuControl
    {
        public enum ProgramState
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
        private static readonly string mainMenuOptions = """
            Choose an option and press [Enter]:
             [1] Display all products in inventory.
             [2] Add a new product.
             [3] Delete a product.
             [4] Edit price of product.
             [5] Edit stock of product.
             [6] Exit Inventory system.
            """;
        private static readonly string optionErrorMessage = "Option must be a number";
        private static readonly string emptyErrorMessage = "You have to write a valid option.";
        private static readonly string exitMessage= "Thanks for using our Inventory System.\nHave a great day!";
        private static readonly string numberErrorMessage = "You must enter a number.";

        public ProgramState CurrentState { get; private set; } = ProgramState.MainMenu;
        public Inventory CurrentInventory { get; set; }
        public MenuControl(Inventory inventory)
        {
            CurrentInventory = inventory;
            while (CurrentState != ProgramState.Exit)
            {
                CurrentState = MenuUI(CurrentState);
            }
            Clear();
            WriteLine(exitMessage);
            ReadKey();
        }

        public ProgramState MenuUI(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.MainMenu:
                    MainMenu();
                    return ChangeStateMainMenu(GetMenuOption());
                case ProgramState.AddProduct:
                    AppendNewProduct(CurrentInventory);
                    return ProgramState.MainMenu;
                case ProgramState.DisplayInventory:
                    DisplayInventory(CurrentInventory);
                    return ProgramState.MainMenu;
                case ProgramState.DeleteProduct:
                    DeleteProduct(CurrentInventory);
                    return ProgramState.MainMenu;
                case ProgramState.EditProductPrice:
                    EditPrice(CurrentInventory);
                    return ProgramState.MainMenu;
                case ProgramState.EditProductStock:
                    EditStock(CurrentInventory);
                    return ProgramState.MainMenu;
                default:
                    return ProgramState.Exit;
            }

        }

        private void MainMenu()
        {
            Clear();
            WriteLine(welcomeMessage);
            WriteLine(mainMenuOptions);
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
            Write("Product [Name]: ");
            string name = GetAText();
            Write("Product [Price]: ");
            decimal price = GetPrice();
            Product operativeProduct = new(name, price);

            try
            {
                inventory.AddProduct(operativeProduct, userInteraction: true);
            }
            catch (ArgumentException)
            {
                WriteLine("This product is already in the inventory.\nTry a different option instead.");
                ReadKey();
                return;
            }

            Write("Current product [Stock]: ");
            inventory.ChangeStock(operativeProduct, GetInt());
            return;
        }
        static int GetInt()
        {
            int number;
            while (!int.TryParse(ReadLine(), out number))
            {
                WriteLine(numberErrorMessage);
            }
            return number;
        }
        static void DisplayInventory(Inventory inventory)
        {
            Clear();
            WriteLine("List of products and prices.\n");
            WriteLine("|       Product name       |  Price  |  Stock  |");
            WriteLine("|--------------------------|---------|---------|");
            foreach (var product in inventory.Catalog)
            {
                WriteLine("|{0,-26}|${1,8:N2}|{2,9:N0}|", product.Key.Name, product.Key.Price, product.Value);
                WriteLine("|--------------------------|---------|---------|");
            }
            ReadKey();
            return;
        }
        static void DeleteProduct(Inventory inventory)
        {
            Clear();
            WriteLine("Delete product mode.");
            Write("Product to delete: ");
            try
            {
                Product product = inventory.GetProduct(GetAText());
                WriteLine("You're about to delete {0}.\nAre you sure you want to continue?\n[1] Yes\n[2] No", product.Name);
                switch (GetMenuOption())
                {
                    case 1:
                        break;
                    case >= 2:
                    default:
                        WriteLine("Nothing was deleted.");
                        ReadKey();
                        return;
                }
                inventory.RemoveProduct(product, true);
                return;
            }
            catch (ArgumentException)
            {
                WriteLine("Product not found in inventory.");
            }
            return;
        }
        static void EditPrice(Inventory inventory)
        {
            Clear();
            WriteLine("Edit Price mode.");
            Write("Product: ");
            try
            {
                Product product =  inventory.GetProduct(GetAText());
                Write("New price: ");
                inventory.ChangePrice(product, GetPrice());
            }
            catch (ArgumentException)
            {
                WriteLine("Product doesn't exist in inventory.");
            }
            return;
        }
        public void EditStock(Inventory inventory)
        {
            Clear();
            WriteLine("Edit stock mode");
            Write("Product to edit: ");
            string name = GetAText();
            WriteLine();
            Write("New stock in inventory: ");
            try
            {
                inventory.Catalog[inventory.GetProduct(name)] = GetInt();
            }
            catch (ArgumentException)
            {
                WriteLine("Product not in inventory.");
                ReadKey();
            }
            return;
        }

        // static bool CheckProduct(Product product)
        // {
        //     if (!productInventory.ContainsKey(product))
        //     {
        //         WriteLine("Product is not in the inventory.");
        //         ReadKey();
        //         return false;
        //     }
        //     else
        //     {
        //         return true;
        //     }
        // }

    }
    public class InventorySystem
    {
        static void Main()
        {
            Inventory inventory = new();
            inventory = FileOperator.LoadInventory(inventory);
            _ = new MenuControl(inventory);
            FileOperator.WriteInventory(inventory);
        }
    }
    
    }