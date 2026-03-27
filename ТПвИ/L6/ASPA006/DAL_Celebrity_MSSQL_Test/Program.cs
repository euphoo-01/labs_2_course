using DAL_Celebrity_MSSQL;
string cn = @"Server=localhost;Initial Catalog=LES01;User Id=sa;Password=Mssql2007;TrustServerCertificate=True";
new Init(cn);
Init.Execute();
Console.WriteLine("БД Инициализирована!");