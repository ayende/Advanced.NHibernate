using System;
using HibernatingRhinos.NHibernate.Profiler.Appender;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Rhino.Security;
using Rhino.Security.Interfaces;

namespace Security
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

				Rhino.Security.Security.Configure<User>(configuration, SecurityTableStructure.Prefix);

				//new SchemaExport(configuration).Create(true, true);
				var factory = configuration.BuildSessionFactory();

				ISession currentSession = null;
				var container = new SillyContainer(() => currentSession);
				ServiceLocator.SetLocatorProvider(() => container);

				//using (currentSession = factory.OpenSession())
				//using (currentSession.BeginTransaction())
				//{
				//    var authorizationRepository = ServiceLocator.Current.GetInstance<IAuthorizationRepository>();
				//    authorizationRepository.CreateOperation("/Blog/View");
				//    authorizationRepository.CreateUsersGroup("Authors");

				//    currentSession.Transaction.Commit();
				//}

				//using (currentSession = factory.OpenSession())
				//using (currentSession.BeginTransaction())
				//{
				//    var user = new User
				//    {
				//        CreatedAt = DateTime.Now,
				//        Username = "ayende"
				//    };

				//    currentSession.Save(user);

				//    var blog = new Blog
				//    {
				//        CreatedAt = DateTime.Now,
				//        Title = "Ayende @ Rahien"
				//    };
				//    currentSession.Save(blog);

				//    currentSession.Transaction.Commit();
				//}

				//using (currentSession = factory.OpenSession())
				//using (currentSession.BeginTransaction())
				//{
				//    var builderService = ServiceLocator.Current.GetInstance<IPermissionsBuilderService>();
				//    var authorizationRepository = ServiceLocator.Current.GetInstance<IAuthorizationRepository>();

				//    var user = currentSession.Get<User>(1);
				//    authorizationRepository.AssociateUserWith(user, "Authors");

				//    var blog = currentSession.Get<Blog>(1);

				//    builderService.Allow("/Blog/View")
				//        .For("Authors")
				//        .On(blog)
				//        .DefaultLevel()
				//        .Save();

				//    currentSession.Transaction.Commit();
				//}

				using (currentSession = factory.OpenSession())
				using (currentSession.BeginTransaction())
				{
					var authorizationService = ServiceLocator.Current.GetInstance<IAuthorizationService>();

					var user = currentSession.Get<User>(1);
					var blog = currentSession.Get<Blog>(1);

					var criteria = currentSession.CreateCriteria<Blog>();

					var information = authorizationService
						.GetAuthorizationInformation<Blog>(user, blog, 
						"/Blog/View"
						);

					Console.WriteLine(information.ToString());

					criteria.List();

					currentSession.Transaction.Commit();
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