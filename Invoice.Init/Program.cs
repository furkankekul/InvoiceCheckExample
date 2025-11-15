using Invoice.DataAccess;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        InvoiceDbContext db = new InvoiceDbContext();   
        db.Database.Migrate();
        Console.WriteLine("Migration'lar başarıyla uygulandı!");
    }
}