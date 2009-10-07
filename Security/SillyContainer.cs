using System;
using System.Collections.Generic;
using Model;
using NHibernate;
using Rhino.Security.Interfaces;
using Rhino.Security.Services;

namespace Security
{
	public class SillyContainer : Microsoft.Practices.ServiceLocation.ServiceLocatorImplBase
	{
		private readonly Func<ISession> sessionProvider;

		public SillyContainer(Func<ISession> sessionProvider)
		{
			this.sessionProvider = sessionProvider;
		}

		protected override object DoGetInstance(Type serviceType,
		                                        string key)
		{
			if (serviceType == typeof(IAuthorizationService))
				return new AuthorizationService(GetInstance<IPermissionsService>(),
				                                GetInstance<IAuthorizationRepository>());
			if (serviceType == typeof(IAuthorizationRepository))
				return new AuthorizationRepository(sessionProvider());
			if (serviceType == typeof(IPermissionsBuilderService))
				return new PermissionsBuilderService(sessionProvider(), GetInstance<IAuthorizationRepository>());
			if (serviceType == typeof(IPermissionsService))
				return new PermissionsService(GetInstance<IAuthorizationRepository>(), sessionProvider());
			if (serviceType == typeof(IEntityInformationExtractor<Blog>))
				return new BlogInfromationExtractor(sessionProvider());
			throw new NotImplementedException();		
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			throw new NotImplementedException();
		}
	}
}