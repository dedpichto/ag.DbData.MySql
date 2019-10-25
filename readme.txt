
// add section to settings file (optional)
{
  "MySqlDbDataSettings": {
    "AllowExceptionLogging": false, // optional, default is "true"
    "ConnectionString": "YOUR_CONNECTION_STRING" // optional
  }
}

***************************************************************************************************

// add appropriate usings
using ag.DbData.MySql.Extensions;
using ag.DbData.MySql.Factories;

***************************************************************************************************

// register services with extension method

		// ...
		services.AddAgMySql();
		// or
		services.AddAgMySql(config.GetSection("MySqlDbDataSettings"));
		// or
		services.AddAgMySql(opts =>
        {
            opts.AllowExceptionLogging = false; // optional
			opts.ConnectionString = YOUR_CONNECTION_STRING; // optional
        });

***************************************************************************************************

// inject IMySqlDbDataFactory into your classes

        private readonly IMySqlDbDataFactory _mySqlFactory;

        public MyClass(IMySqlDbDataFactory mySqlFactory)
        {
            _mySqlFactory = mySqlFactory;
        }

***************************************************************************************************

// MySqlDbDataObject implements IDisposable interface, so use it into 'using' directive

        using (var mySqlDbData = _mySqlFactory.Create(YOUR_CONNECTION_STRING))
        {
            using (var t = mySqlDbData.FillDataTable("SELECT * FROM YOUR_TABLE"))
            {
                foreach (DataRow r in t.Rows)
                {
                    Console.WriteLine(r[0]);
                }
            }
        }

// in case you have defined connection string in configuration setting you may call Create() method
// without parameter

        using (var mySqlDbData = _mySqlFactory.Create())
        {
            using (var t = mySqlDbData.FillDataTable("SELECT * FROM YOUR_TABLE"))
            {
                foreach (DataRow r in t.Rows)
                {
                    Console.WriteLine(r[0]);
                }
            }
        }

***************************************************************************************************