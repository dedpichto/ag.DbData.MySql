
// add section to settings file (optional)
{
  "DbDataSettings": {
    "AllowExceptionLogging": false // default is "true" 
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
		services.AddAgMySql(config.GetSection("DbDataSettings"));
		// or
		services.AddAgMySql(opts =>
        {
            opts.AllowExceptionLogging = false; 
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

***************************************************************************************************