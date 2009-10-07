using System;
using HibernatingRhinos.NHibernate.Profiler.Appender;
using log4net;
using Lucene.Net.Analysis.Standard;
using Model;
using NHibernate.Cfg;
using NHibernate.Search.Store;

namespace Search
{
	public class Program
	{
		public static void Main(string[] args)
		{
			NHibernateProfiler.Initialize();
		
			try
			{
				var configuration = new Configuration()
					.Configure("NHibernate.config")
					.SetProperty("hibernate.search.default.indexBase", @".")
					.SetProperty("hibernate.search.default.directory_provider", typeof(FSDirectoryProvider).AssemblyQualifiedName)
					.SetProperty("hibernate.search.analyzer", typeof(StandardAnalyzer).AssemblyQualifiedName);

				var factory = configuration.BuildSessionFactory();

				//using (var s = factory.OpenSession())
				//using (s.BeginTransaction())
				//{
				//    var user = new User
				//    {
				//        CreatedAt = DateTime.Now,
				//        Username = "ayende",
				//        Email = "ayende@example.org",
				//    };
				//    s.Save(user);
				//    var blog = new Blog
				//    {
				//        CreatedAt = DateTime.Now,
				//        Users = { user },
				//        Title = "Ayende @ Rahien",
				//        Subtitle = "Send me a pull request for that"
				//    };

				//    s.Save(blog);

				//    var post = new Post
				//    {
				//        Blog = blog,
				//        Text = "blah blah",
				//        Title = "NHibernate",
				//        PostedAt = DateTime.Now,
				//        User = user
				//    };
				//    s.Save(post);

				//    s.Transaction.Commit();
				//}

				//for (int i = 0; i < 3; i++)
				{
					using (var s = factory.OpenSession())
					{
						var posts = NHibernate.Search.Search.CreateFullTextSession(s)
							.CreateFullTextQuery<Post>("Title:NHibernate User.Name:ayende")
							.List<Post>();

						foreach (var post in posts)
						{
							Console.WriteLine(post.Title);
						}
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