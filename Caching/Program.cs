using System;
using HibernatingRhinos.NHibernate.Profiler.Appender;
using log4net;
using Model;
using NHibernate.Cfg;
using System.Linq;

namespace Caching
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

				var factory = configuration.BuildSessionFactory();

				using(var s = factory.OpenSession())
				{
					var blogs = s.CreateQuery("from Blog")
						.SetCacheable(true)
						.List<Blog>();

					Console.WriteLine(blogs.Count);
				}

				using(var s = factory.OpenSession())
				using(s.BeginTransaction())
				{
					s.Save(new Blog
					{
						CreatedAt = DateTime.Now,
						Title = "a"
					});

					s.Transaction.Commit();
				}

				using (var s = factory.OpenSession())
				{
					var blogs = s.CreateQuery("from Blog")
						.SetCacheable(true)
						.Enumerable();

					foreach (Blog blog in blogs)
					{
						Console.WriteLine(blog.Title);
					}
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