# CodeMaster Homework - Inventory System

Write a basic product inventory system that allows the user to enter product names and product prices and save that data to a file.

The program will load that data again, if the file exists when it starts.

The program will allow the user to enter new products, remove existing products and display current products.

The program will use enums to track state, and should use different methods correctly to keep individual method line count down. No method should be larger than a single screen of code(?).

## Object model

- Inventory
    - Product
    - Stock
    - Add product
    - Increase stock
    - Reduce stock
    - Delete product
    - Report inventory
- Catalog
    - Product
    - Add product
    - Remove product
    - Display whole catalog of products
- Product
    - Name
    - Price
    - Create product
    - Change price
    - Delete product
- MenuControl
    - Options
        - Prompts
    - Take option
    - Take text
    - Take number
    - Return result
- FileOperator
    - File path
    - Load
    - Write