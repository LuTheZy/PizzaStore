using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedPizzaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
migrationBuilder.InsertData(
                table: "pizzas",
                schema: "public",
                columns: new[] { "Name", "Description", "Price", "IsVegetarian", "CreatedAt" },
                values: new object[,]
                {
                    { "Margherita", "Classic pizza with tomato sauce, mozzarella, and fresh basil", 89.99m, true, DateTime.UtcNow },
                    { "Regina", "Ham and mushroom pizza - a South African favourite", 109.99m, false, DateTime.UtcNow },
                    { "Vegetarian Supreme", "Mushrooms, onions, peppers, olives, and feta cheese", 119.99m, true, DateTime.UtcNow },
                    { "Meat Supreme", "Pepperoni, ham, ground beef, bacon, and Italian sausage", 159.99m, false, DateTime.UtcNow },
                    { "Hawaiian", "Ham and pineapple - controversial but beloved", 99.99m, false, DateTime.UtcNow },
                    { "Tandoori Chicken", "Spicy tandoori chicken with peppers and onions", 139.99m, false, DateTime.UtcNow },
                    { "Four Seasons", "Divided into four sections: mushrooms, olives, artichokes, and ham", 129.99m, false, DateTime.UtcNow },
                    { "Bacon & Feta", "Crispy bacon, feta cheese, and fresh avocado", 134.99m, false, DateTime.UtcNow },
                    { "Mediterranean", "Olives, feta, sundried tomatoes, and fresh rocket", 124.99m, true, DateTime.UtcNow },
                    { "Mexican Fiesta", "Spicy ground beef, jalapeños, onions, and peppers", 144.99m, false, DateTime.UtcNow },
                    { "Four Cheese", "Mozzarella, cheddar, feta, and parmesan cheese blend", 129.99m, true, DateTime.UtcNow },
                    { "BBQ Chicken", "Grilled chicken, BBQ sauce, onions, and peppers", 139.99m, false, DateTime.UtcNow },
                    { "Pepperoni Passion", "Double pepperoni and extra mozzarella cheese", 129.99m, false, DateTime.UtcNow },
                    { "Curry Chicken", "Butter chicken curry sauce, chicken, onions, and peppers", 149.99m, false, DateTime.UtcNow },
                    { "Seafood Delight", "Prawns, calamari, mussels, and garlic butter", 179.99m, false, DateTime.UtcNow },
                    { "Spinach & Feta", "Creamy spinach, feta cheese, and garlic", 114.99m, true, DateTime.UtcNow },
                    { "Sweet Chilli Chicken", "Chicken, sweet chilli sauce, peppers, and pineapple", 139.99m, false, DateTime.UtcNow },
                    { "Mushroom & Truffle", "Mixed mushrooms with truffle oil and thyme", 159.99m, true, DateTime.UtcNow },
                    { "Biltong & Avo", "South African dried beef with fresh avocado", 169.99m, false, DateTime.UtcNow },
                    { "Garlic Prawn", "Prawns, garlic butter, and fresh herbs", 169.99m, false, DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "pizzas",
                schema: "public",
                keyColumn: "Name",
                keyValues: new object[] {
                    "Margherita", "Regina", "Vegetarian Supreme", "Meat Supreme", "Hawaiian",
                    "Tandoori Chicken", "Four Seasons", "Bacon & Feta", "Mediterranean", "Mexican Fiesta",
                    "Four Cheese", "BBQ Chicken", "Pepperoni Passion", "Curry Chicken", "Seafood Delight",
                    "Spinach & Feta", "Sweet Chilli Chicken", "Mushroom & Truffle", "Biltong & Avo", "Garlic Prawn"
                });
        }
    }
}
