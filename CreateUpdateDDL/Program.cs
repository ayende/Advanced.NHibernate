using System;
using HibernatingRhinos.NHibernate.Profiler.Appender;
using log4net;
using Model;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace CreateUpdateDDL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			NHibernateProfiler.Initialize();

			try
			{
				var configuration = new Configuration()
					.Configure("NHibernate.config");

				new SchemaExport(configuration).Create(true, true);
				//new SchemaUpdate(configuration).Execute(true, true);
				//new SchemaValidator(configuration).Validate();

				var factory = configuration.BuildSessionFactory();

				using(var s = factory.OpenSession())
				using(s.BeginTransaction())
				{
					for (int i = 0; i < 30; i++)
					{
						s.Save(new Blog
						{
							CreatedAt = DateTime.Now,
							Title = "a"
						});
					}

					s.Transaction.Commit();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			LogManager.Shutdown();
		}
	}
}