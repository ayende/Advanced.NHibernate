using System;
using HibernatingRhinos.NHibernate.Profiler.Appender;
using log4net;
using Model;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace Futures
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
					var blogs = s.CreateCriteria<Blog>()
						.Future<Blog>();

					var last15Posts = s.CreateCriteria<Post>()
						.AddOrder(Order.Desc("PostedAt"))
						.SetMaxResults(15)
						.Future<Post>();

					var commentsCount = s.CreateCriteria<Comment>()
						.SetProjection(Projections.RowCount())
						.FutureValue<int>();

					foreach (var blog in blogs)
					{
						Console.WriteLine("Blog: {0}", blog.Title);
					}

					foreach (var post in last15Posts)
					{
						Console.WriteLine("Post: {0}", post.Title);
					}

					Console.WriteLine("Comments Count: {0}", 
						commentsCount.Value);
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