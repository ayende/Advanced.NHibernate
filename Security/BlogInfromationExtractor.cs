using System;
using Model;
using NHibernate;
using NHibernate.Criterion;
using Rhino.Security.Interfaces;

namespace Security
{
	public class BlogInfromationExtractor : IEntityInformationExtractor<Blog>
	{
		private readonly ISession session;

		public BlogInfromationExtractor(ISession session)
		{
			this.session = session;
		}

		public Guid GetSecurityKeyFor(Blog entity)
		{
			return entity.SecurityKey;
		}

		public string GetDescription(Guid securityKey)
		{
			var result = session.CreateCriteria<Post>()
				.Add(Restrictions.Eq(SecurityKeyPropertyName, securityKey))
				.UniqueResult();
			if (result == null)
				return "could not find entity";
			return result.ToString();
		}

		public string SecurityKeyPropertyName
		{
			get { return "SecurityKey"; }
		}
	}
}