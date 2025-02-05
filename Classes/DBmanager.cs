using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConsoleShoppen.Classes;

public class DBManager
{
    private readonly ShopDbContext _dbContext;

    public DBManager(ShopDbContext db)
    {
        _dbContext = db;
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    // Product Operations

    public async Task<Product?> GetProduct(int productId)
    {
        return await _dbContext.Products.Where(product => product.Id == productId).FirstOrDefaultAsync();
    }
    public async Task AddProductAsync(Product newProduct)
    {
        await _dbContext.Products.AddAsync(newProduct);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
        {
            var query = @"
                SELECT * FROM Products
                WHERE CONTAINS((Name, Description), @SearchTerm)";

            return await connection.QueryAsync<Product>(query, new { SearchTerm = $"\"*{searchTerm}*\"" });
        }
    }
    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await _dbContext.Products.Include(p => p.ProductCategories)
                                        .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _dbContext.Products.Include(p => p.ProductCategories)
                                        .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId))
                                        .ToListAsync();
    }

    // Customer Operations
    public async Task<Customer?> GetCustomerAsync(int customerId)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
    }
    public async Task<ShippingMethod?> GetShippingMethodAsync(int shippingMethodId)
    {
        return await _dbContext.ShippingMethods.FirstOrDefaultAsync(sm => sm.Id == shippingMethodId);
    }
    public async Task<PaymentMethod?> GetPaymentMethodAsync(int paymentMethodId)
    {
        return await _dbContext.PaymentMethods.FirstOrDefaultAsync(pm => pm.Id == paymentMethodId);
    }

    // ShoppingCart Operations
    public async Task CreateShoppingCartAsync(ShoppingCart order)
    {
        await _dbContext.ShoppingCarts.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }
    public async Task AddProductToCartAsync(ShoppingCart _currentShoppingCart, int productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            Console.WriteLine("Produkten kunde inte hittas.");
            return;
        }
        var itemInCart = _currentShoppingCart.Items
            .FirstOrDefault(item => item.ProductId == productId);

        if (itemInCart == null)
        {
            itemInCart = new ShoppingCartItem { ShoppingCartId = _currentShoppingCart.Id, ProductId = productId, Quantity = 1 };
            _currentShoppingCart.Items.Add(itemInCart);
        }
        else
        {
            itemInCart.Quantity += 1;
        }

        await _dbContext.SaveChangesAsync();
    }
    // Order Operations
    public async Task CreateOrderAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        Console.WriteLine("Ordern är lagd");
    }

    //Helper methods
    public async Task<string> GetCategoryNameAsync(int categoryId)
    {
        return (await _dbContext.ProductCategories.FirstOrDefaultAsync(pc => pc.Id == categoryId))?.Name ?? "Okänd kategori";
    }
    public async Task<List<string>> GetCategoryIdAndNamesAsync()
    {
        return await _dbContext.ProductCategories.Select(pc => $"{pc.Id}. {pc.Name}").ToListAsync();
    }

    public async Task<List<string>> GetPaymentMethodNamesAsync()
    {
        return await _dbContext.PaymentMethods.Select(pm => $"{pm.Id}. {pm.Name}").ToListAsync();
    }

    public async Task<List<string>> GetShippingMethodNamesAsync()
    {
        return await _dbContext.ShippingMethods.Select(sm => $"{sm.Id}. {sm.Name}").ToListAsync();
    }

    public async Task<List<string>> GetCountryNamesAsync()
    {
        return await _dbContext.Countries.Select(country => $"{country.Id}. {country.Name}").ToListAsync();
    }

    public async Task AddCustomerAsync(Customer currentCustomer)
    {
       await _dbContext.Customers.AddAsync(currentCustomer);
    }

    internal async Task<string> GetOrderWithLargestValueAsync()
    {
        throw new NotImplementedException();
    }

    internal async Task<Supplier> GetSupplierWithMostProductsInStockAsync()
    {
        throw new NotImplementedException();
    }

    internal async Task<string> GetCustomersPerCountryAsync()
    {
        throw new NotImplementedException();
    }
}
