using System;
using System.Collections.Generic;
using NHibernate.Search.Attributes;
using Rhino.Security;

namespace Model
{
	public class User : IUser
	{
		public User()
		{
			Posts = new HashSet<Post>();
			Blogs = new HashSet<Blog>();
		}

		public virtual ICollection<Blog> Blogs { get; set; }

		public virtual ICollection<Post> Posts { get; set; }

		public virtual int Id { get; set; }

		public virtual byte[] Password { get; set; }

		[Field]
		public virtual string Email { get; set; }

		[Field]
		public virtual string Username { get; set; }

		public virtual DateTime CreatedAt { get; set; }

		public virtual string Bio { get; set; }

		public virtual SecurityInfo SecurityInfo
		{
			get { return new SecurityInfo(Username, Id); }
		}
	}
}