using System;
using System.Collections.Generic;
using NHibernate.Search.Attributes;

namespace Model
{
    public class Blog
    {
    	public Blog()
    	{
    		Tags = new HashSet<Tag>();
    		Users = new HashSet<User>();
    		Posts = new HashSet<Post>();
			SecurityKey = Guid.NewGuid();
		}

    	public virtual int Id { get; set; }

		public virtual Guid SecurityKey { get; set; }

    	public virtual ICollection<Post> Posts { get; set; }

    	public virtual ICollection<User> Users { get; set; }

    	public virtual ICollection<Tag> Tags { get; set; }

		[Field]
    	public virtual string Title { get; set; }

		[Field]
		public virtual string Subtitle { get; set; }

		public virtual string LogoImage { get; set; }

    	public virtual bool AllowsComments { get; set; }

    	public virtual DateTime CreatedAt { get; set; }
    }
}